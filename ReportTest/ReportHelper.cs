using Stimulsoft.Report;
using Stimulsoft.Report.Export;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportTest
{
    public class ReportHelper
    {
        public void GenerateReport()
        {

            Collection<Stimulsoft.Report.Components.StiPage> reportPages = new Collection<Stimulsoft.Report.Components.StiPage>();

            var appPath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug\\", "");

            //Load Dynamic Report
            var report = new StiReport();
            var reportPath = string.Format("{0}{1}",appPath, "\\template\\Dynamic.mrt");
            report.Load(reportPath);
            report.Render();

            //Create temp Report for page rendering
            StiReport tempReport = new StiReport();
            tempReport.Pages.Clear();
            tempReport.Render();


            


            //Get the new pages
            Stimulsoft.Report.Components.StiPage newPage = report.Pages[0].Clone() as Stimulsoft.Report.Components.StiPage;
            Stimulsoft.Report.Components.StiPage newPage3 = report.Pages[0].Clone() as Stimulsoft.Report.Components.StiPage;
            reportPages.Add(newPage);
            report.Pages.Clear();


   

            newPage.Name = "Page2";
            var image = newPage.Components[newPage.Components.IndexOf("Image")] as Stimulsoft.Report.Components.StiImage;
            image.ImageURL = new Stimulsoft.Report.Components.StiImageURLExpression("http://tweeterism.com/wp-content/uploads/2013/04/LOL-Cat.jpeg");
            tempReport.Pages.Add(newPage);
            tempReport.Render();
            report.Pages.Add(tempReport.Pages[0]);
            tempReport.Pages.Clear();

            newPage3.Name = "Page3";
            var image3 = newPage.Components[newPage3.Components.IndexOf("Image")] as Stimulsoft.Report.Components.StiImage;
            image3.ImageURL = new Stimulsoft.Report.Components.StiImageURLExpression("https://www.google.com/search?q=Rachel+Louise+Carson&oi=ddle&ct=rachel-louise-carsons-107th-birthday-6210023252295680-hp&hl=en");
            tempReport.Pages.Add(newPage3);
            tempReport.Render();
            report.Pages.Add(tempReport.Pages[0]);
            tempReport.Pages.Clear();


         

            report.Render();

            report.Pages.Add(reportPages[0]);


            report.Render();

       
            StiPdfExportService pdfExport = new StiPdfExportService();
            pdfExport.ExportPdf(report, string.Format("{0}{1}", appPath, "\\output\\dyanmic.pdf"));

        }
    }
}
