using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReliDemo.Infrastructure.Services;
using ReliDemo.ViewModels;

namespace ReliDemo.Controllers
{
    public class ConfigurationController : Controller
    {
        //
        // GET: /Configuration/

        public ActionResult Index()
        {
            return View(new ConfigurationViewModel()
            {
                操作记录 = new HeatConsumptionSummaryService().GetTemperatureAudit()
            });
        }

        public ActionResult ChangeTemperature(decimal temperature)
        {
            ConfigurationService.Instance.TemperatureExceed = temperature;
            new HeatConsumptionSummaryService().InsertTemperatureAudit(temperature, MembershipService.CurrentUser.UserId);
            return RedirectToAction("Index", 
                new ConfigurationViewModel() { 
                    操作记录 = new HeatConsumptionSummaryService().GetTemperatureAudit() 
                } );
        }

        public ActionResult ChangeReportRange(DateTime dateFrom, DateTime dateTo)
        {
            ConfigurationService.Instance.报表默认开始时间 = dateFrom;
            ConfigurationService.Instance.报表默认开始时间 = dateFrom;
            ConfigurationService.Instance.Save报表日期();
            return RedirectToAction("Index",
                new ConfigurationViewModel()
                {
                    操作记录 = new HeatConsumptionSummaryService().GetTemperatureAudit()
                });
        }

    }
}
