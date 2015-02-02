using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;

namespace ReliDemo.Core.Interfaces
{
    public interface ICompanyRepository
    {
        IEnumerable<Company> GetAllCompanies();
        Company Find(int companyId);
    }
}