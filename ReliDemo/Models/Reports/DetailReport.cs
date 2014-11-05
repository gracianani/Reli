using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Core.Interfaces;
using OfficeOpenXml;
using ReliDemo.Infrastructure.Repositories;
using ReliDemo.Infrastructure.Services;

namespace ReliDemo.Models
{
    public class DetailReport : IReport
    {
        private const int Start_Row_Index = 5;
        private const int 热力站名称_Column_Index = 1;
        private const int 管理单位_Column_Index = 2;	
        private const int 公司_Column_Index = 3;
        private const int 参考热指标_Column_Index = 4;
        private const int 数据来源_Column_Index = 5;
        private const int 是否重点站_Column_Index = 6;
        private const int 收费性质_Column_Index = 7;
        private const int 生产热源_Column_Index = 8;
        private const int ItemID_Column_Index = 9;
        private const int 日期_Column_Index = 10;
        private const int 热力站ID_Column_Index = 11;
        private const int 总热量GJ_Column_Index = 12;
        private const int 热水GJ_Column_Index = 13; 
        private const int 计划GJ_Column_Index = 14;
        private const int 日单耗_Column_Index = 15;	
        private const int 实际热指标_Column_Index = 16;	
        private const int 核算热指标_Column_Index = 17;	
        private const int 计划热指标_Column_Index = 18;	
        private const int 投入面积_Column_Index = 19;	
        private const int 实际面积_Column_Index = 20;	
        private const int 预报温度_Column_Index = 21;	
        private const int 实际温度_Column_Index = 22;
        private const int upHour_Column_Index = 23;
        private const int 核算GJ_Column_Index = 24; 
        private const int 今日计划Area_Column_Index = 25;	
        private const int 今日投入Area_Column_Index = 26;	
        private const int 面积计划类别_Column_Index = 27;	
        private const int 面积操作类型_Column_Index = 28;	
        private const int 供温avg_Column_Index = 29;
        private const int 回温avg_Column_Index = 30;
        private const int 供压avg_Column_Index = 31;
        private const int 回压avg_Column_Index = 32;	
        private const int 瞬热avg_Column_Index = 33;	
        private const int 瞬流avg_Column_Index = 34;
        private const int 万流avg_Column_Index = 35;

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

        private ReportType _reportType;
        public ReportType ReportType
        {
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

        public void FillReport(ExcelWorksheet worksheet)
        {
            var stationRepo = new StationRepository();
            var stations = new List<StationDetailReport>();
            if (ReportType == Models.ReportType.总明细)
            {
                stations = new StationsService().GetDailyReport(ReportDate).ToList();
            }
            else if (ReportType == Models.ReportType.回温超标明细)
            {
                stations = new StationsService().GetExceed45Stations(ReportDate).ToList();
            }
            else if (ReportType == Models.ReportType.故障明细)
            {
                stations = new StationsService().GetFailureStations(ReportDate).ToList();
            }
            else if (ReportType == Models.ReportType.实际超核算明细)
            {
                stations = new StationsService().Get超核算Stations(ReportDate).ToList();
            }
            for (int i = 0, startIndex = Start_Row_Index; i < stations.Count(); i++, startIndex++)
            {
                var station = stations.ElementAt(i);
                worksheet.Cells[startIndex, 热力站名称_Column_Index].Value = station.热力站名称;
                worksheet.Cells[startIndex, 管理单位_Column_Index].Value = station.管理单位;
                worksheet.Cells[startIndex, 公司_Column_Index].Value = station.公司;
                worksheet.Cells[startIndex, 参考热指标_Column_Index].Value = station.参考热指标;
                worksheet.Cells[startIndex, 数据来源_Column_Index].Value = station.数据来源;
                worksheet.Cells[startIndex, 是否重点站_Column_Index].Value = station.是否重点站;
                worksheet.Cells[startIndex, 收费性质_Column_Index].Value = station.收费性质;

                worksheet.Cells[startIndex, 生产热源_Column_Index].Value = station.生产热源;
                worksheet.Cells[startIndex, ItemID_Column_Index].Value = station.ItemID;
                worksheet.Cells[startIndex, 日期_Column_Index].Value = string.Format("{0:yyyy-MM-dd}", station.日期);
                worksheet.Cells[startIndex, 热力站ID_Column_Index].Value = station.热力站ID;
                worksheet.Cells[startIndex, 总热量GJ_Column_Index].Value = station.总热量GJ;
                worksheet.Cells[startIndex, 热水GJ_Column_Index].Value = station.热水GJ;
                worksheet.Cells[startIndex, 计划GJ_Column_Index].Value = station.计划GJ;

                worksheet.Cells[startIndex, 日单耗_Column_Index].Value = station.日单耗;
                worksheet.Cells[startIndex, 实际热指标_Column_Index].Value = station.实际热指标;
                worksheet.Cells[startIndex, 核算热指标_Column_Index].Value = station.核算热指标;
                worksheet.Cells[startIndex, 计划热指标_Column_Index].Value = station.计划热指标;
                worksheet.Cells[startIndex, 投入面积_Column_Index].Value = station.投入面积;
                worksheet.Cells[startIndex, 实际面积_Column_Index].Value = station.实际面积;
                worksheet.Cells[startIndex, 预报温度_Column_Index].Value = station.预报温度;

                worksheet.Cells[startIndex, 实际温度_Column_Index].Value = station.实际温度;
                worksheet.Cells[startIndex, upHour_Column_Index].Value = station.upHour;
                worksheet.Cells[startIndex, 核算GJ_Column_Index].Value = station.核算GJ;
                worksheet.Cells[startIndex, 今日计划Area_Column_Index].Value = station.今日计划Area;
                worksheet.Cells[startIndex, 今日投入Area_Column_Index].Value = station.今日投入Area;
                worksheet.Cells[startIndex, 面积计划类别_Column_Index].Value = station.面积计划类别;
                worksheet.Cells[startIndex, 面积操作类型_Column_Index].Value = station.面积操作类型;

                worksheet.Cells[startIndex, 供温avg_Column_Index].Value = station.供温avg;
                worksheet.Cells[startIndex, 回温avg_Column_Index].Value = station.回温avg;
                worksheet.Cells[startIndex, 供压avg_Column_Index].Value = station.供压avg;
                worksheet.Cells[startIndex, 回压avg_Column_Index].Value = station.回压avg;
                worksheet.Cells[startIndex, 瞬热avg_Column_Index].Value = station.瞬热avg;
                worksheet.Cells[startIndex, 瞬流avg_Column_Index].Value = station.瞬流avg;
                worksheet.Cells[startIndex, 万流avg_Column_Index].Value = station.万流avg;
            }
        }

        public DetailReport(ReportType reportType, DateTime day)
        {
            _reportType = reportType;
            _reportDate = day;
        }
    }
}