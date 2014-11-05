using ReliDemo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliDemo.Models
{
    public class StationAnalizeReportItem
    {
        public string 热力站名称;
        public string 分公司;
        public string 管理单位;
        public decimal 站面积;
        public decimal 参考热指标;
        public string 数据来源;
        public bool 是否重点站;
        public string 收费性质;
        public string 生产热源;
        public int 统计天数;
        public int 有效天数;
        public int 无效天数;
        public int 超标天数;
        public int 未超标天数;
        public decimal 站天数到位率;
        public decimal 站有效数据率;
        public decimal 站无效数据率;
        public decimal 站天数超标率;
        public decimal 有效日计划供热量;
        public decimal 有效日核算供热量;
        public decimal 有效日实际供热量;
        public decimal 有效日供热总面积;
        public decimal 超标日总供热面积;
        public decimal 未超标日总供热面积;

    }
    public class StationAnalizeReport : IReport
    {
        public ReportType ReportType
        {
            get { return ReportType.热力站分析; }
        }

        public string TemplateName
        {
            get
            {
                return "热力站分析";
            }
            set
            {
            }
        }

        public List<StationAnalizeReportItem> ReportData { get; set; }
        public void FillReport(OfficeOpenXml.ExcelWorksheet worksheet)
        {
            for (int i = 0; i < ReportData.Count(); i++)
            {
                worksheet.Cells[3 + i, 1].Value = i + 1;
                worksheet.Cells[3 + i, 2].Value = ReportData[i].热力站名称;
                worksheet.Cells[3 + i, 3].Value = ReportData[i].分公司;
                worksheet.Cells[3 + i, 4].Value = ReportData[i].管理单位;
                worksheet.Cells[3 + i, 5].Value = ReportData[i].站面积;
                worksheet.Cells[3 + i, 6].Value = ReportData[i].参考热指标;
                worksheet.Cells[3 + i, 7].Value = ReportData[i].数据来源;
                worksheet.Cells[3 + i, 8].Value = ReportData[i].是否重点站;
                worksheet.Cells[3 + i, 9].Value = ReportData[i].收费性质;
                worksheet.Cells[3 + i, 10].Value = ReportData[i].生产热源;

                worksheet.Cells[3 + i, 11].Value = ReportData[i].统计天数;
                worksheet.Cells[3 + i, 12].Value = ReportData[i].有效天数;
                worksheet.Cells[3 + i, 13].Value = ReportData[i].无效天数;
                worksheet.Cells[3 + i, 14].Value = ReportData[i].超标天数;
                worksheet.Cells[3 + i, 15].Value = ReportData[i].未超标天数;
                worksheet.Cells[3 + i, 16].Value = ReportData[i].站天数到位率;
                worksheet.Cells[3 + i, 17].Value = ReportData[i].站有效数据率;
                worksheet.Cells[3 + i, 18].Value = ReportData[i].站无效数据率;
                worksheet.Cells[3 + i, 19].Value = ReportData[i].站天数超标率;

                worksheet.Cells[3 + i, 20].Value = ReportData[i].有效日计划供热量;
                worksheet.Cells[3 + i, 21].Value = ReportData[i].有效日核算供热量;
                worksheet.Cells[3 + i, 22].Value = ReportData[i].有效日实际供热量;

                worksheet.Cells[3 + i, 23].Value = ReportData[i].有效日供热总面积;
                worksheet.Cells[3 + i, 24].Value = ReportData[i].超标日总供热面积;
                worksheet.Cells[3 + i, 25].Value = ReportData[i].未超标日总供热面积;
            }
        }
    }
}