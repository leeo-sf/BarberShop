using barber_shop.Extensions;
using barber_shop.Models.ViewModel;
using barber_shop.Models;
using barber_shop.Services;
using System.Linq.Expressions;

namespace barber_shop.Commands
{
    public interface IUpdateScheduling
    {
        Task Execute(SchedulingFormViewModel schedulingFormViewModel, string cpfLoggedIn);
    }

    public class UpdateScheduling : IUpdateScheduling
    {
        private readonly IBarberShopRepository _barberShopRepository;
        private readonly IValidateSchedulingAndReScheduling _validateSchedulingAndReScheduling;

        public UpdateScheduling(
            IBarberShopRepository barberShopRepository,
            IValidateSchedulingAndReScheduling validateSchedulingAndReScheduling)
        {
            _barberShopRepository = barberShopRepository;
            _validateSchedulingAndReScheduling = validateSchedulingAndReScheduling;
        }

        public async Task Execute(SchedulingFormViewModel obj, string cpfLoggedIn)
        {
            try
            {
                await _validateSchedulingAndReScheduling.ValidateReScheduling(obj, cpfLoggedIn);
                await _barberShopRepository.Update(obj.Scheduling);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
