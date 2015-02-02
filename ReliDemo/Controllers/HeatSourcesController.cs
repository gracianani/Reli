using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ReliDemo.Core.Interfaces;
using ReliDemo.ViewModels;
using ReliDemo.Infrastructure.Services;
using ReliDemo.Models;

namespace ReliDemo.Controllers
{
   // [Authorize]
    public class HeatSourcesController : Controller
    {
        private readonly IStationRepository _stationRepo;
        private readonly ICompanyRepository _companyRepo;
        private readonly IHeatSourceRepository _heatSourceRepo;
        private readonly IManagershipRepository _managershipRepo;
        private HeatSourcesService _heatSourcesService;
        
        public HeatSourcesController(IStationRepository stationRepo, ICompanyRepository companyRepo, IHeatSourceRepository heatSourceRepo, IManagershipRepository managershipRepo)
        {
            _stationRepo = stationRepo;
            _companyRepo = companyRepo;
            _heatSourceRepo = heatSourceRepo;
            _managershipRepo = managershipRepo;
            _heatSourcesService = new HeatSourcesService();
        }
        
        public ActionResult Index()
        {
            var heatSources = _heatSourceRepo.GetAllHeatSources();
            heatSources = heatSources.Where(j => j.是否并网供热 == true );
            return View( new HeatSourcesViewModel ( heatSources ) );
        }

        public ActionResult BasicInfo(int heatSourceId=0)
        {
            var heatSource = _heatSourceRepo.GetAllHeatSources().Single(i=>i.ItemID == heatSourceId);
            return View(new HeatSourceViewModel() { HeatSource = heatSource, HeatSourceId = heatSourceId } );
        }

        public ActionResult RealTime(int startIndex = 0, int pageSize = 10,int selectedRegion=1)
        {
            var total = _heatSourcesService.GetHeatSourcesRealTimeByRegion((HeatSourceRegions)selectedRegion);
            var heatSourceRecents = total.Skip(startIndex*pageSize).Take(pageSize).OrderByDescending(i=>i.采集时间);
            var heatSourceRealTimeViewModel = new HeatSourceRealTimeViewModel() {
                HeatSourceRecents = heatSourceRecents,
                SelectedRegion = selectedRegion,
                UpdatedAt = heatSourceRecents.Count() > 0 ? heatSourceRecents.First().采集时间 : null,
                StartIndex = startIndex,
                PageSize = pageSize,
                TotalItemCount = total.Count()
            };
            return View(heatSourceRealTimeViewModel);
        }
        
        public ActionResult History(int heatSourceId = 0,string daterange="")
        {
            var fromDate = DateTime.Now.AddDays(-7);
            var toDate = DateTime.Now;
            if (!string.IsNullOrEmpty(daterange))
            {
                var fromdate_todate = daterange.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                fromDate = DateTime.Parse(fromdate_todate[0]);
                toDate = DateTime.Parse(fromdate_todate[1]);
            }
            var heatSourceHistories = _heatSourcesService.GetHeatSourceHistory(heatSourceId,fromDate, toDate);
            var heatSource = _heatSourceRepo.GetAllHeatSources().Single(i => i.ItemID == heatSourceId);
            return View(new HeatSourceHistoryViewModel() { HeatSourceId = heatSourceId, Histories = heatSourceHistories, HeatSourceName = heatSource.热源名称 , FromDate = fromDate, ToDate = toDate});
        }

        public ActionResult GetNewDataCount(DateTime fromTime)
        {
            var newData = _heatSourcesService.GetHeatSourcesRealTime(fromTime, DateTime.Now);
            return Json(new { newDataCount = newData.Count() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddToTotalHeatSource(int heatSourceId = 11, bool isAddToTotal = true)
        {
            var heatSource = _heatSourceRepo.GetAllHeatSources().Single(i => i.ItemID == heatSourceId);
            heatSource.是否并网供热 = isAddToTotal;
            _heatSourceRepo.Update(heatSource);
            return RedirectToAction("BasicInfo", new { heatSourceId = heatSourceId });
        }

        public ActionResult ChangeHeatUnitPrice(FormCollection collection)
        {
            var heatSourceId = Convert.ToInt32(collection["heatSourceId"]);
            var prices = collection["price"].Split(',');
            var groupNumbers = collection["groupNumber"].Split(',');
            var heatSource = _heatSourceRepo.GetAllHeatSources().Single(i => i.ItemID == heatSourceId);
            for (int i=0;i< prices.Length;i++)
            {
                var price = Convert.ToDecimal(prices[i]);
                var groupNumber = Convert.ToInt32(groupNumbers[i]);
                heatSource.HeatSourceRecents.Single(j => j.机组号 == groupNumber).单价 = price;
            }
            _heatSourceRepo.Update(heatSource);
            return RedirectToAction("BasicInfo", new { heatSourceId = heatSourceId });
        }

        public ActionResult ChangeHeatSourceCapacity(FormCollection collection)
        {
            var heatSourceId = Convert.ToInt32(collection["heatSourceId"]);
            var heatSource = _heatSourceRepo.GetAllHeatSources().Single(i => i.ItemID == heatSourceId);
            if (heatSource.HeatsourceCapacity.Count == 0)
            {
                var heatsourceCapacity = new heatsourceCapacity() { 供热能力GJ = Convert.ToInt32(collection["gongrenengli"]), 已出力GJ = Convert.ToInt32(collection["yichuli"]) };
                heatSource.HeatsourceCapacity.Add(heatsourceCapacity);
            }
            else
            {
                heatSource.HeatsourceCapacity.First().供热能力GJ = Convert.ToInt32(collection["gongrenengli"]);
                heatSource.HeatsourceCapacity.First().已出力GJ = Convert.ToInt32(collection["yichuli"]);
            }
            _heatSourceRepo.Update(heatSource);
            return RedirectToAction("BasicInfo", new { heatSourceId = heatSourceId });
        }
    }
}
