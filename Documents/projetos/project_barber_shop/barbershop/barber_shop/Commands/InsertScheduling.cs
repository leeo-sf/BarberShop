using barber_shop.Extensions;
using barber_shop.Models;
using barber_shop.Models.ViewModel;
using barber_shop.Services;
using FluentValidation.Validators;
using Microsoft.VisualBasic;
using NPOI.HSSF.Record;
using NPOI.OpenXmlFormats.Dml;

namespace barber_shop.Commands
{
    public interface IInsertScheduling
    {
        Task Execute(SchedulingFormViewModel obj, string loggedInUserCpf);
    }

    public class InsertScheduling : IInsertScheduling
    {
        private IBarberShopRepository _barberShopRepository;
        private IValidateSchedulingAndReScheduling _validateSchedulingAndReScheduling;

        public InsertScheduling(
            IBarberShopRepository barberShopRepository,
            IValidateSchedulingAndReScheduling validateSchedulingAndReScheduling
            )
        {
            _barberShopRepository = barberShopRepository;
            _validateSchedulingAndReScheduling = validateSchedulingAndReScheduling;
        }

        public async Task Execute(SchedulingFormViewModel obj, string cpfLoggedIn)
        {
            try
            {
                await _validateSchedulingAndReScheduling.ValidateScheduling(obj, cpfLoggedIn);
                await _barberShopRepository.Insert(obj.Scheduling);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
