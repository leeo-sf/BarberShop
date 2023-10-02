using barber_shop.Models;
using barber_shop.Services;
using ClosedXML.Excel;
using System.Diagnostics;

namespace barber_shop.Commands
{
    public interface IGenerateReport
    {
        Task<Scheduling[]> Execute(DateTimeOffset createdFrom, DateTimeOffset createdUntil);
    }

    public class GenerateReport : IGenerateReport
    {
        private readonly IBarberShopRepository _barberShopRepository;

        public GenerateReport(
            IBarberShopRepository barberShopRepository)
        {
            _barberShopRepository = barberShopRepository;
        }

        public async Task<Scheduling[]> Execute(DateTimeOffset createdFrom, DateTimeOffset createdUntil)
        {
            var getAllSchedulingsReport = await _barberShopRepository.GetAllSchedulingsReport(createdFrom, createdUntil);
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
            var getAllSchedulings = await _barberShopRepository.GetAllSchedulings();
            return getAllSchedulings;
        }
    }
}
