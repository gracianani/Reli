using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;
using System.Data.Objects;

namespace ReliDemo.Infrastructure.Services
{
    public class HeatConsumptionSummaryService
    {
        private xz2013Entities db = new xz2013Entities();

        public IEnumerable<TemperatureAudit> GetTemperatureAudit()
        {
            return db.TemperatureAudits.OrderByDescending(i => i.UpdatedAt);
        }

        public void InsertTemperatureAudit(decimal temperature, int userId)
        {
            db.TemperatureAudits.AddObject(new TemperatureAudit() { Temperature = temperature, UpdatedAt = DateTime.Now, UpdatedByUserId = userId });
            db.SaveChanges();
        }

        public IEnumerable<GJHistoryItem> GetHistories(DateTime fromDate, DateTime toDate)
        {

            var temperatures = db.Weather_4.Where(i => i.日期 >= fromDate && i.日期 <= toDate).AsEnumerable();

            var accuHistories=
                                from history in db.TotalNetHistories
                                from temperature in temperatures.DefaultIfEmpty()
                                where history.时间 >= fromDate && history.时间 <= toDate && history.ItemID == 3
                                orderby history.时间
                                select new GJHistoryItem
                                {
                                    日期 = history.时间 ??　DateTime.Today,
                                    核算GJ = history.核算GJ ?? 0.0m,
                                    计划GJ = history.计划GJ ?? 0,
                                    采暖GJ = history.累计GJ ?? 0.0m,
                                    预报温度 = temperature != null ? temperature.预报平均 ?? 0.0m : 0.0m,
                                    实测温度 = temperature != null ? temperature.朝阳平均 ?? 0.0m : 0.0m,
                                    计划热指标 = history.计划热指标 ?? 0.0m,
                                    核算热指标 = history.核算热指标 ?? 0.0m,
                                    实际运行热指标 = history.实际热指标 ?? 0.0m
                                };

            return accuHistories;
        }

        public IEnumerable<GJHistoryItem> GetHistoriesByCompany(int companyId, DateTime fromDate, DateTime toDate)
        {   
            var temperatures = db.Weather_4.Where(i => i.日期 >= fromDate && i.日期 <= toDate).AsEnumerable();

            var accuHistories =
                            (from history in db.CompanyHistoeys
                             from temperature in temperatures.DefaultIfEmpty()
                            where history.日期 >= fromDate && history.日期 <= toDate && history.公司ID == companyId
                             orderby history.日期
                            select new GJHistoryItem
                            {
                                日期 = history.日期.Value,
                                核算GJ = history.核算GJ_JK ?? 0.0m,
                                计划GJ = history.今日计划GJ_JK ?? 0.0m,
                                采暖GJ = history.今日累计GJ_JK ?? 0.0m,
                                预报温度 = temperature != null ? temperature.预报平均 ?? 0.0m : 0.0m,
                                实测温度 = temperature != null ? temperature.朝阳平均 ?? 0.0m : 0.0m,
                                计划热指标 = history.计划热指标 ?? 0.0m,
                                核算热指标 = history.核算热指标 ?? 0.0m,
                                实际运行热指标 = history.实际热指标 ?? 0.0m
                            }).ToList();

            return accuHistories;
        }

        public IEnumerable<GJHistoryItem> GetHistoriesByManagership(int managershipId, DateTime fromDate, DateTime toDate)
        {
            var temperatures = db.Weather_4.Where(i => i.日期 >= fromDate && i.日期 <= toDate).AsEnumerable();
            var accuHistories =
                            (from history in db.SubCompanyHistoeys
                            from temperature in temperatures.DefaultIfEmpty()
                            where history.日期 >= fromDate && history.日期 <= toDate && history.中心ID == managershipId
                             orderby history.日期
                            select new GJHistoryItem
                            {
                                日期 = history.日期.Value,
                                核算GJ = history.核算GJ_JK??　0.0m,
                                计划GJ = history.今日计划GJ_JK ?? 0.0m,
                                采暖GJ = history.今日累计GJ_JK ?? 0.0m,
                                预报温度 = temperature != null ? temperature.预报平均 ?? 0.0m :0.0m,
                                实测温度 = temperature != null ? temperature.朝阳平均 ?? 0.0m : 0.0m,
                                计划热指标 = history.计划热指标 ?? 0.0m,
                                核算热指标 = history.核算热指标 ?? 0.0m,
                                实际运行热指标 = history.实际热指标 ?? 0.0m
                            }).ToList();

            return accuHistories;
        }

        public IEnumerable<GJHistoryItem> GetHistoriesByStation(int stationId, DateTime fromDate, DateTime toDate)
        {
            var histories = new List<GJHistoryItem>();
            var accuHistories =
                            (from history in db.StationAccuHistories
                             join temperature in db.Weather_4 on history.日期 equals temperature.日期
                             where history.日期 >= fromDate && history.日期 < toDate && history.热力站ID == stationId
                             orderby history.日期
                             select new GJHistoryItem
                             {
                                 日期 = history.日期,
                                 计划GJ = history.计划GJ ?? 0.0m,
                                 采暖GJ = history.采暖GJ ?? 0.0m,
                                 核算GJ = history.核算GJ ?? 0.0m,
                                 预报温度 = temperature.预报平均,
                                 实测温度 = temperature.朝阳平均,
                                 计划热指标 = history.计划热指标 ?? 0.0m,
                                 核算热指标 = history.核算热指标 ?? 0.0m,
                                 实际运行热指标 = history.实际热指标 ?? 0.0m
                             }).ToList();
            return accuHistories;
        }

        public IEnumerable<GJHistoryItem> GetHistories(IQueryable<Station> stations, DateTime fromDate, DateTime toDate)
        {
            var histories = new List<GJHistoryItem>();
            var accuHistories = 
                            from history in db.StationAccuHistories
                            join temperature in db.Weather_4 on history.日期 equals temperature.日期
                            orderby history.日期
                            select new GJHistoryItem
                            {
                                StationId = history.热力站ID,
                                日期 = history.日期,
                                计划GJ = history.计划GJ ?? 0.0m,
                                采暖GJ = history.采暖GJ ?? 0.0m,
                                预报温度 = temperature.预报平均,
                                实测温度 = temperature.朝阳平均,
                                计划热指标 = history.计划热指标 ?? 0.0m,
                                核算热指标 = history.核算热指标 ?? 0.0m,
                                实际运行热指标 = history.实际热指标 ?? 0.0m
                            } ;
            histories = (from accuHistory in accuHistories.ToList()
                            join station in stations on accuHistory.StationId equals station.ItemID
                            select accuHistory).ToList();

            if (histories.Count() == 0)
            {
                var dateTable = new List<DateTime>();
                var span = toDate - fromDate;
                for (int i = 0; i < span.Days; i++)
                {
                    histories.Add( new GJHistoryItem() { 
                        StationId = 0, 
                        日期 = fromDate.AddDays(i), 
                        核算GJ = 0.0m, 
                        计划GJ = 0.0m, 
                        采暖GJ = 0.0m,
                        预报温度 = 0m,
                        实测温度= 0m
                    });
                }   
            }
            return histories;
        }


        public HeatConsumptionArea GetTodaysHeatConsumptionSummary()
        {
            var today = db.TotalNetRecents.Single(i=>i.ItemID == 3);
            var yesterday = db.TotalNetHistories.OrderByDescending(i=>i.时间).First(i=>i.ItemID == 3);
            var 当日计划是否投 = string.IsNullOrEmpty(today.面积计划类别) || today.面积计划类别 == "供热";
            var 当日实际是否投 = string.IsNullOrEmpty(today.面积操作类型) || today.面积操作类型 == "供热";
            var 前日计划是否投 = string.IsNullOrEmpty(yesterday.面积计划类别) || yesterday.面积计划类别 == "供热";
            var 前日实际是否投 = string.IsNullOrEmpty(yesterday.面积操作类型) || yesterday.面积操作类型 == "供热";
            var area =  new HeatConsumptionArea {
                当日计划投入面积 = 当日计划是否投 ? Decimal.Round(Convert.ToDecimal(today.今日计划Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                当日计划停热面积 = 当日计划是否投 == false ? Decimal.Round(Convert.ToDecimal(today.今日计划Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                当日实际投入面积 = 当日实际是否投 ? Decimal.Round(Convert.ToDecimal(today.今日投入Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                当日实际停热面积 = 当日实际是否投 == false ? Decimal.Round(Convert.ToDecimal(today.今日投入Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                前日计划投入面积 = 前日计划是否投 ? Decimal.Round(Convert.ToDecimal(yesterday.今日计划Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                前日计划停热面积 = 前日计划是否投 == false ? Decimal.Round(Convert.ToDecimal(yesterday.今日投入Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                前日实际投入面积 = 前日实际是否投 ? Decimal.Round(Convert.ToDecimal(yesterday.今日计划Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                前日实际停热面积 = 前日实际是否投 == false ? Decimal.Round(Convert.ToDecimal(yesterday.今日投入Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                总供热面积 = Decimal.Round( Convert.ToDecimal(today.总面积/10000.0m), 2, MidpointRounding.ToEven ),
                计算用监控面积 = Decimal.Round(Convert.ToDecimal(today.总面积 / 10000.0m), 2, MidpointRounding.ToEven),
                当日计划供热总面积 = Decimal.Round(Convert.ToDecimal(today.计划供热面积 / 10000.0m), 2, MidpointRounding.ToEven),
                当日实际供热总面积 = Decimal.Round( Convert.ToDecimal(today.实际供热面积/10000.0m), 2, MidpointRounding.ToEven ),
                前日计划供热总面积 = Decimal.Round(Convert.ToDecimal(yesterday.计划供热面积 / 10000.0m), 2, MidpointRounding.ToEven),
                前日实际供热总面积 = Decimal.Round(Convert.ToDecimal(yesterday.实际供热面积 / 10000.0m), 2, MidpointRounding.ToEven),
                当日计划是否投 = 当日计划是否投,
                当日实际是否投 = 当日实际是否投,
                前日计划是否投 = 前日计划是否投,
                前日实际是否投 = 前日实际是否投
            };
            return area;
        }

        public HeatConsumptionArea GetTodaysHeatConsumptionSummaryByCompany(int companyId)
        {
            var company = db.Companies.Single(i => i.ItemID == companyId);
            var yesterday = db.CompanyHistoeys.OrderByDescending(i => i.日期).First(i => i.公司ID == companyId);
            var 当日计划是否投 = string.IsNullOrEmpty(company.面积计划类别) || company.面积计划类别 == "供热";
            var 当日实际是否投 = string.IsNullOrEmpty(company.面积操作类型) || company.面积操作类型 == "供热";

            if (yesterday == null)
            {
                return new HeatConsumptionArea
                {
                    当日计划投入面积 = 当日计划是否投 ? Decimal.Round(Convert.ToDecimal(company.今日计划Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                    当日计划停热面积 = 当日计划是否投 == false ? Decimal.Round(Convert.ToDecimal(company.今日计划Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                    当日实际投入面积 = 当日实际是否投 ? Decimal.Round(Convert.ToDecimal(company.今日投入Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                    当日实际停热面积 = 当日实际是否投 == false ? Decimal.Round(Convert.ToDecimal(company.今日投入Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                    前日计划投入面积 = 0.0m,
                    前日计划停热面积 = 0.0m,
                    前日实际投入面积 = 0.0m,
                    前日实际停热面积 = 0.0m,
                    总供热面积 = Decimal.Round(Convert.ToDecimal(company.总面积 / 10000.0m), 2, MidpointRounding.ToEven),
                    计算用监控面积 = Decimal.Round(Convert.ToDecimal(company.Area_JK / 10000.0m), 2, MidpointRounding.ToEven),
                    当日计划供热总面积 = Decimal.Round(Convert.ToDecimal(company.计划供热面积 / 10000.0m), 2, MidpointRounding.ToEven),
                    当日实际供热总面积 = Decimal.Round(Convert.ToDecimal(company.实际供热面积 / 10000.0m), 2, MidpointRounding.ToEven),
                    前日计划供热总面积 = 0,
                    前日实际供热总面积 = 0,
                    当日计划是否投 = 当日计划是否投,
                    当日实际是否投 = 当日实际是否投,
                    前日计划是否投 = false,
                    前日实际是否投 = false
                };
            }
            
            var 前日计划是否投 = string.IsNullOrEmpty(yesterday.面积计划类别) || yesterday.面积计划类别 == "供热";
            var 前日实际是否投 = string.IsNullOrEmpty(yesterday.面积操作类型) || yesterday.面积操作类型 == "供热";
            return new HeatConsumptionArea
            {
                当日计划投入面积 = 当日计划是否投 ? Decimal.Round(Convert.ToDecimal(company.今日计划Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                当日计划停热面积 = 当日计划是否投 == false ? Decimal.Round(Convert.ToDecimal(company.今日计划Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                当日实际投入面积 = 当日实际是否投 ? Decimal.Round(Convert.ToDecimal(company.今日投入Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                当日实际停热面积 = 当日实际是否投 == false ? Decimal.Round(Convert.ToDecimal(company.今日投入Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                前日计划投入面积 = 前日计划是否投 ? Decimal.Round(Convert.ToDecimal(yesterday.今日计划Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                前日计划停热面积 = 前日计划是否投 == false ? Decimal.Round(Convert.ToDecimal(yesterday.今日计划Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                前日实际投入面积 = 前日实际是否投 ? Decimal.Round(Convert.ToDecimal(yesterday.今日投入Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                前日实际停热面积 = 前日实际是否投 == false ? Decimal.Round(Convert.ToDecimal(yesterday.今日投入Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                总供热面积 = Decimal.Round( Convert.ToDecimal(company.总面积/10000.0m), 2, MidpointRounding.ToEven ),
                计算用监控面积 = Decimal.Round( Convert.ToDecimal(company.Area_JK / 10000.0m), 2, MidpointRounding.ToEven),
                当日计划供热总面积 = Decimal.Round(Convert.ToDecimal(company.计划供热面积 / 10000.0m), 2, MidpointRounding.ToEven),
                当日实际供热总面积 = Decimal.Round(Convert.ToDecimal(company.实际供热面积 / 10000.0m), 2, MidpointRounding.ToEven),
                前日计划供热总面积 = Decimal.Round(Convert.ToDecimal(yesterday.计划供热面积 / 10000.0m), 2, MidpointRounding.ToEven),
                前日实际供热总面积 = Decimal.Round(Convert.ToDecimal(yesterday.实际供热面积 / 10000.0m), 2, MidpointRounding.ToEven),
                当日计划是否投 = 当日计划是否投,
                当日实际是否投 = 当日实际是否投,
                前日计划是否投 = 前日计划是否投,
                前日实际是否投 = 前日实际是否投
            };
        }

        public HeatConsumptionArea GetTodaysHeatConsumptionSummaryByManagership(int managershipId)
        {
            var managership = db.Managerships.Single(i => i.ItemID == managershipId);
            var yesterday = db.SubCompanyHistoeys.OrderByDescending(i => i.日期).First(i => i.中心ID == managershipId);
            var 当日计划是否投 = string.IsNullOrEmpty(managership.面积计划类别) || managership.面积计划类别 == "供热";
            var 当日投入是否投 = string.IsNullOrEmpty(managership.面积操作类型) || managership.面积操作类型 == "供热";

            if (yesterday == null)
            {
                return new HeatConsumptionArea
                {
                    当日计划投入面积 = 当日计划是否投 ? Decimal.Round(Convert.ToDecimal(managership.今日投入Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                    当日实际停热面积 = 当日计划是否投 == false ? Decimal.Round(Convert.ToDecimal(managership.今日投入Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                    当日实际投入面积 = 当日投入是否投 ? Decimal.Round(Convert.ToDecimal(managership.今日计划Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                    当日计划停热面积 = 当日投入是否投 == false ? Decimal.Round(Convert.ToDecimal(managership.今日计划Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                    前日计划投入面积 = 0.0m,
                    前日计划停热面积 = 0.0m,
                    前日实际投入面积 = 0.0m,
                    前日实际停热面积 = 0.0m,
                    总供热面积 = Decimal.Round(Convert.ToDecimal(managership.总面积 / 10000.0m), 2, MidpointRounding.ToEven),
                    计算用监控面积 = Decimal.Round(Convert.ToDecimal(managership.Area_JK / 10000.0m), 2, MidpointRounding.ToEven),
                    当日计划供热总面积 = Decimal.Round(Convert.ToDecimal(managership.计划供热面积 / 10000.0m), 2, MidpointRounding.ToEven),
                    当日实际供热总面积 = Decimal.Round(Convert.ToDecimal(managership.实际供热面积 / 10000.0m), 2, MidpointRounding.ToEven),
                    前日计划供热总面积 = 0,
                    前日实际供热总面积 = 0,
                    当日计划是否投 = 当日计划是否投,
                    当日实际是否投 = 当日投入是否投,
                    前日计划是否投 = false,
                    前日实际是否投 = false
                };
            }
            var 前日计划是否投 = string.IsNullOrEmpty(yesterday.面积计划类别) || yesterday.面积计划类别 == "供热";
            var 前日投入是否投 = string.IsNullOrEmpty(yesterday.面积操作类型) || yesterday.面积操作类型 == "供热";
            return new HeatConsumptionArea
            {
                当日计划投入面积 = 当日计划是否投 ? Decimal.Round(Convert.ToDecimal(managership.今日投入Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                当日实际停热面积 = 当日计划是否投 == false ? Decimal.Round( Convert.ToDecimal(managership.今日投入Area/10000.0m), 2, MidpointRounding.ToEven ) : 0.0m,
                当日实际投入面积 = 当日投入是否投 ? Decimal.Round(Convert.ToDecimal(managership.今日计划Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                当日计划停热面积 = 当日投入是否投 == false ? Decimal.Round( Convert.ToDecimal(managership.今日计划Area/10000.0m), 2, MidpointRounding.ToEven ) : 0.0m,
                前日计划投入面积 = 前日计划是否投 ? Decimal.Round(Convert.ToDecimal(yesterday.今日计划Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                前日计划停热面积 = 前日计划是否投 == false ? Decimal.Round(Convert.ToDecimal(yesterday.今日计划Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                前日实际投入面积 = 前日投入是否投 ? Decimal.Round(Convert.ToDecimal(yesterday.今日投入Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                前日实际停热面积 = 前日投入是否投 == false ? Decimal.Round(Convert.ToDecimal(yesterday.今日投入Area / 10000.0m), 2, MidpointRounding.ToEven) : 0.0m,
                总供热面积 = Decimal.Round(Convert.ToDecimal(managership.总面积 / 10000.0m), 2, MidpointRounding.ToEven),
                计算用监控面积 = Decimal.Round(Convert.ToDecimal(managership.Area_JK / 10000.0m), 2, MidpointRounding.ToEven),
                当日计划供热总面积 = Decimal.Round(Convert.ToDecimal(managership.计划供热面积 / 10000.0m), 2, MidpointRounding.ToEven),
                当日实际供热总面积 = Decimal.Round(Convert.ToDecimal(managership.实际供热面积 / 10000.0m), 2, MidpointRounding.ToEven),
                前日计划供热总面积 = Decimal.Round(Convert.ToDecimal(yesterday.计划供热面积 / 10000.0m), 2, MidpointRounding.ToEven),
                前日实际供热总面积 = Decimal.Round(Convert.ToDecimal(yesterday.实际供热面积 / 10000.0m), 2, MidpointRounding.ToEven),
                当日计划是否投 = 当日计划是否投,
                当日实际是否投 = 当日投入是否投,
                前日计划是否投 = 前日计划是否投,
                前日实际是否投 = 前日投入是否投
            };
        }
        public IList<HeatConsumptionTotalItem> GetTodaysGJ总ByCompanyId(int companyId)
        {
            var company = db.Companies.Single(i => i.ItemID == companyId);
            
            var heatunitperdict = new HeatConsumptionTotalItem()
            {
                Title = "供暖季预期单耗(GJ/㎡)",
                总 = Convert.ToDecimal(company.预估采暖季单耗),
                IsToday = true
            };
            return new List<HeatConsumptionTotalItem>() {  heatunitperdict };
        }
        public IList<HeatConsumptionTotalItem> GetTodaysGJByCompanyId(int companyId)
        {
            var company = db.Companies.Single(i => i.ItemID == companyId);
            var manualCount = company.Stations.Where(i => string.IsNullOrEmpty(i.数据来源) && (i.生产热源ID.HasValue && (i.生产热源ID == 1 || i.生产热源ID == 22))).Count(); 

            var count = new HeatConsumptionTotalItem()
            {
                Title = "热力站数量",
                监测站智能卡 = company.有效监控站数,
                总 = manualCount + company.有效监控站数,
                人工抄表 = manualCount,
                IsToday = true
            };
            var area = new HeatConsumptionTotalItem()
            {
                Title = "实际投入面积(万㎡)",
                监测站智能卡 = Decimal.Round(Convert.ToDecimal(company.Area_JK / 10000.0m), 2, MidpointRounding.ToEven),
                总 = Decimal.Round(Convert.ToDecimal(company.实际供热面积 / 10000.0m), 2, MidpointRounding.ToEven),
                人工抄表 = Decimal.Round(Convert.ToDecimal((company.实际供热面积 - company.Area_JK) / 10000.0m), 2, MidpointRounding.ToEven),
                IsToday = true
            };

            var plan = new HeatConsumptionTotalItem()
            {
                Title = "计划供热量(GJ)",
                总 = Convert.ToDecimal(company.今日计划GJ), 
                监测站智能卡 = Convert.ToDecimal(company.今日计划GJ_JK),
                人工抄表 = Convert.ToDecimal(company.今日计划GJ) - Convert.ToDecimal(company.今日计划GJ_JK),
                IsToday = true
            };
            var actual = new HeatConsumptionTotalItem()
            {
                Title = "实际供热量(GJ)",
                监测站智能卡 = Convert.ToDecimal(company.今日累计GJ_JK) ,
                IsToday = true
            };

            var seasonal = new HeatConsumptionTotalItem()
            {
                Title = "供暖季累计供热量(GJ)",
                监测站智能卡 = Convert.ToDecimal(company.供暖季累计GJ_JK),
                IsToday = true
            };

            var todayPerdict = new HeatConsumptionTotalItem()
            {
                Title = "预计供热量(GJ)",
                总 = null,
                监测站智能卡 = Convert.ToDecimal(company.预计全天GJ),
                人工抄表 = null,
                IsToday = true
            };
            var water = new HeatConsumptionTotalItem()
            {
                Title = "万平米循环水量(t/h)",
                总 = null,
                监测站智能卡 = Convert.ToDecimal(company.万平方米流量),
                人工抄表 = null,
                IsToday = true
            };

            var calculated_heatindex = new HeatConsumptionTotalItem()
            {
                Title = "计算热指标(kcal/h•㎡)",
                总 = Convert.ToDecimal(company.参考热指标),
                监测站智能卡 = Convert.ToDecimal(company.参考热指标),
                人工抄表 = Convert.ToDecimal(company.参考热指标),
                IsToday = true
            };
            var planned_heatindex = new HeatConsumptionTotalItem()
            {
                Title = "计划热指标(kcal/h•㎡)",
                监测站智能卡 = Convert.ToDecimal(company.计划热指标),
                总 = Convert.ToDecimal(company.计划热指标),
                人工抄表 = Convert.ToDecimal(company.计划热指标),
                IsToday = true
            };
           
            var actual_heatindex = new HeatConsumptionTotalItem()
            {
                Title = "实际运行热指标(kcal/h•㎡)",
                总 = Convert.ToDecimal(company.实际热指标),
                监测站智能卡 = Convert.ToDecimal(company.实际热指标_JK),
                人工抄表 = null,
                IsToday = true
            };
            
            var accu_heatUnit = new HeatConsumptionTotalItem()
            {
                Title = "供暖季累计单耗(GJ/㎡)",
                总 =  Convert.ToDecimal( company.供暖季累计单耗),
                监测站智能卡 = Convert.ToDecimal( company.供暖季累计单耗_JK), 
                人工抄表 = null,
                IsToday = true
            };
            var expected_heatUnit = new HeatConsumptionTotalItem()
            {
                Title = "供暖季预期单耗(GJ/㎡)",
                总 = Convert.ToDecimal( company.预估采暖季单耗 ),
                监测站智能卡 =  Convert.ToDecimal(company.预估采暖季单耗_JK),
                人工抄表 = null,
                IsToday = true
            };
            return new List<HeatConsumptionTotalItem>() { count, area, plan, actual, todayPerdict, seasonal, water, calculated_heatindex, planned_heatindex, actual_heatindex, accu_heatUnit, expected_heatUnit };
        }
        public IList<HeatConsumptionTotalItem> GetTodaysGJRealTimeByManagershipId(int managershipId)
        {
            var managership = db.Managerships.Single(i => i.ItemID == managershipId);
            
            var actual = new HeatConsumptionTotalItem()
            {
                Title = "实际供热量(GJ)",
                总 = Convert.ToDecimal(managership.今日累计GJ),
                IsToday = true
            };
            var seasonal = new HeatConsumptionTotalItem()
            {
                Title = "供暖季累计供热量(GJ)",
                总 = Convert.ToDecimal(managership.供暖季累计GJ),
                IsToday = true
            };
            var water = new HeatConsumptionTotalItem()
            {
                Title = "万平米循环水量(t/h)",
                总 = Convert.ToDecimal(managership.万平方米流量),
                IsToday = true
            };
            var actual_heatindex = new HeatConsumptionTotalItem()
            {
                Title = "实际运行热指标(kcal/h•㎡)",
                总 = Convert.ToDecimal(managership.实际热指标),
                IsToday = true
            };
            var heatUnitConsumptionPerSeason = new HeatConsumptionTotalItem()
            {
                Title = "供暖季累计单耗(GJ/㎡)",
                总 = Convert.ToDecimal(managership.供暖季累计单耗),
                IsToday = true
            };
            return new List<HeatConsumptionTotalItem>() { actual, seasonal,  water, actual_heatindex, heatUnitConsumptionPerSeason };
        }
        public IList<HeatConsumptionTotalItem> GetTodaysGJ总ByManagershipId(int managershipId)
        {
            var managership = db.Managerships.Single(i => i.ItemID == managershipId);
           
            var heatunitperdict = new HeatConsumptionTotalItem()
            {
                Title = "供暖季预期单耗(GJ/㎡)",
                总 = Convert.ToDecimal(managership.预估采暖季单耗),
                IsToday = true
            };
            return new List<HeatConsumptionTotalItem>() {  heatunitperdict };
        }
        public IList<HeatConsumptionTotalItem> GetTodaysGJByManagershipId(int managershipId)
        {
            
            var managership = db.Managerships.Single(i => i.ItemID == managershipId);
            var manualCount = managership.Stations.Where(i => string.IsNullOrEmpty(i.数据来源) && (i.生产热源ID.HasValue && (i.生产热源ID == 1 || i.生产热源ID == 22)) ).Count();
            var count = new HeatConsumptionTotalItem()
            {
                Title = "热力站数量",
                监测站智能卡 = managership.有效监控站数,
                总 = manualCount + managership.有效监控站数,
                人工抄表 = manualCount,
                IsToday = true
            };
            var area = new HeatConsumptionTotalItem()
            {
                Title = "实际投入面积(万㎡)",
                监测站智能卡 = Decimal.Round(Convert.ToDecimal(managership.Area_JK / 10000.0m), 2, MidpointRounding.ToEven),
                总 = Decimal.Round(Convert.ToDecimal(managership.实际供热面积 / 10000.0m), 2, MidpointRounding.ToEven),
                人工抄表 = Decimal.Round(Convert.ToDecimal((managership.实际供热面积 - managership.Area_JK) / 10000.0m), 2, MidpointRounding.ToEven),
                IsToday = true
            };
            var plan = new HeatConsumptionTotalItem()
            {
                Title = "计划供热量(GJ)",
                监测站智能卡 = Convert.ToDecimal(managership.今日计划GJ_JK),
                总= Convert.ToDecimal(managership.今日计划GJ),
                人工抄表 = Convert.ToDecimal(managership.今日计划GJ) - Convert.ToDecimal(managership.今日计划GJ_JK),
                IsToday = true
            };
            var actual = new HeatConsumptionTotalItem()
            {
                Title = "实际供热量(GJ)",
                监测站智能卡 = Convert.ToDecimal(managership.今日累计GJ_JK),
                IsToday = true
            };
            var seasonal = new HeatConsumptionTotalItem()
            {
                Title = "供暖季累计供热量(GJ)",
                监测站智能卡 = Convert.ToDecimal(managership.供暖季累计GJ_JK),
                IsToday = true
            };
            var todayPerdict = new HeatConsumptionTotalItem()
            {
                Title = "预计供热量(GJ)",
                监测站智能卡 = Convert.ToDecimal(managership.预计全天GJ),
                IsToday = true
            };
            var water = new HeatConsumptionTotalItem()
            {
                Title = "万平米循环水量(t/h)",
                总 =Convert.ToDecimal(managership.万平方米流量),
                监测站智能卡 = null,
                IsToday = true
            };
            var calculated_heatindex = new HeatConsumptionTotalItem()
            {
                Title = "计算热指标(kcal/h•㎡)",
                监测站智能卡 = Convert.ToDecimal(managership.参考热指标),
                总 = Convert.ToDecimal(managership.参考热指标),
                人工抄表 = Convert.ToDecimal(managership.参考热指标),
                IsToday = true
            };
            //company.供暖季核算GJ_总 company.供暖季累计GJ_总 company.供暖季累计单耗_总
            //company.供暖季累计单耗_M company.实际热指标_M company.核算热指标_M company.阶段核算累计GJ_M company.阶段累计GJ_M
            var planned_heatindex = new HeatConsumptionTotalItem()
            {
                Title = "计划热指标(kcal/h•㎡)",
                总 = Convert.ToDecimal(managership.计划热指标),
                监测站智能卡 = Convert.ToDecimal(managership.计划热指标),
                人工抄表 = Convert.ToDecimal(managership.计划热指标),
                IsToday = true
            };
            var actual_heatindex = new HeatConsumptionTotalItem()
            {
                Title = "实际运行热指标(kcal/h•㎡)",
                总 = Convert.ToDecimal(managership.实际热指标),
                监测站智能卡 = Convert.ToDecimal(managership.实际热指标_JK),
                人工抄表 = null,
                IsToday = true
            };
            var accu_seasonal = new HeatConsumptionTotalItem()
            {
                Title = "供暖季累计单耗(GJ/㎡)",
                总 = Convert.ToDecimal(managership.供暖季累计单耗),
                监测站智能卡 = Convert.ToDecimal(managership.供暖季累计单耗_JK),
                IsToday = true
            };
            var expected_seasonal = new HeatConsumptionTotalItem()
            {
                Title = "供暖季预期单耗(GJ/㎡)",
                监测站智能卡 = Convert.ToDecimal(managership.预估采暖季单耗_JK),
                总 = Convert.ToDecimal(managership.预估采暖季单耗),
                IsToday = true
            };
            return new List<HeatConsumptionTotalItem>() { count, area, plan, actual, todayPerdict, seasonal, water, calculated_heatindex, planned_heatindex, actual_heatindex,accu_seasonal, expected_seasonal };
        }

        public IList<HeatConsumptionTotalItem> GetTodayAndYesterdaysGJByStation(int stationId)
        {
            var today = DateTime.Today;
            var yesterday = DateTime.Today.AddDays(-1);
            var history = db.StationAccuHistories.Where(i => i.热力站ID == stationId && i.日期 == yesterday);
            var station = db.Stations.Single(i => i.ItemID == stationId);
            var weatherService = new WeatherService();

            var area = new HeatConsumptionTotalItem()
            {
                Title = "实际投入面积(㎡)",
                今日 = Convert.ToDecimal( station.投入面积 ),
                昨日 = Convert.ToDecimal( station.截止昨日实际Area )
            };

            var plan = new HeatConsumptionTotalItem()
            {
                Title = "计划供热量(GJ)",
                今日 = Decimal.Round( Convert.ToDecimal(station.今日计划GJ), 0),
                昨日 = Decimal.Round(Convert.ToDecimal( station.昨日计划GJ ), 0) 
            };

            var actual = new HeatConsumptionTotalItem()
            {
                Title = "实际供热量(GJ)",
                今日 = Decimal.Round(Convert.ToDecimal(station.今日GJ), 0),
                昨日 = Decimal.Round(Convert.ToDecimal(station.昨日GJ), 0)
            };

            var actual_water = new HeatConsumptionTotalItem()
            {
                Title = "实际热水(GJ)",
                今日 = Decimal.Round( Convert.ToDecimal(station.今日热水GJ), 0),
                昨日 = Decimal.Round( Convert.ToDecimal(station.昨日热水GJ), 0)
            };

            var calculated = new HeatConsumptionTotalItem()
            {
                Title = "核算供热量(GJ)",
                今日 = Decimal.Round( Convert.ToDecimal(station.今日核算GJ), 0),
                昨日 = Decimal.Round( Convert.ToDecimal(station.昨日核算GJ), 0)
            };

            var perdict = new HeatConsumptionTotalItem()
            {
                Title = "预计供热量(GJ)",
                今日 = Convert.ToDecimal(station.预计全天GJ)
            };

            var seasonal = new HeatConsumptionTotalItem()
            {
                Title = "供暖季累计供热量(GJ)",
                今日 = Convert.ToDecimal(station.供暖季GJ) + Convert.ToDecimal(station.今日GJ),
                昨日 = Decimal.Round(Convert.ToDecimal(station.供暖季GJ), 0)
            };

            var water = new HeatConsumptionTotalItem()
            {
                Title = "万平米循环水量(t)",
                今日 = Decimal.Round(Convert.ToDecimal(station.万平方米流量), 2),
                昨日 = Decimal.Round( Convert.ToDecimal(station.万平方米流量), 2)
            };
            var heatIndexReferred = new HeatConsumptionTotalItem()
            {
                Title = "计算热指标(Kcal/h·㎡)",
                今日 = Decimal.Round(Convert.ToDecimal(station.参考热指标),2),
                昨日 = Decimal.Round(Convert.ToDecimal(station.参考热指标), 2)
            };
            var heatIndexPlan = new HeatConsumptionTotalItem()
            {
                Title = "计划热指标(Kcal/h·㎡)",
                今日 = Decimal.Round(Convert.ToDecimal(station.计划热指标),2),
                昨日 = Decimal.Round(Convert.ToDecimal(station.昨日计划热指标),2)
            };
            var heatIndexCalculated = new HeatConsumptionTotalItem()
            {
                Title = "核算热指标(Kcal/h·㎡)",
                今日 = null,
                昨日 = Decimal.Round(Convert.ToDecimal(station.昨日核算热指标),2)
            };
            var heatIndexActual = new HeatConsumptionTotalItem()
            {
                Title = "实际热指标(Kcal/h·㎡)",
                今日 = Decimal.Round(Convert.ToDecimal(station.实际热指标),2),
                昨日 = Decimal.Round(Convert.ToDecimal(station.昨日实际热指标),2)
            };
            var accu_heatUnitConsumption = new HeatConsumptionTotalItem()
            {
                Title = "供暖季累计单耗(GJ/㎡)",
                今日 = Decimal.Round(Convert.ToDecimal(station.采暖季单耗),2),
                昨日 = Decimal.Round(Convert.ToDecimal(station.采暖季单耗),2)
            };
            var expected_heatUnitConsumption = new HeatConsumptionTotalItem()
            {
                Title = "供暖季预期单耗(GJ/㎡)",
                今日 = Decimal.Round(Convert.ToDecimal(station.预估采暖季单耗),2),
                昨日 = Decimal.Round(Convert.ToDecimal(station.预估采暖季单耗),2)
            };
            var exceedPlan = new HeatConsumptionTotalItem()
            {
                Title = "实际供热量与计划供热量差值(GJ)",
                今日 = Decimal.Round(Convert.ToDecimal( station.今日GJ - station.今日计划GJ)),
                昨日 = Decimal.Round(Convert.ToDecimal(station.昨日GJ - station.昨日计划GJ))
            };
            var exceedCalculated = new HeatConsumptionTotalItem()
            {
                Title = "实际供热量与核算供热量差值(GJ)",
                昨日 = Decimal.Round(Convert.ToDecimal(station.昨日GJ - station.昨日核算GJ))
            };
            return new List<HeatConsumptionTotalItem>() { area, plan, calculated, actual, actual_water, perdict, exceedPlan, exceedCalculated, seasonal, water, heatIndexReferred, heatIndexPlan, heatIndexCalculated, heatIndexActual, accu_heatUnitConsumption, expected_heatUnitConsumption };
        }

    
        public int GetHeatConsumptionAccuByManagership(int managershipId, DateTime date)
        {
            if (date == DateTime.Today)
            {
                return Convert.ToInt32(db.Managerships.First(i => i.ItemID == managershipId).今日累计GJ ?? 0.0m);
            }
            else
            {
                try
                {
                    return Convert.ToInt32(db.SubCompanyHistoeys.First(i => i.日期 == date && i.ItemID == managershipId).今日累计GJ ?? 0.0m);
                }
                catch
                {
                    return Convert.ToInt32(db.Companies.First(i => i.ItemID == managershipId).今日累计GJ ?? 0.0m);
                }
            }
        }

        public int GetHeatConsumptionAccuByCompany(int companyId, DateTime date)
        {
            if (date == DateTime.Today)
            {
                return Convert.ToInt32( db.Companies.First(i => i.ItemID == companyId).今日累计GJ ?? 0.0m );
            }
            else
            {
                try
                {
                    return Convert.ToInt32(db.CompanyHistoeys.First(i => i.日期 == date && i.ItemID == companyId).今日累计GJ ?? 0.0m);
                }
                catch
                {
                    return Convert.ToInt32(db.Companies.First(i => i.ItemID == companyId).今日累计GJ ?? 0.0m);
                }
            }
        }

        public int GetHeatConsumptionAccuByDate(Region region, DateTime date)
        {
            if (date == DateTime.Today)
            {
                return db.TotalNetRecents.Single(i => i.ItemID == (int)region).今日累计GJ ?? 0;
            }
            else
            {
                try
                {
                    return db.TotalNetHistories.First(i => i.时间 == date && i.ItemID == (int)region).累计GJ ?? 0;
                }
                catch
                {
                    return db.TotalNetRecents.Single(i => i.ItemID == (int)region).昨日累计GJ ?? 0;
                }
            }
        }

        public IList<HeatConsumptionTotalItem> GetTodaysGJRealTime()
        {
            var actual = new HeatConsumptionTotalItem()
            {
                Title = "实际供热量(GJ)",
                全网 = Convert.ToDecimal(db.TotalNetRecents.Single(i => i.ItemID == 3).今日累计GJ),
                东部 = Convert.ToDecimal(db.TotalNetRecents.Single(i => i.ItemID == 1).今日累计GJ),
                西部 = Convert.ToDecimal(db.TotalNetRecents.Single(i => i.ItemID == 2).今日累计GJ),
                IsToday = true
            };
            var total = new HeatConsumptionTotalItem()
            {
                Title = "供暖季累计供热量(GJ)",
                全网 = Convert.ToDecimal(db.TotalNetRecents.Single(i => i.ItemID == 3).供暖季累计GJ),
                东部 = Convert.ToDecimal(db.TotalNetRecents.Single(i => i.ItemID == 1).供暖季累计GJ),
                西部 = Convert.ToDecimal(db.TotalNetRecents.Single(i => i.ItemID == 2).供暖季累计GJ),
                IsToday = true
            };
            var water = new HeatConsumptionTotalItem()
            {
                Title = "万平米循环水量(t/h)",
                全网 = Convert.ToDecimal(db.TotalNetRecents.Single(i => i.ItemID == 3).万平方米流量),
                东部 = Convert.ToDecimal(db.TotalNetRecents.Single(i => i.ItemID == 1).万平方米流量),
                西部 = Convert.ToDecimal(db.TotalNetRecents.Single(i => i.ItemID == 2).万平方米流量),
                IsToday = true
            };
            var actual_heatindex = new HeatConsumptionTotalItem()
            {
                Title = "实际运行热指标(kcal/h·㎡)",
                全网 = Convert.ToDecimal(db.TotalNetRecents.Single(i => i.ItemID == 3).实际热指标),
                东部 = Convert.ToDecimal(db.TotalNetRecents.Single(i => i.ItemID == 1).实际热指标),
                西部 = Convert.ToDecimal(db.TotalNetRecents.Single(i => i.ItemID == 2).实际热指标)
            };
            var accu_heatUnit = new HeatConsumptionTotalItem()
            {
                Title = "供暖季累计单耗(GJ/㎡)",
                全网 = Convert.ToDecimal(db.TotalNetRecents.Single(i => i.ItemID == 3).供暖季累计单耗),
                东部 = Convert.ToDecimal(db.TotalNetRecents.Single(i => i.ItemID == 1).供暖季累计单耗),
                西部 = Convert.ToDecimal(db.TotalNetRecents.Single(i => i.ItemID == 2).供暖季累计单耗)
            };
            return new List<HeatConsumptionTotalItem>() { actual, total, water, actual_heatindex, accu_heatUnit };
        }

        public IList<HeatConsumptionTotalItem> GetTodaysGJRealTimeByCompanyId( int companyId )
        {
            var company = db.Companies.Single(i => i.ItemID == companyId);
            var actual = new HeatConsumptionTotalItem()
            {
                Title = "实际供热量(GJ)",
                总 = Convert.ToDecimal(company.今日累计GJ),
                IsToday = true
            };
            var total = new HeatConsumptionTotalItem()
            {
                Title = "供暖季累计供热量(GJ)",
                总 = Convert.ToDecimal(company.供暖季累计GJ),
                IsToday = true
            };
            var water = new HeatConsumptionTotalItem()
            {
                Title = "万平米循环水量(t/h)",
                全网 = Convert.ToDecimal(company.万平方米流量),
                IsToday = true
            };
            var actual_heatindex = new HeatConsumptionTotalItem()
            {
                Title = "实际运行热指标(kcal/h·㎡)",
                全网 = Convert.ToDecimal(company.实际热指标),
                IsToday = true
            };
            var accu_heatUnit = new HeatConsumptionTotalItem()
            {
                Title = "供暖季累计单耗(GJ/㎡)",
                全网 = Convert.ToDecimal(company.供暖季累计单耗),
                IsToday = true
            };
            return new List<HeatConsumptionTotalItem>() { actual, total, water, actual_heatindex, accu_heatUnit };
        }

        public IList<HeatConsumptionTotalItem> GetTodaysGJ()
        {
            var total = db.TotalNetRecents.Single(i => i.ItemID == 3);
            var east = db.TotalNetRecents.Single(i => i.ItemID == 1);
            var west = db.TotalNetRecents.Single(i => i.ItemID == 2);
            var area = new HeatConsumptionTotalItem()
            {
                Title = "实际投入面积(万㎡)",
                全网 = Decimal.Round(Convert.ToDecimal(total.实际供热面积 / 10000.0m), 2),
                东部 = Decimal.Round( Convert.ToDecimal( east.实际供热面积 / 10000.0m), 2),
                西部 = Decimal.Round( Convert.ToDecimal(west.实际供热面积 / 10000.0m), 2),
                IsToday=true
            };
            var plan_early = new HeatConsumptionTotalItem()
            {
                Title = "计划供热量(GJ)",
                全网 = Decimal.Round(Convert.ToDecimal(total.今日计划GJ), 0),
                东部 = Decimal.Round( Convert.ToDecimal(east.今日计划GJ), 0),
                西部 = Decimal.Round( Convert.ToDecimal(west.今日计划GJ), 0),
                IsToday = true
            };
            var actual = new HeatConsumptionTotalItem()
            {
                Title = "实际供热量(GJ)",
                全网 = Decimal.Round(Convert.ToDecimal(total.今日累计GJ), 0),
                东部 = Decimal.Round( Convert.ToDecimal(east.今日累计GJ), 0),
                西部 = Decimal.Round( Convert.ToDecimal(west.今日累计GJ), 0),
                IsToday = true
            };
            var todayPredict = new HeatConsumptionTotalItem()
            {
                Title= "预计供热量(GJ)",
                全网 = Convert.ToDecimal(total.预计全天GJ),
                东部 = Convert.ToDecimal(east.预计全天GJ),
                西部 = Convert.ToDecimal(west.预计全天GJ),
                IsToday = true
            };
            var seasonal = new HeatConsumptionTotalItem()
            {
                Title = "供暖季累计供热量(GJ)",
                全网 = Convert.ToDecimal(total.供暖季累计GJ ) + Convert.ToDecimal(total.今日累计GJ),
                东部 = Convert.ToDecimal(east.供暖季累计GJ)+ Convert.ToDecimal(east.今日累计GJ),
                西部 = Convert.ToDecimal(west.供暖季累计GJ) + Convert.ToDecimal(west.今日累计GJ),
                IsToday = true
            };
            var water = new HeatConsumptionTotalItem()
            {
                Title = "万平米循环水量(t/h)",
                全网 = Decimal.Round(Convert.ToDecimal(total.万平方米流量), 2),
                东部 = Decimal.Round(Convert.ToDecimal(east.万平方米流量),2 ),
                西部 = Decimal.Round(Convert.ToDecimal(west.万平方米流量), 2)
            };
            var calculated_heatindex = new HeatConsumptionTotalItem()
            {
                Title = "计算热指标(kcal/h·㎡)",
                全网 = Decimal.Round(Convert.ToDecimal(total.参考热指标), 2),
                东部 = Decimal.Round(Convert.ToDecimal(east.参考热指标),2 ),
                西部 = Decimal.Round(Convert.ToDecimal(west.参考热指标), 2)
            };
            var planned_heatindex = new HeatConsumptionTotalItem()
            {
                Title = "计划热指标(kcal/h·㎡)",
                全网 = Decimal.Round(Convert.ToDecimal(total.计划热指标), 2),
                东部 = Decimal.Round(Convert.ToDecimal(east.计划热指标),2),
                西部 = Decimal.Round(Convert.ToDecimal(west.计划热指标),2)
            };
            var actual_heatindex = new HeatConsumptionTotalItem()
            {
                Title = "实际运行热指标(kcal/h·㎡)",
                全网 = Decimal.Round(Convert.ToDecimal(total.实际热指标), 2, MidpointRounding.AwayFromZero),
                东部 = Decimal.Round(Convert.ToDecimal(east.实际热指标), 2),
                西部 = Decimal.Round(Convert.ToDecimal(west.实际热指标), 2)
            };
            var accu_heatUnit = new HeatConsumptionTotalItem()
            {
                Title = "供暖季累计单耗(GJ/㎡)",
                全网 = Decimal.Round(Convert.ToDecimal(total.供暖季累计单耗), 4),
                东部 = Decimal.Round( Convert.ToDecimal(east.供暖季累计单耗),4),
                西部 = Decimal.Round( Convert.ToDecimal(west.供暖季累计单耗),4)
            };
            var expected_heatUnit = new HeatConsumptionTotalItem()
            {
                Title = "供暖季预期单耗(GJ/㎡)",
                全网 = Decimal.Round( Convert.ToDecimal(total.预估采暖季单耗), 4),
                东部 = Decimal.Round( Convert.ToDecimal(east.预估采暖季单耗), 4),
                西部 = Decimal.Round(Convert.ToDecimal(west.预估采暖季单耗), 4)
            };
            return new List<HeatConsumptionTotalItem>() { area, plan_early, actual, todayPredict, seasonal, water, calculated_heatindex, planned_heatindex, actual_heatindex, accu_heatUnit, expected_heatUnit };
        }
        
        public IList<HeatConsumptionTotalItem> GetYesterdaysGJByCompany(int companyId)
        {
            var yesterday = DateTime.Today.AddDays(-1);
            var weatherService = new WeatherService();
            var company = db.Companies.Single(i => i.ItemID == companyId);
            var manualCount = company.Stations.Where(i => string.IsNullOrEmpty(i.数据来源) && (i.生产热源ID.HasValue && (i.生产热源ID == 1 || i.生产热源ID == 22) ) ).Count();

            var count = new HeatConsumptionTotalItem()
            {
                Title = "热力站数量",
                监测站智能卡 = company.有效监控站数,
                总 = manualCount + company.有效监控站数,
                人工抄表 = manualCount,
                IsToday = false
            };
            
            var areaTotal = Decimal.Round( Convert.ToDecimal( company.截止昨日实际Area / 10000.0m) , 2, MidpointRounding.ToEven);
            var areaJK = Convert.ToDecimal(company.Area_JK_YD/10000.0m);
            var area = new HeatConsumptionTotalItem()
            {
                Title = "实际投入面积(万㎡)",
                总 = areaTotal,
                监测站智能卡 = areaJK,
                人工抄表 = areaTotal - areaJK, 
                IsToday = false
            };

            var plan = new HeatConsumptionTotalItem()
            {
                Title = "计划供热量(GJ)",
                总= Convert.ToDecimal(company.昨日计划GJ),
                监测站智能卡 = Convert.ToDecimal(company.昨日计划GJ_JK),
                人工抄表 = Convert.ToDecimal(company.昨日计划GJ) - Convert.ToDecimal(company.昨日计划GJ_JK),
                IsToday = false
            };
            var calculated = new HeatConsumptionTotalItem()
            {
                Title = "核算供热量(GJ)",
                总 = Convert.ToDecimal(company.昨日核算GJ),
                监测站智能卡 = Convert.ToDecimal(company.昨日核算GJ_JK),
                人工抄表 = Convert.ToDecimal(company.昨日核算GJ) - Convert.ToDecimal(company.昨日核算GJ_JK),
                IsToday = false
            };
            var actual = new HeatConsumptionTotalItem()
            {
                Title = "实际供热量(GJ)",
                监测站智能卡 = Convert.ToDecimal(company.昨日累计GJ_JK),
                人工抄表 = null,
                IsToday = false
            };
            var planned_heatindex = new HeatConsumptionTotalItem()
            {
                Title = "计划热指标(kcal/h•㎡)",
                总  = Convert.ToDecimal( company.昨日计划热指标 ),
                监测站智能卡 = Convert.ToDecimal( company.昨日计划热指标),
                人工抄表 = Convert.ToDecimal(company.昨日计划热指标),
                IsToday = false
            };
            var actual_heatindex = new HeatConsumptionTotalItem()
            {
                Title = "实际运行热指标(kcal/h•㎡)",
                监测站智能卡 = Convert.ToDecimal(company.昨日实际热指标),
                总 = null,
                人工抄表 = null,
                IsToday = false
            };
            var calculated_heatindex = new HeatConsumptionTotalItem()
            {
                Title = "核算热指标(kcal/h•㎡)",
                监测站智能卡 = Convert.ToDecimal(company.昨日核算热指标),
                总 = Convert.ToDecimal(company.昨日核算热指标),
                人工抄表 = Convert.ToDecimal(company.昨日核算热指标),
                IsToday = false
            };
            var exceed_Plan = new HeatConsumptionTotalItem()
            {
                Title = "实际供热量与计划供热量差值(GJ)",
                总 = Decimal.Round( Convert.ToDecimal(company.昨日累计GJ) - Convert.ToDecimal(company.昨日计划GJ), 0),
                监测站智能卡 = Decimal.Round(Convert.ToDecimal(company.昨日累计GJ_JK) - Convert.ToDecimal(company.昨日计划GJ_JK), 0),
                人工抄表 = null,
                IsToday = false
            };
            var exceed_Calculated = new HeatConsumptionTotalItem()
            {
                Title = "实际供热量与核算供热量差值(GJ)",
                总 = Decimal.Round(Convert.ToDecimal(company.昨日累计GJ) - Convert.ToDecimal(company.昨日核算GJ), 0),
                监测站智能卡 = Decimal.Round(Convert.ToDecimal(company.昨日累计GJ_JK) - Convert.ToDecimal(company.昨日核算GJ_JK), 0),
                人工抄表 = null,
                IsToday = false
            };
            return new List<HeatConsumptionTotalItem>() { count, area, plan, calculated, actual, planned_heatindex, calculated_heatindex, actual_heatindex, exceed_Plan, exceed_Calculated };
            
        }
        public IList<HeatConsumptionTotalItem> GetYesterdaysGJByManagership(int managershipId)
        {
            var yesterday = DateTime.Today.AddDays(-1);
            var history = db.SubCompanyHistoeys.FirstOrDefault(i => i.中心ID == managershipId && i.日期 == yesterday);
            var company = db.Managerships.Single(i => i.ItemID == managershipId);
            var manualCount = company.Stations.Where(i => string.IsNullOrEmpty(i.数据来源) && (i.生产热源ID.HasValue && (i.生产热源ID == 1 || i.生产热源ID == 22)) ).Count();
            var count = new HeatConsumptionTotalItem()
            {
                Title = "热力站数量",
                监测站智能卡 = company.有效监控站数,
                总 = manualCount + company.有效监控站数,
                人工抄表 = manualCount,
                IsToday = false
            };

            var areaTotal = Decimal.Round(Convert.ToDecimal(company.截止昨日实际Area / 10000.0m), 2, MidpointRounding.ToEven);
            var areaJK = Convert.ToDecimal(company.Area_JK_YD / 10000.0m);
            var area = new HeatConsumptionTotalItem()
            {
                Title = "实际投入面积(万㎡)",
                总 = areaTotal,
                监测站智能卡 = areaJK,
                人工抄表 = areaTotal - areaJK,
                IsToday = false
            };

            var plan = new HeatConsumptionTotalItem()
            {
                Title = "计划供热量(GJ)",
                总 = Convert.ToDecimal(company.昨日计划GJ),
                监测站智能卡 = Convert.ToDecimal(company.昨日计划GJ_JK),
                人工抄表 = Convert.ToDecimal(company.昨日计划GJ) - Convert.ToDecimal(company.昨日计划GJ_JK),
                IsToday = false
            };
            var calculated = new HeatConsumptionTotalItem()
            {
                Title = "核算供热量(GJ)",
                总 = Decimal.Round( Convert.ToDecimal( company.昨日核算GJ ), 0),
                监测站智能卡 = Convert.ToDecimal(company.昨日核算GJ_JK),
                人工抄表 = Convert.ToDecimal(company.昨日核算GJ) - Convert.ToDecimal(company.昨日核算GJ_JK),
                IsToday = false
            };
            var actual = new HeatConsumptionTotalItem()
            {
                Title = "实际供热量(GJ)",
                监测站智能卡 = Convert.ToDecimal(company.昨日累计GJ_JK),
                人工抄表 = null,
                IsToday = false
            };
            
            var planned_heatindex = new HeatConsumptionTotalItem()
            {
                Title = "计划热指标(kcal/h•㎡)",
                总 = Convert.ToDecimal(company.昨日计划热指标),
                监测站智能卡 = Convert.ToDecimal(company.昨日计划热指标),
                人工抄表 = Convert.ToDecimal(company.昨日计划热指标),
                IsToday = false
            };
            var calculated_heatindex = new HeatConsumptionTotalItem()
            {
                Title = "核算热指标(kcal/h•㎡)",
                监测站智能卡 = Convert.ToDecimal(company.昨日核算热指标),
                总 = Convert.ToDecimal(company.昨日核算热指标),
                人工抄表 = Convert.ToDecimal(company.昨日核算热指标),
                IsToday = false
            };
            var actual_heatindex = new HeatConsumptionTotalItem()
            {
                Title = "实际运行热指标(kcal/h•㎡)",
                监测站智能卡 = Decimal.Round(Convert.ToDecimal(company.昨日实际热指标), 2),
                总 = null,
                人工抄表 = null,
                IsToday = false
            };
            var exceed_Plan = new HeatConsumptionTotalItem()
            {
                Title = "实际供热量与计划供热量差值(GJ)",
                总 = Decimal.Round( Convert.ToDecimal( company.昨日累计GJ )- Convert.ToDecimal( company.昨日计划GJ ), 2),
                监测站智能卡 = Decimal.Round(Convert.ToDecimal(company.昨日累计GJ_JK) - Convert.ToDecimal(company.昨日计划GJ_JK), 2),
                人工抄表 = null,
                IsToday = false
            };
            var exceed_Calculated = new HeatConsumptionTotalItem()
            {
                Title = "实际供热量与核算供热量差值(GJ)",
                总 = Decimal.Round(Convert.ToDecimal(company.昨日累计GJ) - Convert.ToDecimal(company.昨日核算GJ), 2),
                监测站智能卡 = Decimal.Round(Convert.ToDecimal(company.昨日累计GJ_JK) - Convert.ToDecimal(company.昨日核算GJ_JK), 2),
                人工抄表 = null,
                IsToday = false
            };
            return new List<HeatConsumptionTotalItem>() { count, area, plan, calculated, actual, planned_heatindex, calculated_heatindex, actual_heatindex, exceed_Plan, exceed_Calculated };
        }

        public IList<HeatConsumptionTotalItem> GetYesterdaysGJ()
        {
            var total = db.TotalNetRecents.Single(i => i.ItemID == 3);
            var east = db.TotalNetRecents.Single(i => i.ItemID == 1);
            var west = db.TotalNetRecents.Single(i => i.ItemID == 2);
            var area = new HeatConsumptionTotalItem()
            {
                Title = "实际供热总面积(万㎡)",
                全网 = Decimal.Round( Convert.ToDecimal(total.截止昨日实际Area) / 10000.0m, 2),
                东部 = Decimal.Round( Convert.ToDecimal( east.截止昨日实际Area) / 10000.0m, 2),
                西部 = Decimal.Round( Convert.ToDecimal(west.截止昨日实际Area) / 10000.0m, 2),
                IsToday = false
            };
            var plan_early = 
                new HeatConsumptionTotalItem()
                {
                    Title = "计划供热量(GJ)",
                    全网 = Decimal.Round( Convert.ToDecimal(total.昨日计划GJ), 0),
                    东部 = Decimal.Round( Convert.ToDecimal(east.昨日计划GJ), 0),
                    西部 = Decimal.Round(Convert.ToDecimal(west.昨日计划GJ), 0),
                    IsToday = false
                };
            var plan_end =
                new HeatConsumptionTotalItem()
                {
                    Title = "核算供热量(GJ)",
                    全网 = Decimal.Round( Convert.ToDecimal(total.昨日核算GJ), 0),
                    东部 = Decimal.Round( Convert.ToDecimal(east.昨日核算GJ), 0),
                    西部 = Decimal.Round(Convert.ToDecimal(west.昨日核算GJ), 0),
                    IsToday = false
                };
            var actual =
                new HeatConsumptionTotalItem()
                {
                    Title = "实际供热量(GJ)",
                    全网 = Decimal.Round( Convert.ToDecimal(total.昨日累计GJ), 0),
                    东部 = Decimal.Round( Convert.ToDecimal(east.昨日累计GJ), 0),
                    西部 = Decimal.Round(Convert.ToDecimal(west.昨日累计GJ), 0),
                    IsToday = false
                };
            var calculated_heatindex = new HeatConsumptionTotalItem()
            {
                Title = "核算热指标(kcal/h•㎡)",
                全网 = Decimal.Round( Convert.ToDecimal(total.昨日核算热指标),2),
                东部 = Decimal.Round(Convert.ToDecimal(east.昨日核算热指标),2),
                西部 = Decimal.Round(Convert.ToDecimal(west.昨日核算热指标), 2)
            };
            var planned_heatindex = new HeatConsumptionTotalItem()
            {
                Title = "计划热指标(kcal/h•㎡)",
                全网 = Decimal.Round(Convert.ToDecimal(total.昨日计划热指标),2),
                东部 = Decimal.Round(Convert.ToDecimal(east.昨日计划热指标),2),
                西部 = Decimal.Round(Convert.ToDecimal(west.昨日计划热指标), 2)
            };
            var actual_heatindex = new HeatConsumptionTotalItem()
            {
                Title = "实际运行热指标(kcal/h•㎡)",
                全网 = Decimal.Round( Convert.ToDecimal(total.昨日实际热指标),2),
                东部 = Decimal.Round( Convert.ToDecimal(east.昨日实际热指标),2),
                西部 = Decimal.Round(Convert.ToDecimal(west.昨日实际热指标), 2)
            };
            var exceedPlan = new HeatConsumptionTotalItem()
            {
                Title = "实际供热量与计划供热量差值(GJ)",
                全网 = Decimal.Round( Convert.ToDecimal(total.昨日累计GJ - total.昨日计划GJ), 0),
                东部 = Decimal.Round( Convert.ToDecimal(east.昨日累计GJ - east.昨日计划GJ), 0),
                西部 = Decimal.Round( Convert.ToDecimal(west.昨日累计GJ - west.昨日计划GJ), 0)
            };
            var exceedCalculated = new HeatConsumptionTotalItem()
            {
                Title = "实际供热量与核算供热量差值(GJ)",
                全网 = Decimal.Round(Convert.ToDecimal(total.昨日累计GJ - total.昨日核算GJ), 0),
                东部 = Decimal.Round(Convert.ToDecimal(east.昨日累计GJ - east.昨日核算GJ), 0),
                西部 = Decimal.Round(Convert.ToDecimal(west.昨日累计GJ - west.昨日核算GJ), 0)
            };

            return new List<HeatConsumptionTotalItem>() { area, plan_early, plan_end, actual, planned_heatindex, calculated_heatindex, actual_heatindex, exceedPlan, exceedCalculated };
        }


        public IList<HeatConsumptionTotalItem> GetManualGJByCompanyId(int companyId)
        {
            var company = db.Companies.Single(i => i.ItemID == companyId);

            return new List<HeatConsumptionTotalItem>() { };
        }

        public IList<HeatConsumptionTotalItem> GetManualGJByManagershipId(int managershipId)
        {
            var managership = db.Managerships.Single(i => i.ItemID == managershipId);
           
            return new List<HeatConsumptionTotalItem>() { };
        }

        public int GetTotalStationCount()
        {
            return db.Stations.Count();
        }
        public int Get温度超标站个数()
        {
            return db.Stations.AsEnumerable().Count(i => i.一次回温 > ConfigurationService.Instance.TemperatureExceed);
        }
        public int Get超标站个数()
        {
            return db.Stations.AsEnumerable().Count(i => 
                                        i.一次回温 > ConfigurationService.Instance.TemperatureExceed );
        }
    }
}