using barber_shop.Commands;
using barber_shop.Models;
using barber_shop.Models.Enums;
using barber_shop.Models.ViewModel;
using barber_shop.Services;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace barber_shop.Controllers
{
    [Authorize]
    public class SchedulingController : Controller
    {
        private readonly IBarberShopRepository _barberShopRepository;
        private readonly IInsertScheduling _insertScheduling;
        private readonly IGenerateReport _generateReport;
        private readonly IUpdateScheduling _updateScheduling;

        public SchedulingController(
            IBarberShopRepository barberShopRepository,
            IInsertScheduling insertScheduling,
            IGenerateReport generateReport,
            IUpdateScheduling updateScheduling
            )
        {
            _barberShopRepository = barberShopRepository;
            _insertScheduling = insertScheduling;
            _generateReport = generateReport;
            _updateScheduling = updateScheduling;
        }

        [Authorize(Roles = nameof(EnumAccountCategory.ADMINISTRATOR))]
        public async Task<IActionResult> Index()
        {
            var schedulings = await _barberShopRepository.GetAllSchedulings();
            return View(schedulings);
        }

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

        public async Task<IActionResult> ReSchedule(int id)
        {
            var schedulingById = await _barberShopRepository.GetSchedulingById(id);
            if (DateOnly.FromDateTime(schedulingById.Date) < DateOnly.FromDateTime(DateTime.Now))
            {
                TempData["ErroUpdateScheduling"] = "Nao e possivel reagendar este agendamento.";
                return RedirectToAction("Index", "Home");
            }
            var schedulingTimes = await _barberShopRepository.GetAllSchedulingTimes();
            var barbers = await _barberShopRepository.GetAllBarbers();
            var services = await _barberShopRepository.GetServices();
            var viewModel = new SchedulingFormViewModel { Services = services, Barbers = barbers, SchedulingTimes = schedulingTimes, Scheduling = schedulingById };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReSchedule(SchedulingFormViewModel obj)
        {
            try
            {
                await _updateScheduling.Execute(obj, User.Identity.Name);
                if (User.Claims.First().Value == EnumAccountCategory.ADMINISTRATOR.ToString())
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction("Index", "Dashboard");
            }
            catch (Exception ex)
            {
                var schedulingById = await _barberShopRepository.GetSchedulingById(obj.Scheduling.Id);
                var schedulingTimes = await _barberShopRepository.GetAllSchedulingTimes();
                var barbers = await _barberShopRepository.GetAllBarbers();
                var services = await _barberShopRepository.GetServices();
                var viewModel = new SchedulingFormViewModel { Services = services, Barbers = barbers, SchedulingTimes = schedulingTimes, Scheduling = schedulingById };
                TempData["ErroUpdateScheduling"] = ex.Message;
                return View(viewModel);
            }
        }

        [HttpPost]
        [Authorize(Roles = nameof(EnumAccountCategory.ADMINISTRATOR))]
        public async Task<ActionResult<Scheduling[]>> GenerateReport(
          DateTimeOffset mindate,
          DateTimeOffset maxdate
        )
        {
            await _generateReport.Execute(mindate, maxdate, User.Identity.Name);
            TempData["GeneratedReport"] = "Relatório gerado e enviado";
            return RedirectToAction(nameof(Index));
        }
    }
}
