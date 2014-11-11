
using OfficeOpenXml.Style;
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
            get { return ReportType.各单位执行到位率统计_汇总; }
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

        private void FillItem(OfficeOpenXml.ExcelWorksheet worksheet, int rowNumber, int itemIndex)
        {
            if (itemIndex == 0)
            {
                worksheet.Cells[rowNumber, 1].Value = "合计";
            }
            else
            {
                worksheet.Cells[rowNumber, 1].Value = ReportData[itemIndex].日期;
                worksheet.Cells[rowNumber, 1].Style.Numberformat.Format = "mm月dd日";
            }
            worksheet.Cells[rowNumber, 2].Value = ReportData[itemIndex].室外温度;
            worksheet.Cells[rowNumber, 2].Style.Numberformat.Format = "0.0℃";
            worksheet.Cells[rowNumber, 3].Value = ReportData[itemIndex].销售执行到位率;
            worksheet.Cells[rowNumber, 4].Value = ReportData[itemIndex].创合执行到位率;
            worksheet.Cells[rowNumber, 5].Value = ReportData[itemIndex].特力昆执行到位率;
            worksheet.Cells[rowNumber, 6].Value = ReportData[itemIndex].天禹执行到位率;
            worksheet.Cells[rowNumber, 7].Value = ReportData[itemIndex].合计执行到位率;

            worksheet.Cells[rowNumber, 8].Value = ReportData[itemIndex].销售超标站总面积;
            worksheet.Cells[rowNumber, 9].Value = ReportData[itemIndex].创合超标站总面积;
            worksheet.Cells[rowNumber, 10].Value = ReportData[itemIndex].特力昆超标站总面积;
            worksheet.Cells[rowNumber, 11].Value = ReportData[itemIndex].天禹超标站总面积;
            worksheet.Cells[rowNumber, 12].Value = ReportData[itemIndex].合计超标站总面积;

            worksheet.Cells[rowNumber, 13].Value = ReportData[itemIndex].销售有效站总面积;
            worksheet.Cells[rowNumber, 14].Value = ReportData[itemIndex].创合有效站总面积;
            worksheet.Cells[rowNumber, 15].Value = ReportData[itemIndex].特力昆有效站总面积;
            worksheet.Cells[rowNumber, 16].Value = ReportData[itemIndex].天禹有效站总面积;
            worksheet.Cells[rowNumber, 17].Value = ReportData[itemIndex].合计有效站总面积;

            worksheet.Cells[rowNumber, 18].Value = ReportData[itemIndex].销售有效站数;
            worksheet.Cells[rowNumber, 19].Value = ReportData[itemIndex].创合有效站数;
            worksheet.Cells[rowNumber, 20].Value = ReportData[itemIndex].特力昆有效站数;
            worksheet.Cells[rowNumber, 21].Value = ReportData[itemIndex].天禹有效站数;
            worksheet.Cells[rowNumber, 22].Value = ReportData[itemIndex].合计有效站数;


            var border = worksheet.Cells[rowNumber, 1].Style.Border;
            border.Right.Style = ExcelBorderStyle.Thin;
            border.Right.Color.SetColor(System.Drawing.Color.Black);

            border = worksheet.Cells[rowNumber, 2].Style.Border;
            border.Right.Style = ExcelBorderStyle.Thin;
            border.Right.Color.SetColor(System.Drawing.Color.Black);

            border = worksheet.Cells[rowNumber, 7].Style.Border;
            border.Right.Style = ExcelBorderStyle.Thin;
            border.Right.Color.SetColor(System.Drawing.Color.Black);

            border = worksheet.Cells[rowNumber, 12].Style.Border;
            border.Right.Style = ExcelBorderStyle.Thin;
            border.Right.Color.SetColor(System.Drawing.Color.Black);

            border = worksheet.Cells[rowNumber, 17].Style.Border;
            border.Right.Style = ExcelBorderStyle.Thin;
            border.Right.Color.SetColor(System.Drawing.Color.Black);

            border = worksheet.Cells[rowNumber, 22].Style.Border;
            border.Right.Style = ExcelBorderStyle.Thin;
            border.Right.Color.SetColor(System.Drawing.Color.Black);
        }

        public void FillReport(OfficeOpenXml.ExcelWorksheet worksheet)
        {
            for (int i = 1; i < ReportData.Count(); i++)
            {
                FillItem(worksheet, 2 + i, i);
            }
            FillItem(worksheet, 2 + ReportData.Count(), 0);
        }
    }
}