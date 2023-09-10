using barber_shop.Models;
using barber_shop.Models.Enums;
using barber_shop.Models.ViewModel;
using barber_shop.Services;

namespace barber_shop.Commands
{
    public interface IInsertClient
    {
        Task Execute(UserFormViewModelClient obj);
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

        public async Task Execute(UserFormViewModelClient obj)
        {
            var client = obj.Client;

            var profile = await _barberShopRepository.GetProfileEmail(client.Profile.Email);
            if (profile is not null)
            {
                throw new Exception("Email já cadastrado.");
            }

            if (!obj.Client.ValidateCpf())
            {
                throw new Exception("CPF inválido.");
            }

            if (obj.Client.Profile.CategoryId == 0)
            {
                obj.Client.Profile.CategoryId = (int)EnumAccountCategory.CLIENT;
            }
            await _barberShopRepository.Insert(client);
        }
    }
}
