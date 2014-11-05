using ReliDemo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliDemo.Models
{
    public class CompletionReportItem
    {
        public string 公司名;
        public decimal? 有效日总供热面积;
        public decimal? 超标日总供热面积;
        public decimal? 未超标日总供热面积;
        public decimal? 采暖季执行到位率;

        public string GetDisplay(decimal? v, string append = "", string blank = "--")
        {
            if (v.HasValue)
            {
                return v.Value + append;
            }
            return blank;
        }
    }
    public class CompletionReport : IReport
    {
        public List<CompletionReportItem> ReportData
        {
            get;
            set;
        }
        public ReportType ReportType
        {
            get { return ReportType.公司到位率统计表; }
        }

        public string TemplateName
        {
            get
            {
                return "执行到位率统计";
            }
            set
            {
            }
        }

        public void FillReport(OfficeOpenXml.ExcelWorksheet worksheet)
        {
            for (int i = 0; i < ReportData.Count(); i++)
            {
                worksheet.Cells[2 + i, 1].Value = ReportData[i].公司名;
                worksheet.Cells[2 + i, 2].Value = ReportData[i].有效日总供热面积;
                worksheet.Cells[2 + i, 2].Style.Numberformat.Format = "0.00";

                worksheet.Cells[2 + i, 3].Value = ReportData[i].超标日总供热面积;
                worksheet.Cells[2 + i, 3].Style.Numberformat.Format = "0.00";

                worksheet.Cells[2 + i, 4].Value = ReportData[i].未超标日总供热面积;
                worksheet.Cells[2 + i, 4].Style.Numberformat.Format = "0.00";

                worksheet.Cells[2 + i, 5].Value = ReportData[i].采暖季执行到位率;
                worksheet.Cells[2 + i, 5].Style.Numberformat.Format = "0.00";
            }
        }
    }
}