using barber_shop.Commands;
using barber_shop.Models;
using barber_shop.Models.Enums;
using barber_shop.Models.ViewModel;
using barber_shop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace barber_shop.Controllers
{
    [Authorize]
    public class SchedulingController : Controller
    {
        private readonly IBarberShopRepository _barberShopRepository;
        private readonly IInsertScheduling _insertScheduling;

        public SchedulingController(
            IBarberShopRepository barberShopRepository,
            IInsertScheduling insertScheduling
            )
        {
            _barberShopRepository = barberShopRepository;
            _insertScheduling = insertScheduling;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = nameof(EnumAccountCategory.ADMINISTRATOR))]
        public async Task<IActionResult> WeeklyScheduling()
        {
            //lista todos agendamentos da semana ou mês (definir)
            return View();
        }

        //método agendar (recebe um service caso a pessoa clique no serviço no botão agendar) (caso não clique o serviço irá vir vazio)
        public async Task<IActionResult> ToSchedule()
        {
            var schedulingTimes = await _barberShopRepository.GetAllSchedulingTimes();
            var barbers = await _barberShopRepository.GetAllBarbers();
            var services = await _barberShopRepository.GetServices();
            SchedulingFormViewModel viewModel = new SchedulingFormViewModel { Services = services, Barbers = barbers, SchedulingTimes = schedulingTimes };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToSchedule(SchedulingFormViewModel obj)
        {
            try
            {
                await _insertScheduling.Execute(obj, User.Identity.Name);
                //retorna algum redirect (página dizendo que foi agendado)
                return RedirectToAction(nameof(Index), "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorScheduling"] = ex.Message;
                var schedulingTimes = await _barberShopRepository.GetAllSchedulingTimes();
                var barbers = await _barberShopRepository.GetAllBarbers();
                var services = await _barberShopRepository.GetServices();
                SchedulingFormViewModel viewModel = new SchedulingFormViewModel { Services = services, Barbers = barbers, SchedulingTimes = schedulingTimes };
                return View(viewModel);
            }
        }
    }
}
