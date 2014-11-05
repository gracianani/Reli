
using ReliDemo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliDemo.Models
{
    public class DailyReportItem
    {
        public DateTime? 日期;
        public decimal? 室外温度;

        public decimal? 销售执行到位率;
        public decimal? 创合执行到位率;
        public decimal? 特力昆执行到位率;
        public decimal? 天禹执行到位率;
        public decimal? 合计执行到位率;

        public decimal? 销售超标站总面积;
        public decimal? 创合超标站总面积;
        public decimal? 特力昆超标站总面积;
        public decimal? 天禹超标站总面积;
        public decimal? 合计超标站总面积;

        public decimal? 销售有效站总面积;
        public decimal? 创合有效站总面积;
        public decimal? 特力昆有效站总面积;
        public decimal? 天禹有效站总面积;
        public decimal? 合计有效站总面积;

        public int? 销售有效站数;
        public int? 创合有效站数;
        public int? 特力昆有效站数;
        public int? 天禹有效站数;
        public int? 合计有效站数;

        public string GetDisplay(decimal? v, string append="", string blank = "--")
        {
            if (v.HasValue)
            {
                return v.Value + append;
            }
            return blank;
        }
        public string GetDisplay(int? v, string append = "", string blank = "--")
        {
            if (v.HasValue)
            {
                return v.Value + append;
            }
            return blank;
        }

    }
    public class DailyReport : IReport
    {
        public List<DailyReportItem> ReportData
        {
            get;
            set;
        }

        public ReportType ReportType
        {
            get { return ReportType.一站一日一计划时间段报表; }
        }

        public string TemplateName
        {
            get
            {
                return "一站一日一计划时间段报表";
            }
            set
            {
            }
        }

        public void FillReport(OfficeOpenXml.ExcelWorksheet worksheet)
        {
            for (int i = 0; i < ReportData.Count(); i++)
            {
                worksheet.Cells[3 + i, 1].Value = ReportData[i].日期;
                worksheet.Cells[3 + i, 1].Style.Numberformat.Format = "mm月dd日";
                worksheet.Cells[3 + i, 2].Value = ReportData[i].室外温度;
                worksheet.Cells[3 + i, 2].Style.Numberformat.Format = "0.0℃";
                worksheet.Cells[3 + i, 3].Value = ReportData[i].销售执行到位率;
                worksheet.Cells[3 + i, 4].Value = ReportData[i].创合执行到位率;
                worksheet.Cells[3 + i, 5].Value = ReportData[i].特力昆执行到位率;
                worksheet.Cells[3 + i, 6].Value = ReportData[i].天禹执行到位率;
                worksheet.Cells[3 + i, 7].Value = ReportData[i].合计执行到位率;

                worksheet.Cells[3 + i, 8].Value = ReportData[i].销售超标站总面积;
                worksheet.Cells[3 + i, 9].Value = ReportData[i].创合超标站总面积;
                worksheet.Cells[3 + i, 10].Value = ReportData[i].特力昆超标站总面积;
                worksheet.Cells[3 + i, 11].Value = ReportData[i].天禹超标站总面积;
                worksheet.Cells[3 + i, 12].Value = ReportData[i].合计超标站总面积;

                worksheet.Cells[3 + i, 13].Value = ReportData[i].销售有效站总面积;
                worksheet.Cells[3 + i, 14].Value = ReportData[i].创合有效站总面积;
                worksheet.Cells[3 + i, 15].Value = ReportData[i].特力昆有效站总面积;
                worksheet.Cells[3 + i, 16].Value = ReportData[i].天禹有效站总面积;
                worksheet.Cells[3 + i, 17].Value = ReportData[i].合计有效站总面积;

                worksheet.Cells[3 + i, 18].Value = ReportData[i].销售有效站数;
                worksheet.Cells[3 + i, 19].Value = ReportData[i].创合有效站数;
                worksheet.Cells[3 + i, 20].Value = ReportData[i].特力昆有效站数;
                worksheet.Cells[3 + i, 21].Value = ReportData[i].天禹有效站数;
                worksheet.Cells[3 + i, 22].Value = ReportData[i].合计有效站数;
            }
        }
    }
}