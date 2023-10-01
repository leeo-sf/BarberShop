﻿using barber_shop.Commands;
using barber_shop.Models;
using barber_shop.Models.Enums;
using barber_shop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace barber_shop.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IInsertService _insertService;
        private readonly IBarberShopRepository _barberShopRepository;
        private readonly IUpdateService _updateService;
        private readonly IDeleteService _deleteService;

        public ServiceController(
            IInsertService insertService,
            IBarberShopRepository barberShopRepository,
            IUpdateService updateService,
            IDeleteService deleteService
            )
        {
            _insertService = insertService;
            _barberShopRepository = barberShopRepository;
            _updateService = updateService;
            _deleteService = deleteService;
        }

        [Authorize(Roles = nameof(EnumAccountCategory.ADMINISTRATOR))]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [Authorize(Roles = nameof(EnumAccountCategory.ADMINISTRATOR))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Service obj, IFormFile Image)
        {
            try
            {
                if (Image == null)
                {
                    throw new Exception("A imagem precisa ser inserida.");
                }
                MemoryStream target = new MemoryStream();
                Image.CopyToAsync(target);
                byte[] img = target.ToArray();
                obj.Image = img;
                await _insertService.Execute(obj);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorRegister"] = ex.Message;
                return View();
            }
        }

        [Authorize(Roles = nameof(EnumAccountCategory.ADMINISTRATOR))]
        public async Task<IActionResult> Edit(int id)
        {
            var service = await _barberShopRepository.GetService(id);
            return View(service);
        }

        [Authorize(Roles = nameof(EnumAccountCategory.ADMINISTRATOR))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Service obj, IFormFile Image)
        {
            try
            {
                if (Image == null)
                {
                    throw new Exception("A imagem precisa ser inserida.");
                }
                MemoryStream target = new MemoryStream();
                Image.CopyToAsync(target);
                byte[] img = target.ToArray();
                obj.Image = img;
                await _updateService.Execute(obj);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorRegister"] = ex.Message;
                return View();
            }
        }

        [Authorize(Roles = nameof(EnumAccountCategory.ADMINISTRATOR))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _deleteService.Execute(id);
                TempData["SuccessDelete"] = "Serviço Deletado";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorDelete"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
