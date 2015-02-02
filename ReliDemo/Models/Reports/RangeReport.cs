using ReliDemo.Core.Interfaces;
using ReliDemo.Infrastructure.Repositories;
using ReliDemo.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliDemo.Models
{
    public class RangeReport : IReport
    {
        public ReportType ReportType
        {
            get { return ReportType.按时间段到位率对比; }
        }

        public string TemplateName
        {
            get
            {
                return "按时间段一站一日一计划总明细";
            }
            set
            {
            }
        }

        private Dictionary<DateTime, List<CompanyStat>> _rangeStat;
        public Dictionary<DateTime, List<CompanyStat>> RangeStat
        {
            get
            {
                if (_rangeStat == null)
                {
                    FillCompanyStats();
                }
                return _rangeStat;
            }
            set
            {
                _rangeStat = value;
            }
        }

        private Dictionary<DateTime, decimal> _temperature;
        public Dictionary<DateTime, decimal> Temperature
        {
            get
            {
                if (_temperature == null)
                {
                    FillTemperature();
                }
                return _temperature;
            }
            set
            {
                _temperature = value;
            }
        }
        public DateTime ReportFromDate { get; set; }
        public DateTime ReportToDate { get; set; }
        
        const int 日期_Column_Index = 1;
        const int 室外温度_Column_Index = 2;
        const int 销售_Column_Index = 3;
        const int 创合_Column_Index = 4;
        const int 特力昆_Column_Index = 5;
        const int 天禹_Column_Index = 6;
        const int Start_Row_Index = 2;
        
        private int GetColumnIndexByCompany(Company company)
        {
            if (company.公司.IndexOf("销售") >= 0)
            {
                return 销售_Column_Index;
            }
            else if (company.公司.IndexOf("天禹") >= 0)
            {
                return 天禹_Column_Index;
            }
            else if (company.公司.IndexOf("创合") >= 0)
            {
                return 创合_Column_Index;
            }
            else if (company.公司.IndexOf("特力昆") >= 0)
            {
                return 特力昆_Column_Index;
            }
            return -1;
        }
        public void FillCompanyStats()
        {
            var companyRepo = new CompanyRepository();
            var companies = companyRepo.GetAllCompanies();
            var companyService = new CompanyService();
            _rangeStat = new Dictionary<DateTime, List<CompanyStat>>();

            DateTime day = ReportFromDate;
            int rowIndex = Start_Row_Index;
            for (; DateTime.Compare(day, ReportToDate) <= 0; day = day.AddDays(1), rowIndex++)
            {
                var companyStat = new List<CompanyStat>();

                for (int i = 0; i < companies.Count(); i++)
                {
                    var company = companies.ElementAt(i);
                    var columnIndex = GetColumnIndexByCompany(company);
                    var 计划执行到位率 = companyService.计划执行到位率(company.ItemID, day, true);
                    companyStat.Add(new CompanyStat() { 公司名 = company.公司, 计划执行到位率 = 计划执行到位率 });
                }
                _rangeStat.Add(day, companyStat);
            }
        }

        public void FillTemperature()
        {
            var weatherService = new WeatherService();
            _temperature = new Dictionary<DateTime, decimal>();
            for (DateTime day = ReportFromDate; DateTime.Compare(day, ReportToDate) <= 0; day = day.AddDays(1))
            {
                _temperature.Add(day, weatherService.GetActual(day).Temperature ?? 0.0m);
            }
        }
        public void FillReport(OfficeOpenXml.ExcelWorksheet worksheet)
        {
            _rangeStat = new Dictionary<DateTime, List<CompanyStat>>();
            _temperature = new Dictionary<DateTime, decimal>();
            var companyRepo = new CompanyRepository();
            var companies = companyRepo.GetAllCompanies();
            var companyService = new CompanyService();
            var weatherService = new WeatherService();
            //var rowCount = (chartToDate - chartFromDate).Days + 3;

            //PrintHeaders(worksheet, companies, chartFromDate, chartToDate);
            DateTime day = ReportFromDate;
            int rowIndex = Start_Row_Index;
            for (; DateTime.Compare(day, ReportToDate) <= 0; day = day.AddDays(1), rowIndex++ )
            {
                worksheet.Cells[rowIndex, 日期_Column_Index].Value = day.ToString("MM月dd日");
                var companyStat = new List<CompanyStat>();
                var temperature = weatherService.GetActual(day);
                worksheet.Cells[rowIndex, 室外温度_Column_Index].Value = temperature.Temperature;
                for (int i = 0; i < companies.Count(); i++)
                {
                    var company = companies.ElementAt(i);
                    var columnIndex = GetColumnIndexByCompany(company);
                    var 计划执行到位率 = companyService.计划执行到位率(company.ItemID, day, true);
                    worksheet.Cells[rowIndex, columnIndex].Value = 计划执行到位率*100;
                    companyStat.Add(new CompanyStat() { 公司名 = company.公司, 计划执行到位率 = 计划执行到位率 });
                }
                _rangeStat.Add(day, companyStat);
                _temperature.Add(day, temperature.Temperature ?? 0.0m );
            }

            //    for (int i = 0, startIndex = Start_Row_Index; i < companies.Count(); i++, startIndex++)
            //    {
            //        var company = companies.ElementAt(i);

            //        worksheet.Cells[startIndex, 公司名_Column_Index].Value = company.公司;
            //        worksheet.Cells[startIndex, 有效监控站数_Column_Index].Value = companyService.有效监测站数(company.ItemID, ReportDate, _is非重点);
            //        worksheet.Cells[startIndex, 监测站供热面积_Column_Index].Value = companyService.监测站供热面积(company.ItemID, ReportDate, _is非重点);
            //        worksheet.Cells[startIndex, 回温超标站个数_Column_Index].Value = companyService.回温超标45数(company.ItemID, ReportDate, _is非重点);
            //        worksheet.Cells[startIndex, 实际超核算供热量站个数_Column_Index].Value = companyService.实际超核算数(company.ItemID, ReportDate, _is非重点);
            //        worksheet.Cells[startIndex, 实际超核算供热量站面积_Column_Index].Value = companyService.实际超核算面积(company.ItemID, ReportDate, _is非重点);
            //        worksheet.Cells[startIndex, 核算执行到位率_Column_Index].Value = companyService.核算执行到位率(company.ItemID, ReportDate, _is非重点);
            //        worksheet.Cells[startIndex, 实际超计划供热量站个数_Column_Index].Value = companyService.实际超计划数(company.ItemID, ReportDate, _is非重点);
            //        worksheet.Cells[startIndex, 实际超计划供热量站面积_Column_Index].Value = companyService.实际超计划面积(company.ItemID, ReportDate, _is非重点);
                    

            //        DataTable dataPercent = companyService.到位率图表(company.ItemID, chartFromDate, chartToDate, _is非重点);
            //        for (int j = 0, chartDataColumn = 3; j < dataPercent.Rows.Count; j++)
            //        {
            //            var zong = Convert.ToDecimal(dataPercent.Rows[j][0]);
            //            var hesuan = Convert.ToDecimal(dataPercent.Rows[j][1]);
            //            var jihua = Convert.ToDecimal(dataPercent.Rows[j][2]);
            //            var date = Convert.ToDateTime(dataPercent.Rows[j][3]);
            //            while (date.ToString("yyyy年MM月dd日") != worksheet.Cells[Chart_DateTime_Row_Index, chartDataColumn].Value.ToString())
            //            {
            //                chartDataColumn++;
            //            }
            //            worksheet.Cells[Chart_Data_Row_Index + i * 2, chartDataColumn].Value = Decimal.Round((zong - hesuan) / zong * 100.0m, 2);
            //            worksheet.Cells[Chart_Data_Row_Index + i * 2 + 1, chartDataColumn].Value = Decimal.Round((zong - jihua) / zong * 100.0m, 2);
            //        }
            //    }

            //worksheet.Cells[Summary_Row_Index, 有效监控站数_Column_Index].Formula = string.Format("SUM(C5:C8)");
            //worksheet.Cells[Summary_Row_Index, 监测站供热面积_Column_Index].Formula = string.Format("Round(SUM(D5:D8),2)");
            //worksheet.Cells[Summary_Row_Index, 回温超标站个数_Column_Index].Formula = string.Format("SUM(E5:E8)");
            //worksheet.Cells[Summary_Row_Index, 实际超核算供热量站个数_Column_Index].Formula = string.Format("SUM(F5:F8)");
            //worksheet.Cells[Summary_Row_Index, 实际超核算供热量站面积_Column_Index].Formula = string.Format("Round(SUM(G5:G8),2)");
            //worksheet.Cells[Summary_Row_Index, 核算执行到位率_Column_Index].Formula = string.Format("Round((SUM(D9,-G9) / D9),2)");
            //worksheet.Cells[Summary_Row_Index, 实际超计划供热量站个数_Column_Index].Formula = string.Format("SUM(I5:I8)");
            //worksheet.Cells[Summary_Row_Index, 实际超计划供热量站面积_Column_Index].Formula = string.Format("Round(SUM(J5:J8),2)");
            //worksheet.Cells[Summary_Row_Index, 计划执行到位率_Column_Index].Formula = string.Format("Round((SUM(D9,-J9) / D9),2)");
        }
    }
}