using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReliMobileAdmin.Models
{
    public class HomeViewModel
    {
        public int NumOfNewMessage { get; set; }

        public int NumOfWarningMessages { get; set; }

        public DateTime LatestDailyReport { get; set; }

        public DateTime LatestCustomerReport { get; set; }

        public IEnumerable<User> Users { get; set; }
        public int SelectedUserId { get; set; }
        public string Message { get; set; }
        public string WarningTitle { get; set; }
        public string WarningContent { get; set; }
    }
}