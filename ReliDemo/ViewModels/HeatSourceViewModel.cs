using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;

namespace ReliDemo.ViewModels
{
    public class CostViewModel
    {
        public int TodayGJ { get; set; }
        public int TodayCoalGJ { get; set; }
        public int TodayGasGJ { get; set; }
        public int YesterdayGJ { get; set; }
        public int YesterdayCoalGJ { get; set; }
        public int YesterdayGasGJ { get; set; }
        public int SeasonGJ { get; set; }
        public int SeasonCoalGJ { get; set; }
        public int SeasonGasGJ { get; set; }
        public decimal TodayPrice { get; set; }
        public decimal TodayCoalPrice { get; set; }
        public decimal TodayGasPrice { get; set; }
        public decimal YesterdayPrice { get; set; }
        public decimal YesterdayCoalPrice { get; set; }
        public decimal YesterdayGasPrice { get; set; }
        public decimal SeasonPrice { get; set; }
        public decimal SeasonCoalPrice { get; set; }
        public decimal SeasonGasPrice { get; set; }
        public IEnumerable<HeatSourceRecent> HeatSourceRecents { get; set; }
    }

    public class HeatSourcesViewModel
    {
        public IEnumerable<HeatSource> HeatSources { get; set; }
        public decimal TotalFuelInSquareMeter { get; set; }
        public decimal EastFuelInSquareMeter { get; set; }
        public decimal WestFuelInSquareMeter { get; set; }
        public decimal FuelInFossil { get; set; }
        public decimal FuelInGas { get; set; }
        public decimal HeatSource { get; set; }
        public decimal PeakHeatSource { get; set; }
        public string HeatLoadsData { get; set; }
        public decimal TotalHeatLoad { get; set; }

        public HeatSourcesViewModel(IEnumerable<HeatSource> heatSources)
        {
            var db = new xz2013Entities();
            var total = db.TotalNetRecents.Single(i => i.ItemID == 3);
            var east = db.TotalNetRecents.Single(i => i.ItemID == 1);
            var west = db.TotalNetRecents.Single(i => i.ItemID == 2);
            var heatSourcesFull =   from heatSource in heatSources
                                    select new {
                                        Location = heatSource.东西部,
                                        Type = heatSource.机组类型,
                                        PeatHeatSource = heatSource.燃气尖峰炉数 + heatSource.燃煤尖峰炉数,
                                        HeatLoad = heatSource.Stations.Sum(i=>i.供暖季GJ),
                                        HeatSourceName = heatSource.热源名称
                                    };
            HeatSources = heatSources;
            TotalFuelInSquareMeter = Convert.ToDecimal(total.实际供热面积);
            EastFuelInSquareMeter = Convert.ToDecimal(east.实际供热面积);
            WestFuelInSquareMeter = Convert.ToDecimal(west.实际供热面积);
            FuelInFossil = 39.0m;
            FuelInGas = 61.0m;
            HeatLoadsData = "[" + string.Join(",", heatSourcesFull.Where(i=>i.HeatLoad>0).Select(i => string.Format("{{ \"label\" : \"{0}\", \"data\" : {1} }}", i.HeatSourceName,  i.HeatLoad)).ToList()) + "]";
            TotalHeatLoad = Convert.ToDecimal( total.供暖季累计GJ );
        }
    }

    public class HeatSourceViewModel
    {
        public int HeatSourceId {get;set;}
        public HeatSource HeatSource { get; set; }
    }

    public class HeatSourceHistoryViewModel
    {
        public int HeatSourceId { get; set; }
        public string HeatSourceName { get; set; }
        public IEnumerable<HeatSourceAccuHistory> Histories { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class HeatSourceRealTimeViewModel
    {
        public IEnumerable<ReliDemo.Models.HeatSourceRecent> HeatSourceRecents { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int SelectedRegion { get; set; }
        public int StartIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalItemCount { get; set; }
    }
}