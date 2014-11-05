using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Infrastructure.Repositories;

namespace ReliDemo.Models
{
    public class CompanyHelper
    {
        public static string GetCompanyById(int companyId)
        {
            switch(companyId)
            {
                case 0:
                    return "创合";
                case 4:
                    return "特力昆";
                case 5:
                    return "天禹";
                case 6:
                    return "销售";
                default:
                    return "";
            }
        }
        public static string GetCompanyFullById(int companyId)
        {
            switch (companyId)
            {
                case 0:
                    return "创合供热公司";
                case 4:
                    return "特力昆公司";
                case 5:
                    return "天禹供热公司";
                case 6:
                    return "销售分公司";
                default:
                    return "";
            }
        }

        public static string GetManagershipById(int managershipId)
        {
            var repo = new ManagershipRepository();
            var managerships = repo.GetAllManagerships().First(i => i.ItemID == managershipId);
            return managerships.管理单位;
        }
        public static IList<IdAndName> GetAllCompany()
        {
            var companies = new List<IdAndName>();
            companies.Add(new IdAndName() { Id = 0, Name = "创合供热公司" });
            companies.Add(new IdAndName() { Id = 2, Name = "科利源供热公司" });
            companies.Add(new IdAndName() { Id = 4, Name = "特力昆公司" });
            companies.Add(new IdAndName() { Id = 5, Name = "天禹供热公司" });
            companies.Add(new IdAndName() { Id = 6, Name = "销售分公司" });
            return companies;
        }

        public static IList<IdAndName> GetAllManagershipByCompanyId( int companyId )
        {
            var repo = new ManagershipRepository();
            var managerships = repo.GetAllManagerships().Where(i => i.公司ID == companyId);
            return (
                from managership in managerships
                select new IdAndName
                {
                    Id = managership.ItemID,
                    Name = managership.管理单位
                }
                ).ToList();
        }
    }
}