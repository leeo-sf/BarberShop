using barber_shop.Integration.Email;
using barber_shop.Models;
using barber_shop.Services;

namespace barber_shop.Commands
{
    public interface ISendMessagePromotional
    {
        Task Execute(PromotionalMessage obj, IFormFile Image);
    }

    public class SendMessagePromotional : ISendMessagePromotional
    {
        private readonly IEmail _email;
        private readonly IBarberShopRepository _barberShopRepository;

        public SendMessagePromotional(
            IEmail email,
            IBarberShopRepository barberShopRepository
            )
        {
            _email = email;
            _barberShopRepository = barberShopRepository;
        }

        public async Task Execute(PromotionalMessage obj, IFormFile Image)
        {
            if (Image is not null)
            {
                MemoryStream target = new MemoryStream();
                Image.CopyToAsync(target);
                byte[] img = target.ToArray();
                obj.Image = img;
            }

            try
            {
                var clients = await _barberShopRepository.GetAllClients();
                await _email.SendMessagePromotional(obj, clients);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
