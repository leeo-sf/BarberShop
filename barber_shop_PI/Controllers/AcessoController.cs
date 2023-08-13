using barber_shop_PI.Models;
using barber_shop_PI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace barber_shop_PI.Controllers
{
    public class AcessoController : Controller
    {
        private readonly LoginService _login;

        public AcessoController(LoginService login)
        {
            _login = login;
        }

        public async Task<IActionResult> Login()
        {
            //verifica se usuário já está logado
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Usuario obj)
        {
            var user = await _login.ValidaLogin(obj);
            if (user == null)
            {
                //tratar erro
                return View();
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Categoria.Descricao),
                new Claim("OtherProperties", "Example Role")
            };

            ClaimsIdentity claimsIdentify = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentify), properties);

            return RedirectToAction("Index", "Home");
        }
    }
}
