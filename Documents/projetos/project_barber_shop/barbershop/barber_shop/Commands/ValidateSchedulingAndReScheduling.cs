using barber_shop.Extensions;
using barber_shop.Models;
using barber_shop.Models.ViewModel;
using barber_shop.Services;

namespace barber_shop.Commands
{
    public interface IValidateSchedulingAndReScheduling
    {
        Task ValidateScheduling(SchedulingFormViewModel obj, string cpfLoggedIn);
        Task ValidateReScheduling(SchedulingFormViewModel obj, string cpfLoggedIn);
    }

    public class ValidateSchedulingAndReScheduling : IValidateSchedulingAndReScheduling
    {
        private readonly IBarberShopRepository _barberShopRepository;

        public ValidateSchedulingAndReScheduling(
            IBarberShopRepository barberShopRepository)
        {
            _barberShopRepository = barberShopRepository;
        }

        public async Task ValidateScheduling(SchedulingFormViewModel obj, string cpfLoggedIn)
        {
            var barberHasAnAppointment = await this.ValidatesOtherSchedulingInformation(obj, cpfLoggedIn);
            if (barberHasAnAppointment is not null)
            {
                throw new Exception("O barbeiro selecionado ja possui agendamento nesse dia e horario");
            }
        }

        public async Task ValidateReScheduling(SchedulingFormViewModel obj, string cpfLoggedIn)
        {
            var barberHasAnAppointment = await this.ValidatesOtherSchedulingInformation(obj, cpfLoggedIn);
            if (barberHasAnAppointment is not null)
            {
                // se o id do agendamento que está vindo do formulário não for igual ao id o agendamento que o barbeiro tem
                //significa que o responsável pelo agendamento não é o mesmo cliete que está tentando reagendar
                if (!(obj.Scheduling.Id == barberHasAnAppointment.Id))
                {
                    throw new Exception("O barbeiro selecionado ja possui agendamento nesse dia e horario");
                }
            }
        }

        private async Task<Scheduling> ValidatesOtherSchedulingInformation(SchedulingFormViewModel obj, string cpfLoggedIn)
        {
            User user;
            var schedulingTime = await _barberShopRepository.GetSchedulingTimeById(obj.Scheduling.SchedulingTimesId);
            //data atual, data do agendamento
            var currentDate = DateOnly.FromDateTime(DateTime.Now);
            var dateSheduling = DateOnly.FromDateTime(obj.Scheduling.Date);

            //hora e minuto atual
            var currentHourScheduling = DateTime.Today.AddHours(
                Convert.ToInt32(
                    schedulingTime.Description.Substring(0, 2))
                ).AddMinutes(
                Convert.ToInt32(
                    schedulingTime.Description.Substring(3, 2)));

            //caso a data seja menor que a data atual
            if (dateSheduling < currentDate)
            {
                throw new Exception("A data nao pode ser menor que a data atual.");
            }
            //ou então caso a data do agendamento seja igual a data atual E o horário do agendamento seja menor que o horário atual
            else if (dateSheduling == currentDate && currentHourScheduling < DateTime.Now)
            {
                throw new Exception("Horario do agendamento menor que o horario atual na data atual");
            }

            //se o cpf esitver nulo significa que o usuário que está realizando agendamento é um client
            if (obj.CpfResponsible == null)
            {
                //pega o usuário pelo cpf logado (CLIENT)
                user = await _barberShopRepository.GetUserLoggedInByCpf(cpfLoggedIn);
                obj.Scheduling.ClientId = user.Id;
            }
            //caso não esteja nulo significa que o usuário que está fazendo um agendamento é o adm ou o barbeiro para o usuário (eles irão informar o cpf do cliente que quer agendar)
            else
            {
                obj.CpfResponsible = StringExtension.RemoveFormatCpf(obj.CpfResponsible);
                //valida o cpf
                if (!Person.ValidateCpf(obj.CpfResponsible))
                {
                    throw new Exception("CPF Invalido.");
                }
                user = await _barberShopRepository.GetUserLoggedInByCpf(obj.CpfResponsible);
                //se o usuário for nulo significa que ele não tem cadastro
                if (user == null)
                {
                    throw new Exception("CPF nao encontrado.");
                }
                obj.Scheduling.ClientId = user.Id;
            }

            //fazendo busca no banco se o barbeiro tem agendamento e retornando para os métodos valida agendamento e reagendamento pois ambos tem regras diferentes
            return await _barberShopRepository.GetBarberSchedulings(obj.Scheduling);
        }
    }
}
