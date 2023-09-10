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

        public AccessController(
         IBarberShopRepository barberShopRepository,
         IInsertClient insertClient
         ){
            _barberShopRepository = barberShopRepository;
            _insertClient = insertClient;
        }

        public async Task<IActionResult> Login()
        {
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

            if (claims.First().Value == EnumAccountCategory.ADM.ToString())
            {
                return RedirectToAction(nameof(Index), "Administrator");
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Register()
        {
            var genders = await _barberShopRepository.GetGenders();
            var viewModel = new UserFormViewModel { Genders = genders };
            if (User.Identity.IsAuthenticated)
            {
                if (User.Claims.First().Value == EnumAccountCategory.ADM.ToString())
                {
                    var accountCategories = await _barberShopRepository.GetAccountCategories();
                    viewModel.AccountCategories = accountCategories;
                }
            }
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Profile>> Register(UserFormViewModel obj)
        {
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
                var viewModel = new UserFormViewModel { Genders = genders };
                if (User.Identity.IsAuthenticated)
                {
                    if (User.Claims.First().Value == EnumAccountCategory.ADM.ToString())
                    {
                        var accountCategories = await _barberShopRepository.GetAccountCategories();
                        viewModel.AccountCategories = accountCategories;
                    }
                }
                return View(viewModel);
            }
        }

        //verifica se usuário já está logado
        private bool CheckLoggedIn()
        {
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
