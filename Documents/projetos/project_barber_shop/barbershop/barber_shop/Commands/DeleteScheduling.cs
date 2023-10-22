using barber_shop.Services;

namespace barber_shop.Commands
{
    public interface IDeleteScheduling
    {
        Task Exectute(int id);
    }

    public class DeleteScheduling : IDeleteScheduling
    {
        private readonly IBarberShopRepository _barberShopRepository;

        public DeleteScheduling(
            IBarberShopRepository barberShopRepository)
        {
            _barberShopRepository = barberShopRepository;
        }

        public async Task Exectute(int id)
        {
            try
            {
                var scheduling = await _barberShopRepository.GetSchedulingById(id);
                if (scheduling.Date <= DateTimeOffset.Now)
                {
                    throw new Exception("Voce nao pode deletar esse agendamento, o prazo de cancelamento foi ultrapassado.");
                }
                await _barberShopRepository.Delete(scheduling);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
