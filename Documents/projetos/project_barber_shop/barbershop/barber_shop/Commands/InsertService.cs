using barber_shop.Models;
using barber_shop.Services;

namespace barber_shop.Commands
{
    public interface IInsertService
    {
        Task Execute(Service obj);
    }

    public class InsertService : IInsertService
    {
        private IBarberShopRepository _barberShopRepository;

        public InsertService(
            IBarberShopRepository barberShopRepository
            )
        {
            _barberShopRepository = barberShopRepository;
        }

        public async Task Execute(Service obj)
        {
            await _barberShopRepository.Insert(obj);
        }
    }
}
