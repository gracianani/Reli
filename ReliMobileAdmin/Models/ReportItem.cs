using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliMobileAdmin.Models
{
    public class ReportItem
    {
        public string FileName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }
    }
}