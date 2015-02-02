using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReliMobileAdmin.Models;

namespace ReliMobileAdmin.Controllers
{
    [Authorize]
    public class WarningController : Controller
    {
        ReliMobileEntities context;

        public WarningController()
        {
            context = new ReliMobileEntities();
        }

        public ActionResult Index()
        {
            var warnings = context.warnings.Select(i => new Warning() { DBWarning = i });

            return View(warnings);
        }

        public ActionResult Create()
        {
            return View(new CreateWarningViewModel());
        }

        [HttpPost]
        public ActionResult Create(CreateWarningViewModel warningMV)
        {
            if (ModelState.IsValid)
            {
                var warning = new warning() { warningMessage = warningMV.WarningContent, warningTitle = warningMV.WarningTitle, reportedAt = DateTime.Now };
                context.warnings.AddObject(warning);
                context.SaveChanges();
                warningMV.IsCreated = true;
            }
            return View(warningMV);
        }

        [HttpDelete]
        public ActionResult Delete(int warningId)
        {
            var warning = context.warnings.Single(i => i.warningId == warningId);
            context.warnings.DeleteObject(warning);
            context.SaveChanges();
            return new JsonResult() { Data = "{ \" isDeleted \" : true } " };
        }
    }
}
