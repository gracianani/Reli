using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliWebService.Repository
{
    public class WarningRepository
    {
        private ReliMobileEntities _db = new ReliMobileEntities();
        private List<Warning> _warnings;

        public List<Warning> Warnings
        {
            get
            {
                return _warnings;
            }
            set
            {
                _warnings = value;
            }
        }

        public WarningRepository() 
        {
            _warnings = new List<Warning>();
            _warnings = _db.warnings.Select(i => new Warning { DBWarning = i }).ToList().OrderByDescending(i => i.ReportedAt).ToList();
        }
    }
}