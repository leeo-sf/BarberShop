using barber_shop.Commands;
using barber_shop.Models.Enums;
using barber_shop.Models.ViewModel;
using barber_shop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace barber_shop.Controllers
{
    [Authorize(Roles = "ADM")]
    public class AdministratorController : Controller
    {
        private readonly IBarberShopRepository _barberShopRepository;
        private readonly IInsertClient _insertClient;

        public AdministratorController(
            IBarberShopRepository barberShopRepository,
            IInsertClient insertClient
            )
        {
            _barberShopRepository = barberShopRepository;
            _insertClient = insertClient;
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
