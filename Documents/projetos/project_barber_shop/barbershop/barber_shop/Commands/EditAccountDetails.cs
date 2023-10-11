using barber_shop.Extensions;
using barber_shop.Models;
using barber_shop.Models.Enums;
using barber_shop.Models.ViewModel;
using barber_shop.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;

namespace barber_shop.Commands
{
    public interface IEditAccountDetails
    {
        Task Execute(UserFormViewModel obj, string cpfLoggedIn, string profileLoggedIn);
    }

    public class EditAccountDetails : IEditAccountDetails
    {
        private readonly IBarberShopRepository _barberShopRepository;

        public EditAccountDetails(
            IBarberShopRepository barberShopRepository)
        {
            _barberShopRepository = barberShopRepository;
        }

        public async Task Execute(UserFormViewModel obj, string cpfLoggedIn, string profileLoggedIn)
        {
            obj.User.Cpf = obj.User.Cpf.RemoveFormatCpf();
            var userLoggedIn = await _barberShopRepository.GetUserByCpf(cpfLoggedIn);
            var user = await _barberShopRepository.GetUserById(obj.User.Id);

            //se o id do usuário logado (que está tentando alterar os dados) for diferente do id do usuário que será alterado
            //e se o usuário logado não for um ADM DEVE dar erro
            if (obj.User.Id != userLoggedIn.Id && !(profileLoggedIn == nameof(EnumAccountCategory.ADMINISTRATOR)))
            {
                throw new Exception("Voce nao tem permissao.");
            }

            //se existir o email no banco de dados
            //e o proprietário não for o mesmo que estiver tentando alterar DEVE dar erro
            var profile = await _barberShopRepository.GetUserByEmail(obj.User.Profile.Email);
            if (profile is not null && !(obj.User.Profile.Id == profile.Id))
            {
                throw new Exception("O email ja foi vinculado a uma conta.");
            }

            //se existir o telefone no banco de dados
            //e o proprietário não for o mesmo que estiver tentando alterar DEVE dar erro
            var userByTelefone = await _barberShopRepository.GetUserByTelephone(obj.User.Telephone);
            if (userByTelefone is not null && !(obj.User.Id == userByTelefone.Id))
            {
                throw new Exception("Telefone ja vinculado a uma conta");
            }

            try
            {
                //atribuindo a senha do usuário que está no banco pois, o serviço de alterar senha é outro
                //nesse formulário só é possível alterar os dados cadastrais
                obj.User.Profile.Password = user.Profile.Password;
                await _barberShopRepository.Update(obj.User);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.Message);

            }
        }
    }
}
