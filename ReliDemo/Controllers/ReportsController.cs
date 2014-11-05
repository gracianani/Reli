using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using OfficeOpenXml;
using System.IO;
using ReliDemo.Models;
using ReliDemo.Infrastructure.Services;
using ReliDemo.Core.Interfaces;
using System.Configuration;
using ReliDemo.ViewModels;
using ReliDemo.Models;
using ReliDemo.Infrastructure.Repositories;
            
namespace ReliDemo.Controllers
{
    [Authorize(Roles = "生产部调度,分公司调度,供热中心调度,系统管理员,系统维护员")]
    public class ReportsController : Controller
    {
        private readonly ICompanyRepository _companyRepo;
        public ReportsController(ICompanyRepository companyRepo)
        {
            _companyRepo = companyRepo;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Preview(int reportType, string date)
        {
            var stationService = new StationsService();
            return View(new ReportsViewModel() { ReportTypeId=reportType, Report = stationService.GetDailyReport(DateTime.Parse(date)), Day = DateTime.Parse(date) , Title = ((ReportType)reportType).ToString() });
        }

        public ActionResult PreviewTwo(int reportType, string date)
        {
            var report = ReportFactory.CreateReport((ReportType)reportType, DateTime.Parse(date)) as Report;

            return View(new StatViewModel() { ReportTypeId = reportType, Report = report.公司统计, Day = DateTime.Parse(date), Title = ((ReportType)reportType).ToString() });
        }

        public ActionResult PreviewThree(int reportType, string dateFrom, string DateTo)
        {
            var from = DateTime.Parse(dateFrom);
            var to = DateTime.Parse(DateTo);
            var report = ReportFactory.CreateRangeReport((ReportType)reportType, from,  to) as RangeReport;
            return View(new RangeStatViewModel() { ReportTypeId = reportType, Report = report.RangeStat, Temperature = report.Temperature, From = from, To = to, Title = ((ReportType)reportType).ToString() });
        }

        //一站一日一计划统计
        public ActionResult PreviewDaily(string dateFrom, string dateTo, int companyId = -1, int 是否重点站 = 2, string 热源 = "ALL", string 收费性质 = "ALL", string 数据来源 = "ALL")
        {
            var from = DateTime.Parse(dateFrom);
            var to = DateTime.Parse(dateTo);
            var currentUser = MembershipService.CurrentUser;

            DailyReport report;
            if (currentUser.是否有集团权限)
            {
                report = ReportFactory.CreateRangeReport(ReportType.一站一日一计划时间段报表, from, to, 是否重点站, 热源, 收费性质, 数据来源, companyId) as DailyReport;
            }
            else
            {
                return RedirectToAction("Index");
            }

            return View(new DailyReportViewModel()
            {
                ReportTypeId = (int)ReportType.一站一日一计划时间段报表,
                ReportData = report.ReportData,
                From = from,
                To = to,
                Title = (ReportType.一站一日一计划时间段报表).ToString(),
                收费性质 = 收费性质,
                是否重点站 = 是否重点站,
                数据来源 = 数据来源,
                热源 = 热源,
                SelectedCompanyId = companyId
            });
        }
        public ActionResult DownloadDaily(string dateFrom, string dateTo, int companyId = -1, int 是否重点站 = 2, string 热源 = "ALL", string 收费性质 = "ALL", string 数据来源 = "ALL")
        {
            var from = DateTime.Parse(dateFrom);
            var to = DateTime.Parse(dateTo);
            var currentUser = MembershipService.CurrentUser;

            DailyReport report;
            if (currentUser.是否有集团权限)
            {
                report = ReportFactory.CreateRangeReport(ReportType.一站一日一计划时间段报表, from, to, 是否重点站, 热源, 收费性质, 数据来源, companyId) as DailyReport;
            }
            else
            {
                return Json(new { fileName = "" }, JsonRequestBehavior.AllowGet);
            }

            var templateName = string.Format("{0}{1}.xlsx", ConfigurationManager.AppSettings["ReportTemplateDirectory"], report.TemplateName);
            FileInfo template = new FileInfo(templateName);
            string newFileName = "";
            newFileName = string.Format("{0}{1:yyyy-MM-dd}-{2:yyyy-MM-dd}.xlsx", report.TemplateName, dateFrom, dateTo);

            FileInfo newFile = new FileInfo(Server.MapPath("~/DownloadedReport/") + newFileName);
            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Ti3ned Goods"];
                report.FillReport(worksheet);
                xlPackage.Workbook.Properties.Title = report.TemplateName;
                xlPackage.Save();
            }

            return Json(new { fileName = "/DownloadedReport/" + newFileName }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PreviewCompletion(string dateFrom, string dateTo, int companyId = -1, int managershipId = -1, int 是否重点站 = 2, string 热源 = "ALL", string 收费性质 = "ALL", string 数据来源 = "ALL")
        {
            var from = DateTime.Parse(dateFrom);
            var to = DateTime.Parse(dateTo);
            var currentUser = MembershipService.CurrentUser;
            //根据权限查询

            //集团员工可以查询全部，分公司权限可以查询分公司，中心权限查询中心。
            CompletionReport report;
            if (currentUser.是否有集团权限)
            {
                report = ReportFactory.CreateRangeReport(ReportType.公司到位率统计表, from, to, 是否重点站, 热源, 收费性质, 数据来源, companyId, managershipId) as CompletionReport;
                if (companyId != -1)
                {
                    var m = (new ManagershipRepository()).SearchFor(i => i.公司ID == companyId).Select(i => i.管理单位).ToList();
                    report.ReportData = report.ReportData.Where(i => m.Exists(j => j == i.公司名) || i.公司名 == "合计").ToList();
                }
                
            }
            else if (currentUser.是否有分公司权限 && currentUser.VisibleCompanies.Count > 0)
            {
                if (currentUser.VisibleCompanies.Count(i => i.Id == companyId) > 0)
                {
                    report = ReportFactory.CreateRangeReport(ReportType.公司到位率统计表, from, to, 是否重点站, 热源, 收费性质, 数据来源, companyId, managershipId) as CompletionReport;
                }
                else
                {
                    companyId = currentUser.VisibleCompanies.First().Id;
                    report = ReportFactory.CreateRangeReport(ReportType.公司到位率统计表, from, to, 是否重点站, 热源, 收费性质, 数据来源, companyId, managershipId) as CompletionReport;
                }
                var m = (new ManagershipRepository()).SearchFor(i => i.公司ID == companyId).Select(i => i.管理单位).ToList();
                report.ReportData = report.ReportData.Where(i => m.Exists( j=>j == i.公司名) || i.公司名 == "合计").ToList();
            }
            else if (currentUser.是否有供热中心权限 && currentUser.VisibleManagerships.Count > 0)
            {

                companyId = currentUser.VisibleCompanies.First().Id;
                if (currentUser.VisibleManagerships.Count(i => i.Id == managershipId) > 0)
                {
                    report = ReportFactory.CreateRangeReport(ReportType.公司到位率统计表, from, to, 是否重点站, 热源, 收费性质, 数据来源, currentUser.VisibleCompanies.First().Id, managershipId) as CompletionReport;
                }
                else
                {
                    managershipId = currentUser.VisibleManagerships.First().Id;
                    report = ReportFactory.CreateRangeReport(ReportType.公司到位率统计表, from, to, 是否重点站, 热源, 收费性质, 数据来源, companyId, managershipId) as CompletionReport;
                }
                report.ReportData = report.ReportData.Where(i =>  i.公司名 == currentUser.VisibleManagerships.Single(j=>j.Id == managershipId).Name || i.公司名 == "合计").ToList();
            }
            else
            {
                return RedirectToAction("Index");
            }

            return View(new CompletionReportViewModel()
            {
                ReportTypeId = (int)ReportType.公司到位率统计表,
                ReportData = report.ReportData,
                From = from,
                To = to,
                Title = (ReportType.公司到位率统计表).ToString(),
                Companies = currentUser.含可见所有公司,
                Managerships = currentUser.是否有集团权限 || currentUser.是否有分公司权限 ? new List<IdAndName>() : currentUser.含所有可见中心,
                收费性质 = 收费性质,
                是否重点站 = 是否重点站,
                数据来源 = 数据来源,
                热源 = 热源,
                SelectedCompanyId = companyId, 
                SelectedManagershipId = managershipId
            });
        }
        public ActionResult DownloadCompletion(string dateFrom, string dateTo, int companyId = -1, int managershipId = -1, int 是否重点站 = 2, string 热源 = "ALL", string 收费性质 = "ALL", string 数据来源 = "ALL")
        {
            var from = DateTime.Parse(dateFrom);
            var to = DateTime.Parse(dateTo);
            var currentUser = MembershipService.CurrentUser;
            //根据权限查询

            //集团员工可以查询全部，分公司权限可以查询分公司，中心权限查询中心。
            CompletionReport report;
            if (currentUser.是否有集团权限)
            {
                report = ReportFactory.CreateRangeReport(ReportType.公司到位率统计表, from, to, 是否重点站, 热源, 收费性质, 数据来源, companyId, managershipId) as CompletionReport;
                if (companyId != -1)
                {
                    var m = (new ManagershipRepository()).SearchFor(i => i.公司ID == companyId).Select(i => i.管理单位).ToList();
                    report.ReportData = report.ReportData.Where(i => m.Exists(j => j == i.公司名) || i.公司名 == "合计").ToList();
                }

            }
            else if (currentUser.是否有分公司权限 && currentUser.VisibleCompanies.Count > 0)
            {
                if (currentUser.VisibleCompanies.Count(i => i.Id == companyId) > 0)
                {
                    report = ReportFactory.CreateRangeReport(ReportType.公司到位率统计表, from, to, 是否重点站, 热源, 收费性质, 数据来源, companyId, managershipId) as CompletionReport;
                }
                else
                {
                    companyId = currentUser.VisibleCompanies.First().Id;
                    report = ReportFactory.CreateRangeReport(ReportType.公司到位率统计表, from, to, 是否重点站, 热源, 收费性质, 数据来源, companyId, managershipId) as CompletionReport;
                }
                var m = (new ManagershipRepository()).SearchFor(i => i.公司ID == companyId).Select(i => i.管理单位).ToList();
                report.ReportData = report.ReportData.Where(i => m.Exists(j => j == i.公司名) || i.公司名 == "合计").ToList();
            }
            else if (currentUser.是否有供热中心权限 && currentUser.VisibleManagerships.Count > 0)
            {

                companyId = currentUser.VisibleCompanies.First().Id;
                if (currentUser.VisibleManagerships.Count(i => i.Id == managershipId) > 0)
                {
                    report = ReportFactory.CreateRangeReport(ReportType.公司到位率统计表, from, to, 是否重点站, 热源, 收费性质, 数据来源, currentUser.VisibleCompanies.First().Id, managershipId) as CompletionReport;
                }
                else
                {
                    managershipId = currentUser.VisibleManagerships.First().Id;
                    report = ReportFactory.CreateRangeReport(ReportType.公司到位率统计表, from, to, 是否重点站, 热源, 收费性质, 数据来源, companyId, managershipId) as CompletionReport;
                }
                report.ReportData = report.ReportData.Where(i => i.公司名 == currentUser.VisibleManagerships.Single(j => j.Id == managershipId).Name || i.公司名 == "合计").ToList();
            }
            else
            {
                return Json(new { fileName = "" }, JsonRequestBehavior.AllowGet);
            }

            var templateName = string.Format("{0}{1}.xlsx", ConfigurationManager.AppSettings["ReportTemplateDirectory"], report.TemplateName);
            FileInfo template = new FileInfo(templateName);
            string newFileName = "";
            if (companyId != -1)
            {
                newFileName = string.Format("{0}{1}{2:yyyy-MM-dd}-{3:yyyy-MM-dd}.xlsx", report.TemplateName, CompanyHelper.GetCompanyById(companyId), dateFrom, dateTo);
            }
            else
            {
                newFileName = string.Format("{0}{1:yyyy-MM-dd}-{2:yyyy-MM-dd}.xlsx", report.TemplateName, dateFrom, dateTo);
            }

            FileInfo newFile = new FileInfo(Server.MapPath("~/DownloadedReport/") + newFileName);
            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Ti3ned Goods"];
                report.FillReport(worksheet);
                xlPackage.Workbook.Properties.Title = report.TemplateName;
                xlPackage.Save();
            }

            return Json(new { fileName = "/DownloadedReport/" + newFileName }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PreviewStationAnalize(string dateFrom, string dateTo, int companyId = -1, int managershipId = -1, int 是否重点站 = 2, string 热源 = "ALL", string 收费性质 = "ALL", string 数据来源 = "ALL")
        {
            var from = DateTime.Parse(dateFrom);
            var to = DateTime.Parse(dateTo);
            var currentUser = MembershipService.CurrentUser; 
            //根据权限查询

            //集团员工可以查询全部，分公司权限可以查询分公司，中心权限查询中心。
            StationAnalizeReport report;
            if (currentUser.是否有集团权限)
            {
                report = ReportFactory.CreateRangeReport(ReportType.热力站分析, from, to, 是否重点站, 热源, 收费性质, 数据来源, companyId, managershipId) as StationAnalizeReport;
            }
            else if (currentUser.是否有分公司权限 && currentUser.VisibleCompanies.Count > 0)
            {
                if (currentUser.VisibleCompanies.Count(i => i.Id == companyId) > 0)
                {
                    report = ReportFactory.CreateRangeReport(ReportType.热力站分析, from, to, 是否重点站, 热源, 收费性质, 数据来源, companyId, managershipId) as StationAnalizeReport;
                }
                else
                {
                    companyId = currentUser.VisibleCompanies.First().Id;
                    report = ReportFactory.CreateRangeReport(ReportType.热力站分析, from, to, 是否重点站, 热源, 收费性质, 数据来源, companyId, managershipId) as StationAnalizeReport;
                }
            }
            else if (currentUser.是否有供热中心权限 && currentUser.VisibleManagerships.Count > 0)
            {
                companyId = currentUser.VisibleCompanies.First().Id;
                if (currentUser.VisibleManagerships.Count(i => i.Id == managershipId) > 0)
                {
                    report = ReportFactory.CreateRangeReport(ReportType.热力站分析, from, to, 是否重点站, 热源, 收费性质, 数据来源, companyId, managershipId) as StationAnalizeReport;
                }
                else
                {
                    managershipId = currentUser.VisibleManagerships.First().Id;
                    report = ReportFactory.CreateRangeReport(ReportType.热力站分析, from, to, 是否重点站, 热源, 收费性质, 数据来源, companyId, managershipId) as StationAnalizeReport;
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
            return View(new StationAnalizeViewModel()
            {
                ReportTypeId = (int)ReportType.热力站分析,
                ReportData = report.ReportData,
                From = from,
                To = to,
                Title = (ReportType.热力站分析).ToString(),
                Companies = currentUser.含可见所有公司,
                Managerships = companyId != -1 ? CompanyHelper.GetAllManagershipByCompanyId(companyId) : currentUser.含所有可见中心,
                收费性质 = 收费性质,
                是否重点站 = 是否重点站,
                数据来源 = 数据来源,
                热源 = 热源,
                SelectedCompanyId = companyId,
                SelectedManagershipId = managershipId
            });
        }

        public ActionResult DownloadStationAnalize(string dateFrom, string dateTo, int companyId = -1, int managershipId = -1, int 是否重点站 = 2, string 热源 = "ALL", string 收费性质 = "ALL", string 数据来源 = "ALL")
        {
            var from = DateTime.Parse(dateFrom);
            var to = DateTime.Parse(dateTo);
            var currentUser = MembershipService.CurrentUser;
            //根据权限查询

            //集团员工可以查询全部，分公司权限可以查询分公司，中心权限查询中心。
            StationAnalizeReport report;
            if (currentUser.是否有集团权限)
            {
                report = ReportFactory.CreateRangeReport(ReportType.热力站分析, from, to, 是否重点站, 热源, 收费性质, 数据来源, companyId, managershipId) as StationAnalizeReport;
            }
            else if (currentUser.是否有分公司权限 && currentUser.VisibleCompanies.Count > 0)
            {
                if (currentUser.VisibleCompanies.Count(i => i.Id == companyId) > 0)
                {
                    report = ReportFactory.CreateRangeReport(ReportType.热力站分析, from, to, 是否重点站, 热源, 收费性质, 数据来源, companyId, managershipId) as StationAnalizeReport;
                }
                else
                {
                    companyId = currentUser.VisibleCompanies.First().Id;
                    report = ReportFactory.CreateRangeReport(ReportType.热力站分析, from, to, 是否重点站, 热源, 收费性质, 数据来源, companyId, managershipId) as StationAnalizeReport;
                }
            }
            else if (currentUser.是否有供热中心权限 && currentUser.VisibleManagerships.Count > 0)
            {
                companyId = currentUser.VisibleCompanies.First().Id;
                if (currentUser.VisibleManagerships.Count(i => i.Id == managershipId) > 0)
                {
                    report = ReportFactory.CreateRangeReport(ReportType.热力站分析, from, to, 是否重点站, 热源, 收费性质, 数据来源, companyId, managershipId) as StationAnalizeReport;
                }
                else
                {
                    managershipId = currentUser.VisibleManagerships.First().Id;
                    report = ReportFactory.CreateRangeReport(ReportType.热力站分析, from, to, 是否重点站, 热源, 收费性质, 数据来源, companyId, managershipId) as StationAnalizeReport;
                }
            }
            else
            {
                return Json(new { fileName = "" }, JsonRequestBehavior.AllowGet);
            }

            var templateName = string.Format("{0}{1}.xlsx", ConfigurationManager.AppSettings["ReportTemplateDirectory"], report.TemplateName);
            FileInfo template = new FileInfo(templateName);
            string newFileName = "";
            if (companyId != -1)
            {
                newFileName = string.Format("{0}{1}{2:yyyy-MM-dd}-{3:yyyy-MM-dd}.xlsx", report.TemplateName, CompanyHelper.GetCompanyById(companyId), dateFrom, dateTo);
            }
            else
            {
                newFileName = string.Format("{0}{1:yyyy-MM-dd}-{2:yyyy-MM-dd}.xlsx", report.TemplateName, dateFrom, dateTo);
            }

            FileInfo newFile = new FileInfo(Server.MapPath("~/DownloadedReport/") + newFileName);
            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Ti3ned Goods"];
                report.FillReport(worksheet);
                xlPackage.Workbook.Properties.Title = report.TemplateName;
                xlPackage.Save();
            }

            return Json(new { fileName = "/DownloadedReport/" + newFileName }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult DownloadReport(int reportType, string date)
        {
            DateTime day = DateTime.Parse(date);
            var report = ReportFactory.CreateReport((ReportType)reportType, day);
            var templateName = string.Format("{0}{1}.xlsx", ConfigurationManager.AppSettings["ReportTemplateDirectory"] ,report.TemplateName);
            FileInfo template = new FileInfo(templateName);
            var newFileName = string.Format("{0}{1:yyyy-MM-dd}.xlsx", report.TemplateName, day);
            FileInfo newFile = new FileInfo(Server.MapPath("~/DownloadedReport/") + newFileName);
            using (ExcelPackage xlPackage = new ExcelPackage(newFile,template))
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Ti3ned Goods"];
                report.FillReport(worksheet);
                xlPackage.Workbook.Properties.Title = report.TemplateName;
                xlPackage.Save();
            }

            return Json(new { fileName = "/DownloadedReport/" + newFileName }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult DownloadRangeReport(int reportType, string fromDate, string toDate, int companyId = 0)
        {
            DateTime fromDay = DateTime.Parse(fromDate);
            DateTime toDay = DateTime.Parse(toDate);
            var report = ReportFactory.CreateRangeReport((ReportType)reportType, fromDay, toDay, companyId);
            var templateName = string.Format("{0}{1}.xlsx", ConfigurationManager.AppSettings["ReportTemplateDirectory"], report.TemplateName);
            FileInfo template = new FileInfo(templateName);
            string newFileName = "";
            if (reportType == (int) ReportType.数据分析表 || reportType == (int) ReportType.热力站分析)
            {
                newFileName = string.Format("{0}{1}{2:yyyy-MM-dd}-{3:yyyy-MM-dd}.xlsx", report.TemplateName, CompanyHelper.GetCompanyById(companyId), fromDay, toDay);
            }
            else
            {
                newFileName = string.Format("{0}{1:yyyy-MM-dd}-{2:yyyy-MM-dd}.xlsx", report.TemplateName, fromDay, toDay);
            }
            
            FileInfo newFile = new FileInfo(Server.MapPath("~/DownloadedReport/") + newFileName);
            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Ti3ned Goods"];
                report.FillReport(worksheet);
                xlPackage.Workbook.Properties.Title = report.TemplateName;
                xlPackage.Save();
            }

            return Json(new { fileName = "/DownloadedReport/" + newFileName }, JsonRequestBehavior.AllowGet);

        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }
    }
}
