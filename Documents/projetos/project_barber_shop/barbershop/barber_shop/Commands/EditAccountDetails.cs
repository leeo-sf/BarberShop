using barber_shop.Extensions;
using barber_shop.Models;
using barber_shop.Models.Enums;
using barber_shop.Models.ViewModel;
using barber_shop.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;

namespace barber_shop.Commands
{
    public interface IEditAccountDetails
    {
        Task Execute(UserFormViewModel obj, string cpfLoggedIn, string profileLoggedIn);
    }

    public class EditAccountDetails : IEditAccountDetails
    {
        private readonly IBarberShopRepository _barberShopRepository;

        public EditAccountDetails(
            IBarberShopRepository barberShopRepository)
        {
            _barberShopRepository = barberShopRepository;
        }

        public async Task Execute(UserFormViewModel obj, string cpfLoggedIn, string profileLoggedIn)
        {
            obj.User.Cpf = obj.User.Cpf.RemoveFormatCpf();

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
                var user = await _barberShopRepository.GetUserById(obj.User.Id);
                obj.User.Profile.Password = user.Profile.Password;
                if (user.Cpf != cpfLoggedIn)
                {
                    if (!(profileLoggedIn == nameof(EnumAccountCategory.ADMINISTRATOR)))
                    {
                        throw new Exception("Voce nao tem permissao");
                    }
                }
                await _barberShopRepository.Update(obj.User);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.Message);

            }
        }
    }
}
