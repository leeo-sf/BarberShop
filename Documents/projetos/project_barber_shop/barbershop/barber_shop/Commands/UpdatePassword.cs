using barber_shop.Extensions;
using barber_shop.Models;
using barber_shop.Services;

namespace barber_shop.Commands
{
    public interface IUpdatePassword
    {
        Task Excute(User obj, string cpfLoggeIn);
    }

    public class UpdatePassword : IUpdatePassword
    {
        private readonly IBarberShopRepository _barberShopRepository;

        public UpdatePassword(
            IBarberShopRepository barberShopRepository)
        {
            _barberShopRepository = barberShopRepository;
        }

        public async Task Excute(User obj, string cpfLoggedIn)
        {
            obj.Cpf = obj.Cpf.RemoveFormatCpf();
            var user = await _barberShopRepository.GetUserByCpf(obj.Cpf);

            if (user is null)
            {
                throw new Exception("CPF nao encontrado");
            }

            if (obj.Cpf != cpfLoggedIn || obj.Cpf != user.Cpf)
            {
                throw new Exception("Usuario nao autentico com o usuario proprietario");
            }

            if (obj.Profile.Password == user.Profile.Password)
            {
                throw new Exception("A senha deve ser diferente da senha atual");
            }
            user.Profile.Password = obj.Profile.Password;
            await _barberShopRepository.Update(user);
        }
    }
}
