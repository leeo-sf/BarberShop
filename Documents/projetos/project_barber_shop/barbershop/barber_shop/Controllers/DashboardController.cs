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
        private readonly IInsertPhotoOfBarberServices _insertPhotoOfBarberServices;

        public DashboardController(
            IBarberShopRepository barberShopRepository,
            IUpdatePassword updatePassword,
            IEditAccountDetails editAccountDetails,
            IInsertPhotoOfBarberServices insertPhotoOfBarberServices)
        {
            _barberShopRepository = barberShopRepository;
            _updatePassword = updatePassword;
            _editAccountDetails = editAccountDetails;
            _insertPhotoOfBarberServices = insertPhotoOfBarberServices;
        }

        public async Task<IActionResult> MyDashboard()
        {
            Scheduling[] mySchedluings;
            var user = await _barberShopRepository.GetUserLoggedInByCpf(User.Identity.Name);
            if (User.Claims.First().Value == nameof(EnumAccountCategory.ADMINISTRATOR))
            {
                return RedirectToAction("Index", "Administrator");
            }

            if (User.Claims.First().Value == nameof(EnumAccountCategory.BARBER))
            {
                mySchedluings = await _barberShopRepository.GetBarberSchedules(user.Id);
            }
            else
            {
                mySchedluings = await _barberShopRepository.GetUserSchedules(user.Id);
            }
            var viewModel = new DashboardViewModel
            {
                Schedulings = mySchedluings,
                User = user
            };
            return View(viewModel);
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
                return RedirectToAction(nameof(MyDashboard));
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
            if (User.Claims.First().Value == nameof(EnumAccountCategory.ADMINISTRATOR))
            {
                viewModel.AccountCategories = await _barberShopRepository.GetAccountCategories();
            }

            if (user.Cpf != User.Identity.Name)
            {
                if (!(User.Claims.First().Value == nameof(EnumAccountCategory.ADMINISTRATOR)))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
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
                await _editAccountDetails.Execute(obj, User.Identity.Name, User.Claims.First().Value.ToString());
                if (User.Claims.First().Value == nameof(EnumAccountCategory.ADMINISTRATOR))
                {
                    return RedirectToAction("Index", "Administrator");
                }
                return RedirectToAction(nameof(MyDashboard));
            }
            catch (Exception ex)
            {
                TempData["ErrorEditAccount"] = ex.Message;
                var genders = await _barberShopRepository.GetGenders();
                var user = await _barberShopRepository.GetUserById(obj.User.Id);
                var viewModel = new UserFormViewModel { User = user, Genders = genders };
                if (User.Claims.First().Value == nameof(EnumAccountCategory.ADMINISTRATOR))
                {
                    viewModel.AccountCategories = await _barberShopRepository.GetAccountCategories();
                }
                return View(viewModel);
            }
        }

        public async Task<IActionResult> DashboardBarber(int id)
        {
            var barber = await _barberShopRepository.GetUserById(id);
            var commentsBarber = await _barberShopRepository.GetBarberCommentById(barber.Id);
            var photosOfBarberServices = await _barberShopRepository.GetPhotosOfTheBarberServicesById(barber.Id);
            var viewModel = new DashboardBarberViewModel
            {
                Barber = barber,
                Assessments = commentsBarber,
                PhotoOfBarberServices = photosOfBarberServices
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(EnumAccountCategory.CLIENT))]
        public async Task<IActionResult> EvaluatesBarber(string comments, int idBarber, string cpf)
        {
            try
            {
                var user = await _barberShopRepository.GetUserByCpf(cpf);
                var assesments = new Assessments
                {
                    BarberId = idBarber,
                    ClientId = user.Id,
                    Description = comments,
                };
                await _barberShopRepository.Insert(assesments);
            }
            catch (Exception ex)
            {
                TempData["ErroUpdateScheduling"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> AddMyPhotoToMyGallery()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(EnumAccountCategory.BARBER))]
        public async Task<IActionResult> AddMyPhotoToMyGallery(
            string cpf,
            IFormFile Image)
        {
            try
            {
                await _insertPhotoOfBarberServices.Execute(cpf, Image);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View();
            }
            return RedirectToAction(nameof(MyDashboard));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(EnumAccountCategory.BARBER))]
        public async Task<IActionResult> DeletePhotoToMyGallery(int id)
        {
            var photoGallery = await _barberShopRepository.GetPhotoOfTheBarberGalleryById(id);
            await _barberShopRepository.Delete(photoGallery);
            return RedirectToAction(nameof(MyDashboard));
        }
    }
}
