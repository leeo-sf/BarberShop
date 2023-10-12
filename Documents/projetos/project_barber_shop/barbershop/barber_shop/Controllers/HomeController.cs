using barber_shop.Models;
using barber_shop.Models.ViewModel;
using barber_shop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace barber_shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBarberShopRepository _barberShopRepository;

        public HomeController(ILogger<HomeController> logger, IBarberShopRepository barberShopRepository)
        {
            _logger = logger;
            _barberShopRepository = barberShopRepository;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _barberShopRepository.GetServices();
            var barbers = await _barberShopRepository.GetAllBarbers();
            var viewModel = new HomeViewModel { Services = services, Barbers = barbers };
            return View(viewModel);
        }
    }
}