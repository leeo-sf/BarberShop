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

        public async Task<IActionResult> Index(User? user)
        {
            return View(user);
        }

        public async Task<IActionResult> Register()
        {
            //padronizando registro de um novo usuário
            //quando o adm for registrar um usuário será redirecionado para a mesma página de cadastro que client
            //mas com permissões diferentes (podendo escolher o perfil do usuário a ser cadastrado)
            return RedirectToAction("Register", "Access");
        }

        public async Task<IActionResult> ManageAccount()
        {
            var users = await _barberShopRepository.GetAllUsers();
            return View(users);
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
    }
}
