using barber_shop.Extensions;
using barber_shop.Models;
using barber_shop.Models.Enums;
using barber_shop.Models.ViewModel;
using barber_shop.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace barber_shop.Commands
{
    public interface IInsertClient
    {
        Task Execute(UserFormViewModel obj);
    }

    public class InsertClient : IInsertClient
    {
        private readonly IBarberShopRepository _barberShopRepository;

        public InsertClient(
            IBarberShopRepository barberShopRepository
            )
        {
            _barberShopRepository = barberShopRepository;
        }

        public async Task Execute(UserFormViewModel obj)
        {
            obj.User.Cpf = obj.User.Cpf.RemoveFormatCpf();

            var profile = await _barberShopRepository.GetProfileEmail(obj.User.Profile.Email);
            if (profile is not null)
            {
                throw new Exception("Email ja cadastrado.");
            }
            if (!Person.ValidateCpf(obj.User.Cpf))
            {
                throw new Exception("CPF invalido.");
            }

            var userByCpf = await _barberShopRepository.GetUserByCpf(obj.User.Cpf);
            if (userByCpf is not null)
            {
                throw new Exception("CPF ja vinculado a uma conta.");
            }

            var userByTelefone = await _barberShopRepository.ThisPhoneExist(obj.User.Telephone);
            if (userByTelefone)
            {
                throw new Exception("Telefone ja vinculado a uma conta");
            }

            if (obj.User.Profile.CategoryId == 0)
            {
                obj.User.Profile.CategoryId = (int)EnumAccountCategory.CLIENT;
            }

            try
            {
                await _barberShopRepository.Insert(obj.User);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.Message);

            }
        }
    }
}
