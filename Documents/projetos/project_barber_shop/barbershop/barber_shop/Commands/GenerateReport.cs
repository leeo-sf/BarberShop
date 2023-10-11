using barber_shop.Integration.Email;
using barber_shop.Models;
using barber_shop.Services;
using ClosedXML.Excel;
using System.Diagnostics;

namespace barber_shop.Commands
{
    public interface IGenerateReport
    {
        Task<Scheduling[]> Execute(DateTimeOffset createdFrom, DateTimeOffset createdUntil, string loggedInUserCpf);
    }

    public class GenerateReport : IGenerateReport
    {
        private readonly IBarberShopRepository _barberShopRepository;
        private readonly IEmail _email;

        public GenerateReport(
            IBarberShopRepository barberShopRepository,
            IEmail email
            )
        {
            _barberShopRepository = barberShopRepository;
            _email = email;
        }

        public async Task<Scheduling[]> Execute(DateTimeOffset createdFrom, DateTimeOffset createdUntil, string loggedInUserCpf)
        {
            var getAllSchedulingsReport = await _barberShopRepository.GetAllSchedulingsReport(createdFrom, createdUntil);
            if (getAllSchedulingsReport.Count() == 0)
            {
                throw new Exception("Nenhum dado foi encontrado");
            }
            var getUserLoggedIn = await _barberShopRepository.GetUserLoggedInByCpf(loggedInUserCpf);
            var fileName = @"C:\Relatorios BarberShop\" + Guid.NewGuid() + " Relatorio.xlsx";

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Relatorio");
                worksheet.Cell("A1").Value = "Cliente";
                worksheet.Cell("B1").Value = "Barbeiro";
                worksheet.Cell("C1").Value = "Data";
                worksheet.Cell("D1").Value = "Horário";
                worksheet.Cell("E1").Value = "Serviço";
                worksheet.Cell("F1").Value = "Valor";

                var line = 2;

                foreach(var scheduling in  getAllSchedulingsReport)
                {
                    var schedullings = getAllSchedulingsReport.Where(x =>
                    x.Client.Cpf == scheduling.Client.Cpf &&
                    x.Barber.Cpf == scheduling.Barber.Cpf).ToArray();

                    worksheet.Cell("A" + line).Value = scheduling.Client.Name.ToUpper();
                    worksheet.Cell("B" + line).Value = scheduling.Barber.Name.ToUpper();
                    worksheet.Cell("C" + line).Value = scheduling.Date;
                    worksheet.Cell("D" + line).Value = scheduling.SchedulingTimes.Description;
                    worksheet.Cell("E" + line).Value = scheduling.Service.Name;
                    worksheet.Cell("F" + line).Value = scheduling.Service.Value.ToString("C");
                    line++;
                }
                workbook.SaveAs(Path.GetFullPath(fileName));
            }
            
            try
            {
                await _email.SendEmail(
                    emailTo: getUserLoggedIn.Profile.Email,
                    subject: $"Relatório BarberShop",
                    body: $"Segue em anexo relatório entre as datas {createdFrom.ToString("dd/MM/yyyy")} até {createdUntil.ToString("dd/MM/yyyy")}",
                    attachments: fileName
                    );
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return getAllSchedulingsReport;
        }
    }
}
