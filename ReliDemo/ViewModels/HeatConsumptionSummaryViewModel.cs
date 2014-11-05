using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;

namespace ReliDemo.ViewModels
{

    public class HeatConsumptionSummaryViewModel
    {
        public IEnumerable<HeatConsumptionArea> TodaysHeatConsumptionSummary { get; set; }
        
        public HeatConsumptionTotalItem TodayHeatConsumptionAccu { get; set; }
        public IList<HeatConsumptionTotalItem> TodayGJ { get; set; }
        public IList<HeatConsumptionTotalItem> YesterdayGJ { get; set; }
        
        public GJHistoriesViewModel Histories { get; set; }
        public EnergySavingTopChartViewModel EnergyTopChart { get; set; }
        public CompanyAreaOnOffViewModel CompanyAreaOnOff { get; set; }
        public IEnumerable<Company> Companies { get; set; }
        public TopStatsViewModel TopStats { get; set; }
        public int 监控站个数 { get; set; }
        public int 智能卡站个数 { get; set; }
        public int 手抄表站个数 { get; set; }
        public int 有效站个数 { get; set; }
        public DateTime? 手抄表最近更新时间 { get; set; }
        public ExceedGraphViewModel ExceedGraph { get; set; }
        public IEnumerable<ITemperatureGraphItem> WeatherGraph { get; set; }
    }

    public class CompanyAreaOnOffViewModel {
        public string CompanyAreaTotalData { get; set; }
        public string CompanyAreaOnData { get; set; }
    }

    public class CompanyViewModel : HeatConsumptionSummaryViewModel
    {
        public Company Company { get; set; }
        public IEnumerable<Managership> Centers { get; set; }
        public IList<HeatConsumptionTotalItem> 手抄GJ { get; set; }
        public IList<HeatConsumptionTotalItem> GJ总 { get; set; }

    }

    public class ManagershipViewModel : HeatConsumptionSummaryViewModel
    {
        public Managership Managership { get; set; }
        public IList<HeatConsumptionTotalItem> 手抄GJ { get; set; }
        public IList<HeatConsumptionTotalItem> GJ总 { get; set; }

    }

    public class ExceedGraphViewModel
    {
        public int 超标站个数 { get; set; }
        public int 热力站总数 { get; set; }
        public int 单耗比计划超标站个数 {get;set;}
        public int 单耗比核算超标站个数 { get; set; }
        public int 流量超标站个数 { get; set; }
        public int 温度超标站个数 { get; set; }
    }
    
    public class GJHistoriesViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string 计划Data { get; set; }
        public string 核算Data { get; set; }
        public string 实际Data { get; set; }
        public string 计划热指标 { get; set; }
        public string 核算热指标 { get; set; }
        public string 实际运行热指标 { get; set; }
        public string 预报温度 { get; set; }
        public string 实际温度 { get; set; }
        public HeatConsumptionGraphType 类型 { get; set; }
        public HeatConsumptionGraphSpan 时间 { get; set; }
        public int CompanyId { get; set; }
        public int ManagershipId { get; set; }
        public int StationId { get; set; }
    }

    public class EnergySavingTopChartViewModel
    {
        public IEnumerable<Station> TopEnergySaving { get; set; }
        public IEnumerable<Station> TopEnergyConsuming { get; set; }
        public IEnumerable<Station> TopWaterSaving { get; set; }
        public IEnumerable<Station> TopWaterConsuming { get; set; }
        public int EnergySavingCount { get; set; }
        public int EnergyConsumingCount { get; set; }
        public int WaterSavingCount { get; set; }
        public int WaterConsumingCount { get; set; }
        public int EnergyOrderBy { get; set; }
        public int WaterOrderBy { get; set; }
    }

    public class TopStatsViewModel
    {
        public decimal TodaysHighestTemperature { get; set; }
        public decimal TodaysLowestTemperature { get; set; }
        public decimal TodaysAverageTemperature { get; set; }
        public string TodaysWeather { get; set; }
        public int TotalHeatSupplyDays { get; set; }
        public string TodaysWind { get; set; }
        public decimal YesterdayAverateTemperature { get; set; }
        public WeatherTypes WeatherType { get; set; }
        public DateTime WeatherPublishedAt { get; set; }
        public int 今日累计供热量 { get; set; }
        public int 昨日累计供热量 { get; set; }
        public HeatConsumptionArea 投停面积信息 { get; set; }

        public DateTime 起始供热时间 { get; set; }
        public User 当前用户 { get; set; }
        public DateTime 当前时间 { get; set; }
        public int? CompanyId { get; set; }
        public int? ManagershipId { get; set; }
    }

}