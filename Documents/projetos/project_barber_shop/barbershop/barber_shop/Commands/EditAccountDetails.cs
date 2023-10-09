using barber_shop.Extensions;
using barber_shop.Models;
using barber_shop.Models.Enums;
using barber_shop.Models.ViewModel;
using barber_shop.Services;
using Microsoft.EntityFrameworkCore;

namespace barber_shop.Commands
{
    public interface IEditAccountDetails
    {
        Task Execute(UserFormViewModel obj, string cpfLoggedIn);
    }

    public class EditAccountDetails : IEditAccountDetails
    {
        private readonly IBarberShopRepository _barberShopRepository;

        public EditAccountDetails(
            IBarberShopRepository barberShopRepository)
        {
            _barberShopRepository = barberShopRepository;
        }

        public async Task Execute(UserFormViewModel obj, string cpfLoggedIn)
        {
            obj.User.Cpf = obj.User.Cpf.FormatCpf();

            var profile = await _barberShopRepository.GetUserByEmail(obj.User.Profile.Email);
            if (profile is not null)
            {
                if (!(obj.User.Profile.Id == profile.Id))
                {
                    throw new Exception("Email ja cadastrado.");
                }
            }
            if (!Person.ValidateCpf(obj.User.Cpf))
            {
                throw new Exception("CPF invalido.");
            }

            var userByCpf = await _barberShopRepository.GetUserByCpf(obj.User.Cpf);
            if (userByCpf is not null)
            {
                if (!(obj.User.Id == userByCpf.Id))
                {
                    throw new Exception("CPF ja vinculado a uma conta.");
                }
            }

            var userByTelefone = await _barberShopRepository.GetUserByTelephone(obj.User.Telephone);
            if (userByTelefone is not null)
            {
                if (!(obj.User.Id == userByTelefone.Id))
                {
                    throw new Exception("Telefone ja vinculado a uma conta");
                }
            }

            if (obj.User.Profile.CategoryId == 0)
            {
                obj.User.Profile.CategoryId = (int)EnumAccountCategory.CLIENT;
            }

            try
            {
                obj.User.Profile.Password = userByCpf.Profile.Password;
                await _barberShopRepository.Update(obj.User);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.Message);

            }
        }
    }
}
