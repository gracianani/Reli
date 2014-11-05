using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Core.Interfaces;
using ReliDemo.Infrastructure.Repositories;
using ReliDemo.Infrastructure.Services;

namespace ReliDemo.Models
{
    public class DailyProductionReport : IReport
    {
        public Dictionary<int, int> dict_row_生产热源ID = new Dictionary<int, int>()
        {
            {6, 19},//国华北
            {7,7}, //华能热电厂
            {8,37}, //华能热电厂二期
            {9, 8}, //京能热电厂
            {10, 26},//高井热电厂
            {11, -1}, // ?
            {12, 18}, //华电热电厂
            {13, 15}, //郑常庄热电厂
            {14, 35}, //太阳宫热电厂
            {15, 16}, //左家庄供热厂
            {16, 22}, //方庄供热厂
            {17, 21}, //双榆树供热厂
            {18, 29} //西八里供热厂
        };
        public const int 国华电厂RowIndex = 6;
        public const int 华能电厂RowIndex = 7;
        public const int 华能二期RowIndex = 8;
        public const int 京能电厂RowIndex = 9;
        public const int 高井电厂RowIndex = 10;
        public const int 京桥电厂RowIndex = 11;
        public const int 华电二热RowIndex = 12;
        public const int 华电郑常庄RowIndex = 13;
        public const int 太阳宫RowIndex = 14;
        public const int 左家庄RowIndex = 15;
        public const int 方庄RowIndex = 16;
        public const int 双榆树RowIndex = 17;
        public const int 西八里RowIndex = 18;

        private const int 日期RowIndex = 2;
        private const int 昨日预报平均RowIndex = 5;
        private const int 昨日实况平均RowIndex = 6;
        private const int 今日预报平均RowIndex = 7;
        private const int 七日预测趋势RowIndex = 11;

        private const int 供热数据汇总RowIndex = 31;

        private const int 供热面积RowIndex = 31;
        private const int 循环水量RowIndex = 32;
        private const int 万平米循环水量RowIndex = 33;
        private const int 昨日实际RowIndex = 34;
        private const int 昨日计划RowIndex = 35;
        private const int 昨日核算RowIndex = 36;
        private const int 今日计划RowIndex = 37;

        private const int 有效监控站数_Row_Index = 41;
        private const int 监测站供热面积_Row_Index = 48;
        private const int 回温超标站个数_Row_Index = 55;
        private const int 实际超核算供热量站个数_Row_Index = 62;
        private const int 实际超核算供热量站面积_Row_Index = 69;
        private const int 核算执行到位率_Row_Index = 76;
        public ReportType ReportType
        {
            get { return ReportType.生产日报; }
        }
        private System.Xml.XmlDocument _reportContent;
        public System.Xml.XmlDocument ReportContent
        {
            get
            {
                return _reportContent;
            }
            set
            {
                _reportContent = value;
            }
        }
        public string TemplateName
        {
            get
            {
                return "生产日报";
            }
            set
            {
            }
        }

        public void FillReport(OfficeOpenXml.ExcelWorksheet worksheet)
        {
            worksheet.Cells[日期RowIndex, 1].Value = string.Format("{0:yyyy-MM-dd HH:mm tt} 生成", DateTime.Now);
            var heatSources = new HeatSourceRepository().GetAllHeatSources();

            FillWeather(worksheet);

            FillTotal(worksheet);

            FillCompanyExecutionSummary(worksheet);
        }

        protected void FillWeather(OfficeOpenXml.ExcelWorksheet worksheet)
        {
            var weatherService = new WeatherService();
            var yesterday = weatherService.GetPreviousDays(1).FirstOrDefault();
            worksheet.Cells[昨日预报平均RowIndex, 1].Value = "昨日预报平均温度：" + ( yesterday!=null ? yesterday.forecastAverage : 0.0m) + "℃";
            worksheet.Cells[昨日实况平均RowIndex, 1].Value = "昨日实况平均温度：" + weatherService.GetYeasterday().昨日实况一天平均温 + "℃";
            worksheet.Cells[今日预报平均RowIndex, 1].Value = "今日预报平均温度：" + weatherService.GetToday().forecastAverage + "℃";

            for (int i = 0; i < 7; i++)
            {
                var forcast7 = weatherService.GetSevenDays().ElementAt(i);
                worksheet.Cells[七日预测趋势RowIndex + i * 2, 2].Value = forcast7.日期.ToString("MM/dd/yyyy");
                worksheet.Cells[七日预测趋势RowIndex + i * 2, 3].Value = forcast7.天气;
                worksheet.Cells[七日预测趋势RowIndex + i * 2, 4].Value = forcast7.forecastHighest + "℃";
                worksheet.Cells[七日预测趋势RowIndex + i * 2, 5].Value = forcast7.forecastLowest + "℃";
            }
        }

        private void FillTotal(OfficeOpenXml.ExcelWorksheet worksheet)
        {
            var todayGJ = new HeatConsumptionSummaryService().GetTodaysGJ();
            var yesterdayGJ = new HeatConsumptionSummaryService().GetYesterdaysGJ();

            worksheet.Cells[供热面积RowIndex, 3].Value = todayGJ[0].全网;
            worksheet.Cells[供热面积RowIndex, 4].Value = todayGJ[0].东部;
            worksheet.Cells[供热面积RowIndex, 5].Value = todayGJ[0].西部;

            worksheet.Cells[循环水量RowIndex, 3].Value = "";
            worksheet.Cells[循环水量RowIndex, 4].Value = "";
            worksheet.Cells[循环水量RowIndex, 5].Value = "";

            worksheet.Cells[万平米循环水量RowIndex, 3].Value = todayGJ[5].全网;
            worksheet.Cells[万平米循环水量RowIndex, 4].Value = todayGJ[5].东部;
            worksheet.Cells[万平米循环水量RowIndex, 5].Value = todayGJ[5].西部;

            worksheet.Cells[昨日实际RowIndex, 3].Value = yesterdayGJ[3].全网;
            worksheet.Cells[昨日实际RowIndex, 4].Value = yesterdayGJ[3].全网;
            worksheet.Cells[昨日实际RowIndex, 5].Value = yesterdayGJ[3].全网;

            worksheet.Cells[昨日计划RowIndex, 3].Value = yesterdayGJ[1].全网;
            worksheet.Cells[昨日计划RowIndex, 4].Value = yesterdayGJ[1].全网;
            worksheet.Cells[昨日计划RowIndex, 5].Value = yesterdayGJ[1].全网;

            worksheet.Cells[昨日核算RowIndex, 3].Value = yesterdayGJ[2].全网;
            worksheet.Cells[昨日核算RowIndex, 4].Value = yesterdayGJ[2].全网;
            worksheet.Cells[昨日核算RowIndex, 5].Value = yesterdayGJ[2].全网;

            worksheet.Cells[今日计划RowIndex, 3].Value = todayGJ[1].全网;
            worksheet.Cells[今日计划RowIndex, 4].Value = todayGJ[1].东部;
            worksheet.Cells[今日计划RowIndex, 5].Value = todayGJ[1].西部;
        }
        private void FillCompanyExecutionSummary(OfficeOpenXml.ExcelWorksheet worksheet)
        {
            var companyRepo = new CompanyRepository();
            var companies = companyRepo.GetAllCompanies();
            var companyService = new CompanyService();
            var reportDate = DateTime.Today.AddDays(-1);
            for (int i = 0; i < companies.Count(); i++)
            {
                var company = companies.ElementAt(i);
                worksheet.Cells[有效监控站数_Row_Index + i, 3].Value = companyService.有效监测站数(company.ItemID, reportDate, false);
                worksheet.Cells[监测站供热面积_Row_Index + i, 3].Value = companyService.监测站供热面积(company.ItemID, reportDate, false);
                worksheet.Cells[回温超标站个数_Row_Index + i, 3].Value = companyService.回温超标45数(company.ItemID, reportDate, false);
                worksheet.Cells[实际超核算供热量站个数_Row_Index + i, 3].Value = companyService.实际超核算数(company.ItemID, reportDate, false);
                worksheet.Cells[实际超核算供热量站面积_Row_Index + i, 3].Value = companyService.实际超核算面积(company.ItemID, reportDate, false);
                worksheet.Cells[核算执行到位率_Row_Index + i, 3].Value = companyService.核算执行到位率(company.ItemID, reportDate, false);
            }
            worksheet.Cells[有效监控站数_Row_Index + 4, 3].Formula = string.Format("SUM(C41:C44)");
            worksheet.Cells[监测站供热面积_Row_Index + 4, 3].Formula = string.Format("Round(SUM(C48:C51),2)");
            worksheet.Cells[回温超标站个数_Row_Index + 4, 3].Formula = string.Format("SUM(C55:C58)");
            worksheet.Cells[实际超核算供热量站个数_Row_Index + 4, 3].Formula = string.Format("SUM(C62:C65)");
            worksheet.Cells[实际超核算供热量站面积_Row_Index + 4, 3].Formula = string.Format("Round(SUM(C69:C72),2)");
            worksheet.Cells[核算执行到位率_Row_Index + 4, 3].Formula = string.Format("Round((SUM(C52,-C73) / C52),2)");
        }
    }
}