using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Core.Interfaces;
using ReliDemo.Models;

namespace ReliDemo.Infrastructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private xz2013Entities db = new xz2013Entities();
        
        public virtual IEnumerable<Company> GetAllCompanies()
        {
            return db.Companies.Where(i=>i.有效监控站数 > 0).ToList();
        }

        public virtual Company Find(int companyId)
        {
            return db.Companies.Single(i => i.ItemID == companyId);
        }
    }
}