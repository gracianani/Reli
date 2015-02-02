using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliMobileAdmin.Models
{
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
        public string warningTitle
        {
            get
            {
                return DBWarning.warningTitle;
            }
            set
            {
                DBWarning.warningTitle = value;
            }
        }

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

        public Warning(warning dbWarning)
        {
            _dbWarning = dbWarning;
        }
        public Warning()
        {
        }
    }
}