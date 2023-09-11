using barber_shop.Models;
using barber_shop.Services;

namespace barber_shop.Commands
{
    public interface IDeleteService
    {
        Task Execute(int id);
    }

    public class DeleteService : IDeleteService
    {
        private IBarberShopRepository _barberShopRepository;

        public DeleteService(
            IBarberShopRepository barberShopRepository
            )
        {
            _barberShopRepository = barberShopRepository;
        }

        public async Task Execute(int id)
        {
            var service = await _barberShopRepository.GetService(id);
            await _barberShopRepository.Delete(service);
        }
    }
}
