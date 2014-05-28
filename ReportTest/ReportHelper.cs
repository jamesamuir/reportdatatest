using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Export;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportDataTest
{
    public class ReportHelper
    {
        public void GenerateReport()
        {
            var appPath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug\\", "");
            var dbPath = appPath + "\\app_data\\Test.sdf";
            var dbConnString = "Data Source = " + dbPath;
            var reportPath = appPath + "\\template\\Dynamic.mrt";
            var outputPath = appPath + "\\output\\dynamic.pdf";

            SqlCeConnection conn = null;

            try
            {

                #region "Database Create & Populate"
                //if (File.Exists(dbPath))
                //    File.Delete(dbPath);

                //SqlCeEngine engine = new SqlCeEngine(dbConnString);
                //engine.CreateDatabase();

                //conn = new SqlCeConnection(dbConnString);
                //conn.Open();

                //SqlCeCommand cmd = conn.CreateCommand();

                //cmd.CommandText =
                //    "CREATE TABLE Users1 (ID INT PRIMARY KEY, NAME NTEXT, AGE INTEGER)";

                //cmd.ExecuteNonQuery();

                //cmd.CommandText =
                //    "INSERT INTO Users1 (ID, NAME, AGE) VALUES (0, 'kyle', 14)";
                //cmd.ExecuteNonQuery();
                //cmd.CommandText =
                //    "INSERT INTO Users1 (ID, NAME, AGE) VALUES (1, 'stan', 14)";
                //cmd.ExecuteNonQuery();
                //cmd.CommandText =
                //    "INSERT INTO Users1 (ID, NAME, AGE) VALUES (2, 'cartman', 15)";
                //cmd.ExecuteNonQuery();
                //cmd.CommandText =
                //    "INSERT INTO Users1 (ID, NAME, AGE) VALUES (3, 'kenny', 16)";
                //cmd.ExecuteNonQuery();
                #endregion


                #region "Select Data"
                conn = new SqlCeConnection(dbConnString);
                conn.Open();

                SqlCeCommand cmd = conn.CreateCommand();

                cmd.CommandText = "SELECT * FROM [Users1]";

                SqlCeDataReader rdr = cmd.ExecuteReader();

                

                while (rdr.Read())
                {
                    Console.WriteLine(rdr.GetInt32(0) + " | " + rdr.GetString(1) + " | " + rdr.GetInt32(2));
                }
                #endregion



            }
            catch (SqlCeException e)
            {
                Console.WriteLine("Exception " + e.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }


            #region "Report Test"
            var report = new StiReport();
            report.Load(reportPath);

            StiConfig.Services.Add(new StiSqlCeAdapterService());
            StiConfig.Services.Add(new StiSqlCeDatabase());
         
            StiSqlCeSource DSDynamic = new StiSqlCeSource("SQLCE", "Users", "Users", "SELECT * FROM [Users]", true, false);
            report.Dictionary.DataSources["Users"] = DSDynamic;

            foreach (StiDataColumn col in DSDynamic)
            {
                DSDynamic.Columns.Add(col.Name, col.Type);
            }

            report.Dictionary.Synchronize();
            report.Compile();
            report.Render();
            
            StiPdfExportService pdfExport = new StiPdfExportService();
            pdfExport.ExportPdf(report, outputPath);
            #endregion


            //StiPdfExportService pdfExport = new StiPdfExportService();
            //Collection<Stimulsoft.Report.Components.StiPage> reportPages = new Collection<Stimulsoft.Report.Components.StiPage>();

            //var appPath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug\\", "");

            ////Load Dynamic Report
            //var report = new StiReport();
            //var reportPath = string.Format("{0}{1}",appPath, "\\template\\Dynamic.mrt");
            //report.Load(reportPath);
            //var image = report.Pages[0].Components[report.Pages[0].Components.IndexOf("Image")] as Stimulsoft.Report.Components.StiImage;
            //image.ImageURL = new Stimulsoft.Report.Components.StiImageURLExpression("http://tweeterism.com/wp-content/uploads/2013/04/LOL-Cat.jpeg");
            //report.Render();

            //pdfExport.ExportPdf(report, string.Format("{0}{1}", appPath, "\\output\\dynamic.pdf"));


            ////Create temp Report for page rendering
            //StiReport tempReport = new StiReport();
            //tempReport.Pages.Clear();
            //tempReport.Render();
            //tempReport.RenderedPages.Add(report.RenderedPages[0]);
            //tempReport.Render();

            
            //pdfExport.ExportPdf(tempReport, string.Format("{0}{1}", appPath, "\\output\\tempReport1.pdf"));
            
            
            //Get the new pages
            //Stimulsoft.Report.Components.StiPage newPage = report.Pages[0].Clone() as Stimulsoft.Report.Components.StiPage;
            //newPage.Name = "newPage";
            //var image = newPage.Components[newPage.Components.IndexOf("Image")] as Stimulsoft.Report.Components.StiImage;
            //image.ImageURL = new Stimulsoft.Report.Components.StiImageURLExpression("http://tweeterism.com/wp-content/uploads/2013/04/LOL-Cat.jpeg");
            //tempReport.SaveState("updated");
            //tempReport.Pages.Add(newPage);
            //tempReport.Compile();

            
            


            //report.Pages.Add(tempReport.RenderedPages[1]);

            
            //pdfExport.ExportPdf(report, string.Format("{0}{1}", appPath, "\\output\\dynamic.pdf"));



        }
    }
}
