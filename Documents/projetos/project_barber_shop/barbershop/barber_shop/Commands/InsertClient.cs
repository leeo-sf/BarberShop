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
            var documentFormated = obj.User.Cpf.FormatCpf();
            var client = obj.User;

            var profile = await _barberShopRepository.GetProfileEmail(client.Profile.Email);
            if (profile is not null)
            {
                throw new Exception("Email já cadastrado.");
            }

            if (!Person.ValidateCpf(documentFormated))
            {
                throw new Exception("CPF inválido.");
            }

            if (obj.User.Profile.CategoryId == 0)
            {
                obj.User.Profile.CategoryId = (int)EnumAccountCategory.CLIENT;
            }
            
            try
            {
                await _barberShopRepository.Insert(client);
            }
            catch (DbUpdateException ex)
            {
                if (ex.ToString().Contains("Duplicate"))
                {
                    throw new Exception("Alguma informação já está vinculada a uma conta.");
                }
            }
        }
    }
}
