using barber_shop.Models;
using barber_shop.Models.Enums;
using barber_shop.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace barber_shop.Controllers
{
    [Authorize]
    public class SchedulingController : Controller
    {
        public IActionResult Index()
        {
            //var name = User.Identity.Name;
            return View();
        }

        [Authorize(Roles = nameof(EnumAccountCategory.ADM))]
        public async Task<IActionResult> WeeklyScheduling()
        {
            //lista todos agendamentos da semana ou mês (definir)
            return View();
        }

        //método agendar (recebe um service caso a pessoa clique no serviço no botão agendar) (caso não clique o serviço irá vir vazio)
        public async Task<IActionResult> ToSchedule(Service? obj)
        {
            return View();
        }
    }
}
