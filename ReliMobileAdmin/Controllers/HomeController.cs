using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.Mvc;
using ReliMobileAdmin.Models;
using System.IO;
using System.Configuration;
using ReliMobileAdmin.Helper;

namespace ReliMobileAdmin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        ReliMobileEntities context;
        private string DailyReportsRoot
        {
            get
            {
                return ConfigurationManager.AppSettings["DailyReportsRoot"];
            }
        }
        private string CustomerReportsRoot
        {
            get
            {
                return ConfigurationManager.AppSettings["CustomerReportsRoot"];
            }
        }
        public HomeController()
        {
            context = new ReliMobileEntities();
        }

        public ActionResult Index()
        {
            var latestCustomerReport = new DirectoryInfo(CustomerReportsRoot)
                    .GetFiles("*", SearchOption.TopDirectoryOnly)
                    .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden))
                    .Select((f, index) => new ReportItem() { CreatedAt = f.CreationTime, FileName = f.Name, Extension = f.Extension, Path = f.FullName })
                    .OrderByDescending(i => i.CreatedAt)
                    .FirstOrDefault();
            var latestProductionReport = new DirectoryInfo(DailyReportsRoot)
                    .GetFiles("*", SearchOption.TopDirectoryOnly)
                    .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden))
                    .Select((f, index) => new ReportItem() { CreatedAt = f.CreationTime, FileName = f.Name, Extension = f.Extension, Path = f.FullName })
                    .OrderByDescending(i => i.CreatedAt)
                    .FirstOrDefault();
            return View(new HomeViewModel() { 
                NumOfNewMessage = context.messages.Count(), 
                NumOfWarningMessages = context.warnings.Count(),
                LatestCustomerReport = latestCustomerReport.CreatedAt,
                LatestDailyReport = latestProductionReport.CreatedAt,
                Users = context.users.Select(i => new User() { UserId = i.userId, UserName = i.姓名 })  
            });
        }


        [HttpPost]
        public PartialViewResult Create(CreateMessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedUserId > 0)
                {
                    var message = new message()
                    {
                        sendFromUserId = UserHelper.AdminId,
                        sendToUserId = model.SelectedUserId,
                        messageContent = model.Message,
                        createdAt = DateTime.Now
                    };
                    context.AddObject("messages", message);
                    context.SaveChanges();
                    model.IsCreated = true;
                    model.Message = "";
                }
            }
            model.Users = context.users.Select(i => new User() { UserId = i.userId, UserName = i.姓名 });
            return PartialView("", model);
        }
    }
}
