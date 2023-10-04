using barber_shop.Extensions;
using barber_shop.Models.ViewModel;
using barber_shop.Models;
using barber_shop.Services;

namespace barber_shop.Commands
{
    public interface IUpdateScheduling
    {
        Task Execute(SchedulingFormViewModel schedulingFormViewModel, string cpfLoggedIn);
    }

    public class UpdateScheduling : IUpdateScheduling
    {
        private readonly IBarberShopRepository _barberShopRepository;

        public UpdateScheduling(
            IBarberShopRepository barberShopRepository)
        {
            _barberShopRepository = barberShopRepository;
        }

        public async Task Execute(SchedulingFormViewModel obj, string cpfLoggedIn)
        {
            User user;
            var scheduling = await _barberShopRepository.GetSchedulingById(obj.Scheduling.Id);
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

            //se a data do agendamento que estiver no banco já estiver passado não pode reagendar
            if (DateOnly.FromDateTime(scheduling.Date) <= currentDate)
            {
                throw new Exception("Nao e possivel reagendar este agedamento.");
            }

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
                obj.CpfResponsible = StringExtension.FormatCpf(obj.CpfResponsible);
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

            //fazendo busca no banco se o barbeiro tem agendamento naquele dia e horário (se vier nulo NÃO TEM caso contrário TEM AGENDAMENTO)
            var barberSchedule = await _barberShopRepository.GetBarberSchedulings(obj.Scheduling);
            if (barberSchedule is not null)
            {
                throw new Exception("O barbeiro selecionado ja possui agendamento nesse dia e horario");
            }

            await _barberShopRepository.Update(obj.Scheduling);
        }
    }
}
