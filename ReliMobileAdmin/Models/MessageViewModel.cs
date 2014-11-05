using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliMobileAdmin.Models
{
    public class MessageViewModel
    {
        public IEnumerable<Message> Messages { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
    }
}