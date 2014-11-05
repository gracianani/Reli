using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;
using ReliDemo.Core.Interfaces;
using ReliDemo.Models;

namespace ReliDemo.Infrastructure.Services
{
    public class ReportFactory
    {
        private static Dictionary<ReportType, List<string>> _reportDictionary = new Dictionary<ReportType, List<string>>
        {
            { ReportType.一日一站一计划日报, new List<string>() { "单位", "有效监控站数", "监测站供热面积(万平米)", "回温超标45站个数", "实际超核算供热量站数", "实际超核算供热量站面积(万平米)", "核算执行到位率" } },
            { ReportType.非重点站一站一日一计划, new List<string>() { "单位", "有效监控站数", "监测站供热面积(万平米)", "回温超标45站个数", "实际超核算供热量站数", "实际超核算供热量站面积(万平米)", "核算执行到位率" } },
            { ReportType.总明细, new List<string>() { "热力站名称", "管理单位", "公司", "参考热指标", "数据来源", "是否重点站",
                                                      "收费性质", "生产热源", "ItemID", "日期", "热力站ID", "总热量GJ", "热水GJ", "计划GJ",
                                                      "日单耗", "实际热指标", "核算热指标", "计划热指标", "投入面积", "实际面积", "预报温度",
                                                      "实际温度", "upHour", "核算GJ", "今日计划Area", "今日投入Area", "面积计划类别", "面积操作类型",
                                                      "供温avg", "回温avg", "供压avg", "回压avg", "瞬热avg", "瞬流avg", "万流avg" }},
            { ReportType.实际超核算明细, new List<string>() { "热力站名称", "管理单位", "公司", "参考热指标", "数据来源", "是否重点站",
                                                      "收费性质", "生产热源", "ItemID", "日期", "热力站ID", "总热量GJ", "热水GJ", "计划GJ",
                                                      "日单耗", "实际热指标", "核算热指标", "计划热指标", "投入面积", "实际面积", "预报温度",
                                                      "实际温度", "upHour", "核算GJ", "今日计划Area", "今日投入Area", "面积计划类别", "面积操作类型",
                                                      "供温avg", "回温avg", "供压avg", "回压avg", "瞬热avg", "瞬流avg", "万流avg" }} ,
            { ReportType.故障明细, new List<string>() { "热力站名称", "管理单位", "公司", "参考热指标", "数据来源", "是否重点站",
                                                      "收费性质", "生产热源", "ItemID", "日期", "热力站ID", "总热量GJ", "热水GJ", "计划GJ",
                                                      "日单耗", "实际热指标", "核算热指标", "计划热指标", "投入面积", "实际面积", "预报温度",
                                                      "实际温度", "upHour", "核算GJ", "今日计划Area", "今日投入Area", "面积计划类别", "面积操作类型",
                                                      "供温avg", "回温avg", "供压avg", "回压avg", "瞬热avg", "瞬流avg", "万流avg" }} ,
            { ReportType.回温超标明细, new List<string>() { "热力站名称", "管理单位", "公司", "参考热指标", "数据来源", "是否重点站",
                                                      "收费性质", "生产热源", "ItemID", "日期", "热力站ID", "总热量GJ", "热水GJ", "计划GJ",
                                                      "日单耗", "实际热指标", "核算热指标", "计划热指标", "投入面积", "实际面积", "预报温度",
                                                      "实际温度", "upHour", "核算GJ", "今日计划Area", "今日投入Area", "面积计划类别", "面积操作类型",
                                                      "供温avg", "回温avg", "供压avg", "回压avg", "瞬热avg", "瞬流avg", "万流avg" }} 
        };

        public static IReport CreateReport(ReportType reportType, DateTime day)
        {
            if (reportType == ReportType.一日一站一计划日报)
            {
                return new Report(reportType, day, false) { TemplateName = "一站一日一计划日报" };
            }
            else if (reportType == ReportType.非重点站一站一日一计划)
            {
                return new Report(reportType, day, true) { TemplateName = "非重点站一站一日一计划日报" };
            }
            else if (reportType == ReportType.总明细)
            {
                return new DetailReport(reportType, day) { TemplateName = "一站一日一计划总明细" };
            }
            else if (reportType == ReportType.实际超核算明细)
            {
                return new DetailReport(reportType, day) { TemplateName = "实际超核算明细" };
            }
            else if (reportType == ReportType.故障明细)
            {
                return new DetailReport(reportType, day) { TemplateName = "故障明细" };
            }
            else if (reportType == ReportType.回温超标明细)
            {
                return new DetailReport(reportType, day) { TemplateName = "回温超标明细" };
            }
            else if (reportType == ReportType.热耗统计明细)
            {
                return new HeatConsumptionDetailsReport();
            }
            else if (reportType == ReportType.热耗统计汇总)
            {
                return new HeatConsumptionSummaryReport();
            }
            else if (reportType == ReportType.天气预报历史)
            {
                return new WeatherReport();
            }
            else if (reportType == ReportType.生产日报)
            {
                return new DailyProductionReport();
            }
            throw new Exception("report type not supported");
        }

        public static IReport CreateRangeReport(ReportType reportType, DateTime fromDate, DateTime toDate, int 是否重点站 = 2, string 热源 = "ALL", string 收费性质 = "ALL", string 数据来源 = "ALL", int companyId = -1, int managershipId = -1)
        {
            if (reportType == Models.ReportType.一站一日一计划时间段报表)
            {
                var reportItem = new ReportService().GetDailyReportData(fromDate, toDate, 是否重点站, 热源, 收费性质, 数据来源);
                return new DailyReport() { ReportData = reportItem };
            }
            else if (reportType == ReportType.公司到位率统计表)
            {
                var reportItem = new ReportService().GetCompletionReportData(fromDate, toDate, 是否重点站, 热源, 收费性质, 数据来源, companyId, companyId == -1 );
                return new CompletionReport() { ReportData = reportItem };
            }
            else if (reportType == ReportType.热力站分析)
            {
                var reportItem = new ReportService().GetStationAnalizeReportData(fromDate, toDate, 是否重点站, 热源, 收费性质, 数据来源, companyId, managershipId);
                return new StationAnalizeReport() { ReportData = reportItem };
            }
            return new RangeReport() { ReportFromDate = fromDate, ReportToDate = toDate };
        }
    }
}