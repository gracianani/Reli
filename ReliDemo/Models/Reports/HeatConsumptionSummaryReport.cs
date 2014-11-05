using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Core.Interfaces;
using ReliDemo.ViewModels;

namespace ReliDemo.Models
{
    public class HeatConsumptionSummaryReport :IReport
    {
        private const int Start_Row_Index = 5;
        private const int 热力站名称_Column_Index = 1;
        private const int 时间段_Column_Index = 2;
        private const int 计划供热量_Column_Index = 3;
        private const int 核算供热量_Column_Index = 4;
        private const int 实际供热量_Column_Index = 5;
        private const int 公司_Column_Index = 6;
        private const int 管理单位_Column_Index = 7;
        private const int 监控站_Column_Index = 8;
        private const int 是否重点站_Column_Index = 9;
        private const int 收费方式_Column_Index = 10;
        private const int 热源_Column_Index = 11;

        public  ReportType ReportType
        {
            get { return ReportType.热耗统计汇总; }
        }


        public  string TemplateName
        {
            get
            {
                return "热耗统计汇总";
            }
            set
            {
                ;
            }
        }
        public  IEnumerable<StationsAccuByDaysSpan> Statistics { get; set; }
        public  void FillReport(OfficeOpenXml.ExcelWorksheet worksheet)
        {
            var statList = Statistics.ToList();
            for (int i = 0, startIndex = Start_Row_Index; i < statList.Count(); i++, startIndex++)
            {
                var stat = statList.ElementAt(i);
                worksheet.Cells[startIndex, 热力站名称_Column_Index].Value = stat.热力站名称;
                worksheet.Cells[startIndex, 时间段_Column_Index].Value = stat.时间段;
                worksheet.Cells[startIndex, 计划供热量_Column_Index].Value = stat.计划GJ;
                worksheet.Cells[startIndex, 核算供热量_Column_Index].Value = stat.核算GJ;
                worksheet.Cells[startIndex, 实际供热量_Column_Index].Value = stat.实际GJ;

                worksheet.Cells[startIndex, 公司_Column_Index].Value = stat.公司;
                worksheet.Cells[startIndex, 管理单位_Column_Index].Value = stat.中心;
                worksheet.Cells[startIndex, 监控站_Column_Index].Value = string.IsNullOrEmpty(stat.数据来源) ? "人工抄表" : stat.数据来源;
                worksheet.Cells[startIndex, 是否重点站_Column_Index].Value = stat.是否重点站;
                worksheet.Cells[startIndex, 收费方式_Column_Index].Value = stat.收费性质;
                worksheet.Cells[startIndex, 热源_Column_Index].Value = stat.热源;
            }
        }
    }
}