using System.Text;

namespace ExelAndPdfShop.Api.Utility
{
    public static class TemplateGenerator
    {
        public static string GetHtmlString()
        {
            var builder = new StringBuilder();

            builder.Append(@"
<html>
    <head></head>
        <body>
            <div class='header'>
                <h1>Generated PDF export!</h1>
                <table align='center'>
                    <tr>
                        <th>Id</th>
                        <th>Name</th>
                        <th>Surname</th>
                        <th>JoinDate</th>
                    </tr>
                ");
            foreach (var employee in DataStorage.GetEmployees())
            {
                builder.AppendFormat(@"
                    <tr>
                        <td>{0}</td>
                        <td>{1}</td>
                        <td>{2}</td>
                        <td>{3:d}</td>
                    </tr>", 
                    employee.Id, employee.Name, employee.Surname, employee.JoinDate);
            }
            
            builder.Append(@"
                </table>    
            </body>
        </html>");
            
            return builder.ToString();
        }
    }
}