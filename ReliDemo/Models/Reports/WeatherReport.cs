using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Core.Interfaces;
using ReliDemo.Infrastructure.Services;

namespace ReliDemo.Models
{
    public class WeatherReport : IReport
    {
        private const int Start_Row_Index = 4;
        private const int 日期_Row_Index = 3;
        private const int 预报最高温度_Row_Index = 4;
        private const int 预报最低温度_Row_Index = 5;
        private const int 预报平均温度_Row_Index = 6;
        private const int 实际最高温度_Row_Index = 7;
        private const int 实际最低温度_Row_Index = 8;
        private const int 实际平均温度_Row_Index = 9;

        public ReportType ReportType
        {
            get { return ReportType.天气预报历史; }
        }

        public string TemplateName
        {
            get
            {
                return "天气预报历史";
            }
            set
            {
                ;
            }
        }

        private DateTime _fromDate;
        public DateTime FromDate
        {
            get
            {
                return _fromDate;
            }
            set
            {
                _fromDate = value;
            }
        }

        private DateTime _toDate;
        public DateTime ToDate
        {
            get
            {
                return _toDate;
            }
            set
            {
                _toDate = value;
            }
        }

        public void FillReport(OfficeOpenXml.ExcelWorksheet worksheet)
        {
            var weathers = new WeatherService().GetHistory(FromDate, ToDate).ToList();
            for (int i = 0, startIndex = Start_Row_Index; i < weathers.Count(); i++, startIndex++)
            {
                var weather = weathers.ElementAt(i);
                worksheet.Cells[startIndex, 日期_Row_Index].Value = string.Format("{0:yyyy-MM-dd}", weather.日期);
                worksheet.Cells[startIndex, 预报最高温度_Row_Index].Value = weather.forecastHighest;
                worksheet.Cells[startIndex, 预报最低温度_Row_Index].Value = weather.forecastLowest;
                worksheet.Cells[startIndex, 预报平均温度_Row_Index].Value = weather.forecastAverage;
                worksheet.Cells[startIndex, 实际最高温度_Row_Index].Value = weather.actualHighest;
                worksheet.Cells[startIndex, 实际最低温度_Row_Index].Value = weather.actualLowest;
                worksheet.Cells[startIndex, 实际平均温度_Row_Index].Value = weather.actualAverage;
            }
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