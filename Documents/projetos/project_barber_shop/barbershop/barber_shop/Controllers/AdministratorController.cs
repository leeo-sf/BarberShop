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

        public AdministratorController(
            IBarberShopRepository barberShopRepository
            )
        {
            _barberShopRepository = barberShopRepository;
        }

        public async Task<IActionResult> Index(string? cpf)
        {
            DashboardAdministratorViewModel viewModel = new DashboardAdministratorViewModel();
            if (cpf is not null)
            {
                var user = await _barberShopRepository.GetUserByCpf(cpf);
                viewModel.User = user;
            }
            else
            {
                var users = await _barberShopRepository.GetAllUsers();
                viewModel.Users = users;
            }
            return View(viewModel);
        }
    }
}
