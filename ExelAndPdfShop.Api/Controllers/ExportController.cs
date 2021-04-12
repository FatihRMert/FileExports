using System.IO;
using System.Text;
using ClosedXML.Excel;
using DinkToPdf;
using DinkToPdf.Contracts;
using ExelAndPdfShop.Api.Utility;
using Microsoft.AspNetCore.Mvc;

namespace ExelAndPdfShop.Api.Controllers
{
    [ApiController]
    [Route("export")]
    public class ExportController : ControllerBase
    {

        private readonly IConverter _converter;

        public ExportController(IConverter converter)
        {
            _converter = converter;
        }

        [HttpGet("pdf")]
        public IActionResult Pdf()
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings {Top = 10},
                DocumentTitle = "Pdf Export",
               // Out = @"C:\LogFolder\Report.pdf" burası calıstıgı zaman dosyanın indirileceği yeri belirtiyoruz
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = TemplateGenerator.GetHtmlString(),
                WebSettings =
                {
                    DefaultEncoding = "utf-8",
                    UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "style.css")
                },
                HeaderSettings = {FontName = "arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true},
                FooterSettings = {FontName = "arial", FontSize = 9, Line = true, Center = "Report Footer"}
            };
            
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = {objectSettings}
            };

            var file = _converter.Convert(pdf);
            return File(file, "application/pdf", "Info.pdf"); // pdf indirilir
            // return File(file, "application/pdf"); pdf indirilmesin sadece tarayıcı icerisinde göreyim dersen burası 
            //return Ok("successfully created pdf, exported LogFolder"); Out özelliğini set edersek burasını acalım
        }


        [HttpGet("csv")]
        public IActionResult Csv()
        {
            var builder = new StringBuilder();

            builder.AppendLine("EmployeeId,EmployeeName,EmployeeSurname,EmployeeJoinDate");
            foreach (var employee in DataStorage.GetEmployees())
            {
                builder.AppendLine(
                    $"{employee.Id},{employee.Name},{employee.Surname},{employee.JoinDate:d}");
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "EmployeeInfo.csv");
        }

        [HttpGet("excel")]
        public IActionResult Excel()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Employees");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Employee ID";
                worksheet.Cell(currentRow, 2).Value = "Employee Name";
                worksheet.Cell(currentRow, 3).Value = "Employee Surname";
                worksheet.Cell(currentRow, 4).Value = "Employee Join Date";

                foreach (var employee in DataStorage.GetEmployees())
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = employee.Id;
                    worksheet.Cell(currentRow, 2).Value = employee.Name;
                    worksheet.Cell(currentRow, 3).Value = employee.Surname;
                    worksheet.Cell(currentRow, 4).Value = employee.JoinDate;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "EmployeeInfo.xlsx");
                }
            }
            
        }
    }
}