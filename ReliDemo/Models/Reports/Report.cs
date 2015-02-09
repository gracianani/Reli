using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;
using ReliDemo.Infrastructure.Repositories;
using ReliDemo.Core.Interfaces;
using ReliDemo.Infrastructure.Services;
using OfficeOpenXml.Drawing.Chart;
using System.Data;

namespace ReliDemo.Models
{
    public class Report : IReport
    {
        private const int Start_Row_Index = 5;
        private const int Summary_Row_Index = 9;
        private const int Chart_DateTime_Row_Index = 11;
        private const int Chart_Data_Row_Index = 12;

        private const int 公司名_Column_Index = 2;
        private const int 有效监控站数_Column_Index = 3;
        private const int 监测站供热面积_Column_Index = 4;
        private const int 回温超标站个数_Column_Index = 5;
        private const int 实际超核算供热量站个数_Column_Index = 6;
        private const int 实际超核算供热量站面积_Column_Index = 7;
        private const int 核算执行到位率_Column_Index = 8;
        private const int 实际超计划供热量站个数_Column_Index = 9;
        private const int 实际超计划供热量站面积_Column_Index =10;
        private const int 计划执行到位率_Column_Index = 11;

        private List<CompanyStat> _公司统计;
        public List<CompanyStat> 公司统计
        {
            get
            {
                if (_公司统计 == null)
                {
                    FillCompanyStats();
                }
                return _公司统计;
            }
            set
            {
                _公司统计 = value;
            }
        }

        private ReportType _reportType;
        public ReportType ReportType { 
            get { return _reportType; } 
        }

        private string _templateName;
        public string TemplateName
        {
            get
            {
                return _templateName;
            }
            set
            {
                _templateName = value;
            }
        }

        private DateTime _reportDate;
        public DateTime ReportDate
        {
            get
            {
                return _reportDate;
            }
            set
            {
                _reportDate = value;
            }
        }

        private bool _is非重点;
        public bool Is非重点
        {
            get
            {
                return _is非重点;
            }
            set
            {
                _is非重点 = value;
            }
        }

        private void PrintHeaders(ExcelWorksheet worksheet, IEnumerable<Company> companies, DateTime fromDate, DateTime toDate)
        {
            for (DateTime day = fromDate; day <= toDate; day = day.AddDays(1) )
            {
                worksheet.Cells[Chart_DateTime_Row_Index, (day - fromDate).Days + 3 ].Value = string.Format("{0:yyyy年MM月dd日}", day);
            }
            for (int i = 0; i < companies.Count(); i++)
            {
                worksheet.Cells[Chart_Data_Row_Index + i * 2, 1].Value = companies.ElementAt(i).公司;
                worksheet.Cells[Chart_Data_Row_Index + i * 2, 1, Chart_Data_Row_Index + i * 2 + 1, 1].Merge = true;
                worksheet.Cells[Chart_Data_Row_Index + i * 2, 2].Value = "核算执行到位率";
                worksheet.Cells[Chart_Data_Row_Index + i * 2 + 1, 2].Value = "计划执行到位率";
            }
        }

        public void FillCompanyStats()
        {
            var reportService = new ReportService();
            _公司统计 = new List<CompanyStat>();
            if(!_is非重点)
            {
                _公司统计.AddRange( reportService.Get有效监测站统计(ReportDate));
            }
            else
            {
                 _公司统计.AddRange( reportService.Get非重点站统计(ReportDate));
            }
        }

        public void FillReport(ExcelWorksheet worksheet)
        {
            var companyRepo = new CompanyRepository();
            var companies = companyRepo.GetAllCompanies();
            var companyService = new CompanyService();
            var chartFromDate =  new DateTime(ReportDate.Year, ReportDate.Month, 1);
            var chartToDate = chartFromDate.AddMonths(1).AddDays(-1) > ReportDate ? ReportDate : chartFromDate.AddMonths(1).AddDays(-1);

            var rowCount = ( chartToDate - chartFromDate).Days + 3 ;
            
            PrintHeaders(worksheet, companies, chartFromDate, chartToDate);
            
            for (int i = 0, startIndex = Start_Row_Index; i < companies.Count(); i++, startIndex++)
            {
                var company = companies.ElementAt(i);
                
                worksheet.Cells[startIndex, 公司名_Column_Index].Value = company.公司;
                worksheet.Cells[startIndex, 有效监控站数_Column_Index].Value = companyService.有效监测站数(company.ItemID, ReportDate, _is非重点);
                worksheet.Cells[startIndex, 监测站供热面积_Column_Index].Value = companyService.监测站供热面积(company.ItemID, ReportDate, _is非重点);
                worksheet.Cells[startIndex, 回温超标站个数_Column_Index].Value = companyService.回温超标45数(company.ItemID, ReportDate, _is非重点);
                worksheet.Cells[startIndex, 实际超核算供热量站个数_Column_Index].Value = companyService.实际超核算数(company.ItemID, ReportDate, _is非重点);
                worksheet.Cells[startIndex, 实际超核算供热量站面积_Column_Index].Value = companyService.实际超核算面积(company.ItemID, ReportDate, _is非重点);
                worksheet.Cells[startIndex, 核算执行到位率_Column_Index].Value = companyService.核算执行到位率(company.ItemID, ReportDate, _is非重点);
                worksheet.Cells[startIndex, 实际超计划供热量站个数_Column_Index].Value = companyService.实际超计划数(company.ItemID, ReportDate, _is非重点);
                worksheet.Cells[startIndex, 实际超计划供热量站面积_Column_Index].Value = companyService.实际超计划面积(company.ItemID, ReportDate, _is非重点);
                worksheet.Cells[startIndex, 计划执行到位率_Column_Index].Value = companyService.计划执行到位率(company.ItemID, ReportDate, _is非重点);

                DataTable dataPercent = companyService.到位率图表(company.ItemID, chartFromDate, chartToDate, _is非重点);
                for (int j = 0, chartDataColumn = 3; j < dataPercent.Rows.Count; j++)
                {
                    var zong = Convert.ToDecimal(dataPercent.Rows[j][0]);
                    var hesuan = Convert.ToDecimal(dataPercent.Rows[j][1]);
                    var jihua = Convert.ToDecimal(dataPercent.Rows[j][2]);
                    var date = Convert.ToDateTime(dataPercent.Rows[j][3]);
                    while(date.ToString("yyyy年MM月dd日") != worksheet.Cells[Chart_DateTime_Row_Index, chartDataColumn].Value.ToString()) {
                        chartDataColumn++;
                    }
                    worksheet.Cells[Chart_Data_Row_Index + i * 2, chartDataColumn].Value = Decimal.Round((zong - hesuan) / zong * 100.0m, 2);
                    worksheet.Cells[Chart_Data_Row_Index + i * 2 + 1, chartDataColumn].Value = Decimal.Round((zong - jihua) / zong * 100.0m, 2);
                }
            }

            worksheet.Cells[Summary_Row_Index, 有效监控站数_Column_Index].Formula = string.Format("SUM(C5:C8)");
            worksheet.Cells[Summary_Row_Index, 监测站供热面积_Column_Index].Formula = string.Format("Round(SUM(D5:D8),2)");
            worksheet.Cells[Summary_Row_Index, 回温超标站个数_Column_Index].Formula = string.Format("SUM(E5:E8)");
            worksheet.Cells[Summary_Row_Index, 实际超核算供热量站个数_Column_Index].Formula = string.Format("SUM(F5:F8)");
            worksheet.Cells[Summary_Row_Index, 实际超核算供热量站面积_Column_Index].Formula = string.Format("Round(SUM(G5:G8),2)");
            worksheet.Cells[Summary_Row_Index, 核算执行到位率_Column_Index].Formula = string.Format("Round((SUM(D9,-G9) / D9),2)");
            worksheet.Cells[Summary_Row_Index, 实际超计划供热量站个数_Column_Index].Formula = string.Format("SUM(I5:I8)");
            worksheet.Cells[Summary_Row_Index, 实际超计划供热量站面积_Column_Index].Formula = string.Format("Round(SUM(J5:J8),2)");
            worksheet.Cells[Summary_Row_Index, 计划执行到位率_Column_Index].Formula = string.Format("Round((SUM(D9,-J9) / D9),2)");

            ExcelBarChart chart = null;

            chart = worksheet.Drawings.AddChart("核算执行到位率", eChartType.ColumnClustered ) as ExcelBarChart;//设置图表样式
            chart.Legend.Position = eLegendPosition.Right;
            chart.Legend.Add();
            chart.Series.Add(
                    worksheet.Cells[Chart_Data_Row_Index, 3,
                                    Chart_Data_Row_Index, rowCount],
                    worksheet.Cells[Chart_DateTime_Row_Index , 3,
                                    Chart_DateTime_Row_Index, rowCount]);
            chart.Series.Add(
                    worksheet.Cells[Chart_Data_Row_Index + 2, 3,
                                    Chart_Data_Row_Index + 2, rowCount],
                    worksheet.Cells[Chart_DateTime_Row_Index, 3,
                                    Chart_DateTime_Row_Index, rowCount]);
            chart.Series.Add(
                    worksheet.Cells[Chart_Data_Row_Index + 4, 3,
                                    Chart_Data_Row_Index + 4, rowCount],
                    worksheet.Cells[Chart_DateTime_Row_Index, 3,
                                    Chart_DateTime_Row_Index, rowCount]);
            chart.Series.Add(
                    worksheet.Cells[Chart_Data_Row_Index + 6, 3,
                                    Chart_Data_Row_Index + 6, rowCount],
                    worksheet.Cells[Chart_DateTime_Row_Index, 3,
                                    Chart_DateTime_Row_Index, rowCount]);
            chart.Series[0].Header = companies.ElementAt(0).公司;
            chart.Series[1].Header = companies.ElementAt(1).公司;
            chart.Series[2].Header = companies.ElementAt(2).公司;
            chart.Series[3].Header = companies.ElementAt(3).公司;
            chart.Title.Text = "核算执行到位率";
            chart.SetPosition(Chart_DateTime_Row_Index, 15, 1, 7);

            chart = worksheet.Drawings.AddChart("计划执行到位率", eChartType.ColumnClustered) as ExcelBarChart;//设置图表样式
            chart.Legend.Position = eLegendPosition.Right;
            chart.Legend.Add();
            var series =  chart.Series.Add(
                    worksheet.Cells[Chart_Data_Row_Index + 1, 3,
                                    Chart_Data_Row_Index + 1, rowCount],
                    worksheet.Cells[Chart_DateTime_Row_Index, 3,
                                    Chart_DateTime_Row_Index, rowCount]);

            chart.Series.Add(
                    worksheet.Cells[Chart_Data_Row_Index + 3, 3,
                                    Chart_Data_Row_Index + 3, rowCount],
                    worksheet.Cells[Chart_DateTime_Row_Index, 3,
                                    Chart_DateTime_Row_Index, rowCount]);
            chart.Series.Add(
                    worksheet.Cells[Chart_Data_Row_Index + 5, 3,
                                    Chart_Data_Row_Index + 5, rowCount],
                    worksheet.Cells[Chart_DateTime_Row_Index, 3,
                                    Chart_DateTime_Row_Index, rowCount]);
            chart.Series.Add(
                    worksheet.Cells[Chart_Data_Row_Index + 7, 3,
                                    Chart_Data_Row_Index + 7, rowCount],
                    worksheet.Cells[Chart_DateTime_Row_Index, 3,
                                    Chart_DateTime_Row_Index, rowCount]);
            chart.Title.Text = "计划执行到位率";
            chart.Series[0].Header = companies.ElementAt(0).公司;
            chart.Series[1].Header = companies.ElementAt(1).公司;
            chart.Series[2].Header = companies.ElementAt(2).公司;
            chart.Series[3].Header = companies.ElementAt(3).公司;
            chart.SetPosition( Chart_DateTime_Row_Index+16, 15, 1, 7);

            _reportContent = worksheet.WorksheetXml;
        }

        public Report(ReportType reportType)
            : this(reportType, DateTime.Today)
        {
        }

        public Report(ReportType reportType, DateTime reportDate) : this(reportType, DateTime.Today, false)
        {
        }

        public Report(ReportType reportType, DateTime reportDate, bool is非重点)
        {
            _reportType = reportType;
            _reportDate = reportDate;
            _is非重点 = is非重点;
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
    }
}