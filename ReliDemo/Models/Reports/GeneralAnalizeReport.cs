using ReliDemo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliDemo.Models
{
    public class GeneralAnalizeReportItem
    {
        public string 热力站名称;
        public string 管理单位;
        public decimal 投入面积;
        public int 超核算次数;
        public decimal 比例;
    }
    public class GeneralAnalizeReport : IReport
    {
        public List<GeneralAnalizeReportItem> ReportData { get; set; }
        public ReportType ReportType
        {
            get { return ReportType.数据分析表; }
        }

        public string TemplateName
        {
            get
            {
                return "数据分析表";
            }
            set
            {
            }
        }

        public void FillReport(OfficeOpenXml.ExcelWorksheet worksheet)
        {
            for (int i = 0; i < ReportData.Count(); i++)
            {
                worksheet.Cells[2 + i, 1].Value = ReportData[i].热力站名称;
                worksheet.Cells[2 + i, 2].Value = ReportData[i].管理单位;
                worksheet.Cells[2 + i, 3].Value = ReportData[i].投入面积;
                worksheet.Cells[2 + i, 4].Value = ReportData[i].超核算次数;
                worksheet.Cells[2 + i, 5].Value = ReportData[i].比例;
            }
        }
    }
}