using barber_shop.Models;
using barber_shop.Services;

namespace barber_shop.Commands
{
    public interface IUpdateService
    {
        Task Execute(Service obj);
    }

    public class UpdateService : IUpdateService
    {
        private IBarberShopRepository _barberShopRepository;

        public UpdateService(
            IBarberShopRepository barberShopRepository
            )
        {
            _barberShopRepository = barberShopRepository;
        }

        public async Task Execute(Service obj)
        {
            await _barberShopRepository.Update(obj);
        }
    }
}
