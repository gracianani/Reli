using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ReliDemo.ViewModels;
using ReliDemo.Core.Interfaces;

namespace ReliDemo.Controllers
{
    [Authorize]
    public class CostController : Controller
    {
        private readonly IHeatSourceRepository _heatSourceRepo;

        public CostController(IHeatSourceRepository heatSourceRepo)
        {
            _heatSourceRepo = heatSourceRepo;
        }

        public ActionResult Index()
        {
            var recents = _heatSourceRepo.GetAllHeatSourceRecents();
            return View( new CostViewModel() {
                TodayGJ = recents.Sum(i=>i.今日GJ),
                TodayCoalGJ = recents.Where(i=>i.HeatSource.机组类型 == "燃煤").Sum(i=>i.今日GJ),
                TodayGasGJ = recents.Where(i => i.HeatSource.机组类型 == "燃气").Sum(i => i.今日GJ),
                YesterdayGJ = recents.Sum(i => i.昨日GJ),
                YesterdayCoalGJ = recents.Where(i => i.HeatSource.机组类型 == "燃煤").Sum(i => i.昨日GJ),
                YesterdayGasGJ = recents.Where(i => i.HeatSource.机组类型 == "燃气").Sum(i => i.昨日GJ),
                SeasonGJ = recents.Sum(i => i.供暖季GJ) ?? 0,
                SeasonCoalGJ = recents.Where(i => i.HeatSource.机组类型 == "燃煤").Sum(i => i.供暖季GJ) ?? 0,
                SeasonGasGJ = recents.Where(i => i.HeatSource.机组类型 == "燃气").Sum(i => i.供暖季GJ) ?? 0,
                TodayPrice = decimal.Round( recents.Average(i=>i.今日热价) ?? 0.0m, 2, MidpointRounding.AwayFromZero), 
                TodayCoalPrice = decimal.Round(  recents.Where(i=>i.HeatSource.机组类型 == "燃煤").Average(i=>i.今日热价) ?? 0.0m, 2, MidpointRounding.AwayFromZero),
                TodayGasPrice = decimal.Round(  recents.Where(i=>i.HeatSource.机组类型 == "燃气").Average(i=>i.今日热价) ?? 0.0m, 2, MidpointRounding.AwayFromZero),
                YesterdayPrice = decimal.Round(  recents.Average(i => i.昨日热价) ?? 0.0m, 2, MidpointRounding.AwayFromZero),
                YesterdayCoalPrice = decimal.Round(  recents.Where(i => i.HeatSource.机组类型 == "燃煤").Average(i => i.昨日热价) ?? 0.0m, 2, MidpointRounding.AwayFromZero),
                YesterdayGasPrice = decimal.Round(  recents.Where(i => i.HeatSource.机组类型 == "燃气").Average(i => i.昨日热价) ?? 0.0m, 2, MidpointRounding.AwayFromZero),
                SeasonPrice =  decimal.Round( recents.Average(i => i.供暖季热价) ?? 0.0m, 2, MidpointRounding.AwayFromZero),
                SeasonCoalPrice = decimal.Round(  recents.Where(i => i.HeatSource.机组类型 == "燃煤").Average(i => i.供暖季热价) ?? 0.0m, 2, MidpointRounding.AwayFromZero),
                SeasonGasPrice = decimal.Round(recents.Where(i => i.HeatSource.机组类型 == "燃气").Average(i => i.供暖季热价) ?? 0.0m, 2, MidpointRounding.AwayFromZero),
                HeatSourceRecents = recents
            } );
        }
    }
}
