using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    [DataContract]
    public class DailyReport
    {
        private int _reportId;
        private string _reportName;
        private DateTime _createdAt;

        [DataMember]
        public int intReportId
        {
            get
            {
                return _reportId;
            }
            set
            {
                _reportId = value;
            }
        }

        [DataMember]
        public string strReportName
        {
            get {
                return _reportName;
            }
            set
            {
                _reportName = value;
            }
        }

        [DataMember]
        public DateTime dtCreatedAt
        {
            get
            {
                return _createdAt;
            }
            set
            {
                _createdAt = value;
            }
        }

        [DataMember]
        public string strPath
        {
            get {
                return "http://192.168.1.106:11223/reports/" + _reportName;
            }
            set {
            }
        }


        [DataMember]
        public string strListId
        {
            get
            {
                return _reportId.ToString();
            }
            set
            {
                _reportId = Convert.ToInt32(value);
            }
        }
        [DataMember]
        public string strListName
        {
            get
            {
                return strReportName;
            }
            set
            {
                strReportName = value;
            }
        }

        [DataMember]
        public string strListOther
        {
            get
            {
                return "";
            }
            set
            {
            }
        }
        public DailyReport(int reportId, string reportName, DateTime createdAt)
        {
            _reportId = reportId;
            _reportName = reportName;
            _createdAt = createdAt;
        }
    }
}