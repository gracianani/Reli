using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    [DataContract]
    public class Warning
    {
        private warning _dbWarning;
        public warning DBWarning
        {
            get
            {
                if (_dbWarning == null)
                {
                    _dbWarning = new warning();
                }
                return _dbWarning;
            }
            set
            {
                _dbWarning = value;
            }
        }
        
        public DateTime ReportedAt
        {
            get
            {
                return DBWarning.reportedAt;
            }
            set
            {
                DBWarning.reportedAt = value;
            }
        }

        [DataMember]
        public string reportedAt
        {
            get
            {
                return ReportedAt.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set
            {
            }
        }

        [DataMember]
        public int warningId
        {
            get
            {
                return DBWarning.warningId;
            }
            set
            {
            }
        }
        [DataMember]
        public string warningTitle {
            get
            {
                return DBWarning.warningTitle;
            }
            set
            {
                DBWarning.warningTitle = value;
            }
        }

        [DataMember]
        public string warningContent
        {
            get
            {
                return DBWarning.warningMessage;
            }
            set
            {
                DBWarning.warningMessage = value;
            }
        }

        public Warning( warning dbWarning)
        {
            _dbWarning = dbWarning;
        }
        public Warning()
        {
        }
    }
}