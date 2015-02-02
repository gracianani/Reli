using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReliDemo.Models;
using ReliDemo.Infrastructure.Services;
using System.IO;
using OfficeOpenXml;
using System.Configuration;
using System.Data;

namespace DailyReportConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime day = DateTime.Today;
            var report = ReportFactory.CreateReport(ReportType.生产日报, day);
            var templateName = string.Format("{0}{1}.xlsx", ConfigurationManager.AppSettings["ReportTemplateDirectory"], report.TemplateName);
            FileInfo template = new FileInfo(templateName);
            var newFileName = string.Format("{0}{1:yyyy-MM-dd}.xlsx", report.TemplateName, day);
            FileInfo newFile = new FileInfo("c:/inetpub/vhosts/ReliWebServices/DailyReports/"+ newFileName);
            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["生产运行情况"];
                report.FillReport(worksheet);
                xlPackage.Workbook.Properties.Title = report.TemplateName;

                xlPackage.Save();
            }
        }
        /*
        static void Main(string[] args)
        {
            var directory = @"C:\Users\yaqiZhao\Desktop\qiqi\moban\";

            var files = new DirectoryInfo(directory)
                    .GetFiles("*", SearchOption.TopDirectoryOnly)
                    .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden)).ToArray();

            var newFileName = directory + "merged.xlsx";
            ExcelPackage merged = new ExcelPackage(new FileInfo(newFileName));
            var ws = merged.Workbook.Worksheets.Add("Content");
            ws.View.ShowGridLines = false;
            int column = 1;
            foreach (var file in files)
            {
                FileInfo newFile = new FileInfo(file.FullName);
                ExcelPackage ep = new ExcelPackage(newFile);
                var dt = WorksheetToDataTable(ep.Workbook.Worksheets[1]);
                ws.Cells[1, column].LoadFromDataTable(dt, true);
                column += dt.Columns.Count;
                //Add the Content sheet
            }
            merged.Save();
        }
        */
        public static DataTable WorksheetToDataTable(ExcelWorksheet oSheet)
        {
            int totalRows = oSheet.Dimension.End.Row;
            int totalCols = oSheet.Dimension.End.Column;
            DataTable dt = new DataTable(oSheet.Name);
            DataRow dr = null;
            for (int i = 1; i <= totalRows; i++)
            {
                if (i > 1) dr = dt.Rows.Add();
                for (int j = 1; j <= totalCols; j++)
                {
                    if (i == 1)
                        dt.Columns.Add(oSheet.Cells[i, j].Value.ToString());
                    else
                        dr[j - 1] = oSheet.Cells[i, j].Value.ToString();
                }
            }
            return dt;
        }
    }
}
