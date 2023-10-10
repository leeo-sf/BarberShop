using barber_shop.Commands;
using barber_shop.Extensions;
using barber_shop.Models;
using barber_shop.Models.Enums;
using barber_shop.Models.ViewModel;
using barber_shop.Services;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace barber_shop.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IBarberShopRepository _barberShopRepository;
        private readonly IUpdatePassword _updatePassword;
        private readonly IEditAccountDetails _editAccountDetails;

        public DashboardController(
            IBarberShopRepository barberShopRepository,
            IUpdatePassword updatePassword,
            IEditAccountDetails editAccountDetails)
        {
            _barberShopRepository = barberShopRepository;
            _updatePassword = updatePassword;
            _editAccountDetails = editAccountDetails;
        }

        public async Task<IActionResult> Index()
        {
            Scheduling[] mySchedule;
            var user = await _barberShopRepository.GetUserLoggedInByCpf(User.Identity.Name);
            if (User.Claims.First().Value == nameof(EnumAccountCategory.BARBER))
            {
                mySchedule = await _barberShopRepository.GetBarberSchedules(user.Id);
            }
            else
            {
                mySchedule = await _barberShopRepository.GetUserSchedules(user.Id);
            }
            DashboardViewModel viewModel = new DashboardViewModel { User = user, Schedulings =  mySchedule };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageAccount(string? cpf)
        {
            User user;
            cpf = cpf.RemoveFormatCpf();
            if (cpf is not null)
            {
                user = await _barberShopRepository.GetUserByCpf(cpf);
                return RedirectToAction("Index", "Administrator", user);
            }
            user = await _barberShopRepository.GetUserByCpf(User.Identity.Name);
            //retornar a view do cliente com os dados dele
            return View();
        }

        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(User obj)
        {
            try
            {
                await _updatePassword.Excute(obj, User.Identity.Name);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorChange"] = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> EditAccountDetails(int id)
        {
            var genders = await _barberShopRepository.GetGenders();
            var user = await _barberShopRepository.GetUserById(id);
            var viewModel = new UserFormViewModel { User = user, Genders = genders };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccountDetails(UserFormViewModel obj, IFormFile Image)
        {
            if (Image is not null)
            {
                MemoryStream target = new MemoryStream();
                Image.CopyToAsync(target);
                byte[] img = target.ToArray();
                obj.User.Profile.Image = img;
            }
            try
            {
                await _editAccountDetails.Execute(obj, User.Identity.Name);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                TempData["ErrorEditAccount"] = ex.Message;
                var genders = await _barberShopRepository.GetGenders();
                var user = await _barberShopRepository.GetUserById(obj.User.Id);
                var viewModel = new UserFormViewModel { User = user, Genders = genders };
                return View(viewModel);
            }
        }
    }
}
