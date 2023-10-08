using barber_shop.Extensions;
using barber_shop.Models;
using barber_shop.Models.Enums;
using barber_shop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace barber_shop.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IBarberShopRepository _barberShopRepository;

        public DashboardController(
            IBarberShopRepository barberShopRepository)
        {
            _barberShopRepository = barberShopRepository;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _barberShopRepository.GetUserLoggedInByCpf(User.Identity.Name);
            if (User.Claims.First().Value == nameof(EnumAccountCategory.BARBER))
            {
                var mySchedulesBarber = await _barberShopRepository.GetBarberSchedules(user.Id);
                return View(mySchedulesBarber);
            }
            var mySchedules = await _barberShopRepository.GetUserSchedules(user.Id);
            return View(mySchedules);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageAccount(string? cpf)
        {
            User user;
            cpf = cpf.FormatCpf();
            if (cpf is not null)
            {
                user = await _barberShopRepository.GetUserByCpf(cpf);
                return RedirectToAction("Index", "Administrator", user);
            }
            user = await _barberShopRepository.GetUserByCpf(User.Identity.Name);
            //retornar a view do cliente com os dados dele
            return View();
        }
    }
}
