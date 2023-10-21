using barber_shop.Commands;
using barber_shop.Extensions;
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
        private readonly ISendMessagePromotional _sendMessagePromotional;

        public AdministratorController(
            IBarberShopRepository barberShopRepository,
            ISendMessagePromotional sendMessagePromotional
            )
        {
            _barberShopRepository = barberShopRepository;
            _sendMessagePromotional = sendMessagePromotional;
        }

        public async Task<IActionResult> Index(string? cpf)
        {
            DashboardAdministratorViewModel viewModel = new DashboardAdministratorViewModel();
            if (cpf is not null)
            {
                var user = await _barberShopRepository.GetUserByCpf(cpf);
                if (!(user is not null))
                {
                    TempData["Error"] = "CPF nao encontrado";
                    viewModel.Users = await _barberShopRepository.GetAllUsers();
                    return View(viewModel);
                }
                viewModel.User = user;
            }
            else
            {
                var users = await _barberShopRepository.GetAllUsers();
                viewModel.Users = users;
            }
            return View(viewModel);
        }

        public async Task<IActionResult> DefinePromotionalMessage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DefinePromotionalMessage(PromotionalMessage obj, IFormFile Image)
        {
            try
            {
                await _sendMessagePromotional.Execute(obj, Image);
                TempData["Success"] = "Email enviado para todos os clientes cadastrados";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View();
            }
        }
    }
}
