using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;
using OfficeOpenXml;

namespace ReliDemo.Core.Interfaces
{
    public interface IReport
    {
        ReportType ReportType { get; }

        string TemplateName { get; set; }
        void FillReport(ExcelWorksheet worksheet);
    }
}