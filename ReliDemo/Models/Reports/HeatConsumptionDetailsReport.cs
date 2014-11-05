using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Core.Interfaces;
using ReliDemo.Infrastructure.Services;

namespace ReliDemo.Models
{
    public class HeatConsumptionDetailsReport : HeatConsumptionReportBase
    {
        private const int Start_Row_Index = 5;
        private const int 热力站名称_Column_Index = 1;
        private const int 日期_Column_Index = 2;
        private const int 计划供热量_Column_Index = 3;
        private const int 核算供热量_Column_Index = 4;
        private const int 实际供热量_Column_Index = 5;
        private const int 热水_Column_Index = 6;
        private const int 计算热指标_Column_Index = 7;
        private const int 计划热指标_Column_Index = 8;
        private const int 核算热指标_Column_Index = 9;
        private const int 实际热指标_Column_Index = 10;
        private const int 实际比计划多耗百分比_Column_Index = 11;
        private const int 实际比核算多耗百分比_Column_Index = 12;
        private const int 预报温度_Column_Index = 13;
        private const int 实际温度_Column_Index = 14;
        private const int 面积_Column_Index = 15;
        private const int 管理单位_Column_Index = 16;
        private const int 监控站_Column_Index = 17;
        private const int 是否重点站_Column_Index = 18;
        private const int 收费方式_Column_Index = 19;

        public override ReportType ReportType
        {
            get { return ReportType.热耗统计明细; }
        }

        public List<string> Headers
        {
            get { throw new NotImplementedException(); }
        }

        public override string TemplateName
        {
            get
            {
                return "热耗统计明细";
            }
            set
            {
                ;
            }
        }

        public override IEnumerable<StationAccuHistory> Statistics { get; set; }

        public void FillExcelHeader(OfficeOpenXml.ExcelWorksheet worksheet)
        {
            throw new NotImplementedException();
        }


        public override void FillReport(OfficeOpenXml.ExcelWorksheet worksheet)
        {
            var statList = Statistics.ToList();
            for (int i = 0, startIndex = Start_Row_Index; i < statList.Count(); i++, startIndex++)
            {
                var stat = statList.ElementAt(i);
                worksheet.Cells[startIndex, 热力站名称_Column_Index].Value = stat.热力站名称;
                worksheet.Cells[startIndex, 日期_Column_Index].Value = string.Format("{0:yyyy-MM-dd}", stat.日期);
                worksheet.Cells[startIndex, 计划供热量_Column_Index].Value = stat.计划GJ;
                worksheet.Cells[startIndex, 核算供热量_Column_Index].Value = stat.核算GJ;
                worksheet.Cells[startIndex, 实际供热量_Column_Index].Value = stat.采暖GJ;
                worksheet.Cells[startIndex, 热水_Column_Index].Value = stat.热水GJ;
                worksheet.Cells[startIndex, 计算热指标_Column_Index].Value = stat.参考热指标;
                worksheet.Cells[startIndex, 计划热指标_Column_Index].Value = stat.计划热指标;
                worksheet.Cells[startIndex, 核算热指标_Column_Index].Value = stat.核算热指标;
                worksheet.Cells[startIndex, 实际热指标_Column_Index].Value = stat.实际热指标;
                worksheet.Cells[startIndex, 实际比计划多耗百分比_Column_Index].Value = string.Format("{0:0.00}", stat.计划的多耗);
                worksheet.Cells[startIndex, 实际比核算多耗百分比_Column_Index].Value = string.Format("{0:0.00}", stat.核算的多耗);
                worksheet.Cells[startIndex, 预报温度_Column_Index].Value = stat.预报温度;
                worksheet.Cells[startIndex, 实际温度_Column_Index].Value = stat.实际温度;

                worksheet.Cells[startIndex, 面积_Column_Index].Value = stat.投入面积;
                worksheet.Cells[startIndex, 管理单位_Column_Index].Value = stat.管理单位;
                worksheet.Cells[startIndex, 监控站_Column_Index].Value = string.IsNullOrEmpty(stat.数据来源) ? "人工抄表" : stat.数据来源; ;
                worksheet.Cells[startIndex, 是否重点站_Column_Index].Value = stat.是否重点站 != 2 ? stat.是否重点站 == 1 ? "是" : "否" : "";
                worksheet.Cells[startIndex, 收费方式_Column_Index].Value = stat.收费性质; 

            }
        }
    }
}