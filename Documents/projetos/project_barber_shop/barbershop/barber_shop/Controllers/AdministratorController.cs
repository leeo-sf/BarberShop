using barber_shop.Commands;
using barber_shop.Models;
using barber_shop.Models.Enums;
using barber_shop.Models.ViewModel;
using barber_shop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace barber_shop.Controllers
{
    [Authorize(Roles = nameof(EnumAccountCategory.ADMINISTRATOR))]
    public class AdministratorController : Controller
    {
        private readonly IBarberShopRepository _barberShopRepository;

        public AdministratorController(
            IBarberShopRepository barberShopRepository
            )
        {
            _barberShopRepository = barberShopRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Register()
        {
            //padronizando registro de um novo usuário
            //quando o adm for registrar um usuário será redirecionado para a mesma página de cadastro que client
            //mas com permissões diferentes (podendo escolher o perfil do usuário a ser cadastrado)
            return RedirectToAction("Register", "Access");
        }
    }
}
