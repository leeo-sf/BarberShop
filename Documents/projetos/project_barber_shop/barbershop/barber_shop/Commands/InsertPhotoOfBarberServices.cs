using barber_shop.Models;
using barber_shop.Services;

namespace barber_shop.Commands
{
    public interface IInsertPhotoOfBarberServices
    {
        Task Execute(string cpf, IFormFile Image);
    }

    public class InsertPhotoOfBarberServices : IInsertPhotoOfBarberServices
    {
        private readonly IBarberShopRepository _barberShopRepository;

        public InsertPhotoOfBarberServices(
            IBarberShopRepository barberShopRepository)
        {
            _barberShopRepository = barberShopRepository;
        }

        public async Task Execute(string cpf, IFormFile Image)
        {
            var user = await _barberShopRepository.GetUserByCpf(cpf);
            var photoOfBarberServices = new PhotoOfBarberServices();
            try
            {
                MemoryStream target = new MemoryStream();
                Image.CopyToAsync(target);
                byte[] img = target.ToArray();
                photoOfBarberServices.Image = img;
                photoOfBarberServices.BarberId = user.Id;

                await _barberShopRepository.Insert(photoOfBarberServices);
            }
            catch (Exception ex)
            {
                throw new Exception("Nao foi possivel salvar essa imagem. Tente com as dimensoes ate 900 x 900.");
            }
        }
    }
}
