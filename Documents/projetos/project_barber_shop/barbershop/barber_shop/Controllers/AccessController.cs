using barber_shop.Commands;
using barber_shop.Models;
using barber_shop.Models.Enums;
using barber_shop.Models.ViewModel;
using barber_shop.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace barber_shop.Controllers
{
    public class AccessController : Controller
    {
        private readonly IInsertClient _insertClient;
        private readonly IBarberShopRepository _barberShopRepository;
        private bool __notLoggedIn;
        /* atributo que auxilia na view para mostrar botão de (Entrar ou Sair)
            Está sempre retornando true, que significa que o usuário não fez login, na view estará mostrando o botão (Entrar).
            Quando retornar false (no método POST Login) significa que o login foi feito pelo usuário.
            Sendo assim, na view deixa de mostrar para o usuário o botão (Entrar), passará a mostrar o botão (Sair).
        */

        public AccessController(
         IBarberShopRepository barberShopRepository,
         IInsertClient insertClient
         ){
            __notLoggedIn = true;
            _barberShopRepository = barberShopRepository;
            _insertClient = insertClient;
        }

        public async Task<IActionResult> Login()
        {
            TempData["logged"] = __notLoggedIn;
            if (CheckLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Profile obj)
        {
            var existingAccount = await _barberShopRepository.ValidateEmailPassword(obj);
            if (!existingAccount)
            {
                TempData["ErrorMessageLogin"] = "Email ou senha inválido.";
                return View();
            }

            var user = await _barberShopRepository.GetProfileEmail(obj.Email);
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, user.Category.Description.ToString().ToUpper())
            };

            ClaimsIdentity claimsIdentify = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentify), properties);
            // enviar objeto para view
            __notLoggedIn = false;
            TempData["logged"] = __notLoggedIn;
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Register()
        {
            TempData["logged"] = __notLoggedIn;
            var genders = await _barberShopRepository.GetGenders();
            var viewModel = new UserFormView { Genders = genders };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Profile>> Register(UserFormView obj)
        {
            TempData["logged"] = __notLoggedIn;
            try
            {
                await _insertClient.Execute(obj);
                return RedirectToAction("Login", "Access");
            }
            //tratando erro caso algum dado esteja inválido
            catch (Exception ex)
            {
                TempData["ErrorRegister"] = ex.Message;
                //é necessário enviar para a view novamente a lista dos genêros
                var genders = await _barberShopRepository.GetGenders();
                var viewModel = new UserFormView { Genders = genders };
                return View(viewModel);
            }
        }

        public bool CheckLoggedIn()
        {
            //verifica se usuário já está logado
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return true;
            }
            return false;
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Access");
        }
    }
}
