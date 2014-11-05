using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;

namespace ReliDemo.ViewModels
{
    public class UserInfoViewModel
    {
        public PaginatedList<User> Users { get; set; }
        private IEnumerable<IdAndName> _companies;
        public IEnumerable<IdAndName> Companies
        {
            get
            {
                if (_companies == null)
                {
                    _companies = new List<IdAndName>();
                }
                return _companies;
            }
            set
            {
                _companies = value;
            }
        }
        private IEnumerable<IdAndName> _managerships;
        public IEnumerable<IdAndName> Managerships
        {
            get
            {
                if (_managerships == null)
                {
                    _managerships = new List<IdAndName>();
                }
                return _managerships;
            }
            set
            {
                _managerships = value;
            }
        }
        public int? SelectedCompanyId { get; set; }
        public int? SelectedManagershipId { get; set; }
        public string SearchKeyword { get; set; }
    }
}