using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using ReliMobileAdmin.Models;
using System.Configuration;

namespace ReliMobileAdmin.Controllers
{
    [Authorize]
    public class DailyReportController : Controller
    {
        private string DailyReportsRoot
        {
            get
            {
                return ConfigurationManager.AppSettings["DailyReportsRoot"] ;
            }
        }

        private string DailyReportsServerPath
        {
            get
            {
                return ConfigurationManager.AppSettings["DailyReportsServerPath"];
            }
        }

        private string CustomerReportsRoot
        {
            get
            {
                return ConfigurationManager.AppSettings["CustomerReportsRoot"];
            }
        }

        private string CustomerReportsServerPath
        {
            get
            {
                return ConfigurationManager.AppSettings["CustomerReportsServerPath"];
            }
        }

        private string KnowledgeFilesRoot
        {
            get
            {
                return ConfigurationManager.AppSettings["KnowledgeFilesRoot"];
            }
        }

        private string KnowledgeFilesServerPath
        {
            get
            {
                return ConfigurationManager.AppSettings["KnowledgeFilesServerPath"];
            }
        }

        public ActionResult ProductionReport()
        {
            var files = new DirectoryInfo(DailyReportsRoot)
                    .GetFiles("*", SearchOption.TopDirectoryOnly)
                    .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden))
                    .Select((f, index) => new ReportItem() { CreatedAt=f.CreationTime, FileName = f.Name, Extension = f.Extension, Path = f.FullName })
                    .ToArray();
            return View(new ProductionReportViewModel() { Reports = files, ReportType = ReportType.ProductionReport });
        }

        public ActionResult CustomerReport()
        {
            var files = new DirectoryInfo(CustomerReportsRoot)
                    .GetFiles("*", SearchOption.TopDirectoryOnly)
                    .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden))
                    .Select((f, index) => new ReportItem() { CreatedAt = f.CreationTime, FileName = f.Name, Extension = f.Extension, Path = f.FullName })
                    .ToArray();
            return View(new ProductionReportViewModel() { Reports = files, ReportType = ReportType.CustomerReport });
        }

        public ActionResult KnowledgeFiles()
        {
            var files = new DirectoryInfo(KnowledgeFilesRoot)
                    .GetFiles("*", SearchOption.TopDirectoryOnly)
                    .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden))
                    .Select((f, index) => new ReportItem() { CreatedAt = f.CreationTime, FileName = f.Name, Extension = f.Extension, Path = f.FullName })
                    .ToArray();
            return View(new ProductionReportViewModel() { Reports = files, ReportType = ReportType.KnowledgeFile });
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase FileData, string reportType)
        {
            var enumReportType = (ReportType) Enum.Parse( typeof(ReportType),  reportType);
            string fileRoot = "";
            if (enumReportType == ReportType.CustomerReport)
            {
                fileRoot = CustomerReportsRoot;
            }
            else if (enumReportType == ReportType.ProductionReport)
            {
                fileRoot = DailyReportsRoot;
            }
            else
            {
                fileRoot = KnowledgeFilesRoot;
            }
            if (FileData != null && FileData.ContentLength > 0)
            {
                var fileName = Path.GetFileName(FileData.FileName);
                var path = Path.Combine(fileRoot, fileName);
                FileData.SaveAs(path);
                var file = new FileInfo(path);
                return new JsonResult() { Data = "{ \"result\" : true, \"creationDate\" : \"" + file.CreationTime.ToString("MM月dd日, tt HH:mm", new System.Globalization.CultureInfo("zh-cn")) + "\" }" };
            }
            return new JsonResult() { Data = "{ \"result\" : false }" };
        }

        [HttpDelete]
        public ActionResult Delete(string fileName, string extension, string reportType)
        {
            var enumReportType = (ReportType)Enum.Parse(typeof(ReportType), reportType);
            string fileRoot = "";
            if (enumReportType == ReportType.CustomerReport)
            {
                fileRoot = CustomerReportsRoot;
            }
            else if(enumReportType == ReportType.ProductionReport)
            {
                fileRoot = DailyReportsRoot;
            }
            else
            {
                fileRoot = KnowledgeFilesRoot;
            }
            var path = Path.Combine(fileRoot, fileName.Trim() + extension);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
                return new JsonResult() { Data = " { \"isDeleted\" : true } " };
            }
            else
            {
                return new JsonResult() { Data = " { \"isDeleted\" : false } " };
            }
        }

        [HttpGet]
        public ActionResult Download(string fileName, string extension, string reportType)
        {
            var enumReportType = (ReportType)Enum.Parse(typeof(ReportType), reportType);
            string fileRoot = "";
            if (enumReportType == ReportType.CustomerReport)
            {
                fileRoot = CustomerReportsServerPath;
            }
            else if (enumReportType == ReportType.ProductionReport)
            {
                fileRoot = DailyReportsServerPath;
            }
            else
            {
                fileRoot = KnowledgeFilesRoot;
            }
            return Json(new { fileName = fileRoot + fileName.Trim() + extension }, JsonRequestBehavior.AllowGet);
        }
    }
}
