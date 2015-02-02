using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Core.Interfaces;

namespace ReliDemo.Models
{
    public abstract class HeatConsumptionReportBase : IReport
    {
        public abstract IEnumerable<StationAccuHistory> Statistics { get; set; }

        public abstract ReportType ReportType
        {
            get;
        }

        public abstract string TemplateName
        {
            get;
            set;
        }

        private System.Xml.XmlDocument _reportContent;
        public System.Xml.XmlDocument ReportContent
        {
            get
            {
                return _reportContent;
            }
            set
            {
                _reportContent = value;
            }
        }

        public abstract void FillReport(OfficeOpenXml.ExcelWorksheet worksheet);
    }
}