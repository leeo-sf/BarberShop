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

        public async Task<IActionResult> RegisterUser()
        {
            var genders = await _barberShopRepository.GetGenders();
            var accountCategorys = await _barberShopRepository.GetAccountCategory();
            var viewModel = new UserFormViewModelAdm { Genders = genders, AccountCategorys = accountCategorys };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(UserFormViewModelClient obj)
        {
            try
            {
                await _insertClient.Execute(obj);
                return RedirectToAction(nameof(Index));
            }
            //tratando erro caso algum dado esteja inválido
            catch (Exception ex)
            {
                TempData["ErrorRegister"] = ex.Message;
                //é necessário enviar para a view novamente a lista dos genêros
                var genders = await _barberShopRepository.GetGenders();
                var accountCategorys = await _barberShopRepository.GetAccountCategory();
                var viewModel = new UserFormViewModelAdm { Genders = genders, AccountCategorys = accountCategorys };
                return View(viewModel);
            }
        }
    }
}
