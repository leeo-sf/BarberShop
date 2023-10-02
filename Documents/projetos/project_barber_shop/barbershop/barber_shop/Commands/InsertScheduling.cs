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

        public InsertScheduling(
            IBarberShopRepository barberShopRepository
            )
        {
            _barberShopRepository = barberShopRepository;
        }

        public async Task Execute(SchedulingFormViewModel obj, string cpfLoggedIn)
        {
            var documentFormated = obj.CpfResponsible.FormatCpf();
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
                throw new Exception("Não é possível fazer um agendamento menor que a data atual.");
            }
            //ou então caso a data do agendamento seja igual a data atual E o horário do agendamento seja menor que o horário atual
            else if (dateSheduling == currentDate && currentHourScheduling < DateTime.Now)
            {
                throw new Exception("Não é possível fazer um agendamento na mesma data atual com o horário menor que o atual");
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
                //valida o cpf
                if (!Person.ValidateCpf(documentFormated))
                {
                    throw new Exception("CPF Inválido.");
                }
                user = await _barberShopRepository.GetUserLoggedInByCpf(documentFormated);
                //se o usuário for nulo significa que ele não tem cadastro
                if (user == null)
                {
                    throw new Exception("CPF não encontrado.");
                }
                obj.Scheduling.ClientId = user.Id;
            }

            //fazendo busca no banco se o barbeiro tem agendamento naquele dia e horário (se vier nulo NÃO TEM caso contrário TEM AGENDAMENTO)
            var barberSchedule = await _barberShopRepository.GetBarberSchedulings(obj.Scheduling);
            if (barberSchedule is not null)
            {
                throw new Exception("O barbeiro selecionado já possuí agendamento nesse dia e horário");
            }

            await _barberShopRepository.Insert(obj.Scheduling);
        }
    }
}
