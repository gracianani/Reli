using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Core.Interfaces;
using ReliDemo.Infrastructure.Repositories;

namespace ReliDemo.Models
{
    public partial class User : IUser
    {
        public List<IdAndName> VisibleManagerships
        {
            get
            {
                var managerships = new List<IdAndName>();
                if (是否有供热中心权限)
                {
                    managerships.Add(new IdAndName() { Id = Managership.ItemID, Name = Managership.管理单位});
                }
                return managerships;
            }
        }
        public List<IdAndName> VisibleCompanies
        {
            get
            {
                var companies = new List<IdAndName>();
                if (是否有集团权限)
                {
                    companies.Add(new IdAndName() { Id = 0, Name = "创合供热公司" });
                    companies.Add(new IdAndName() { Id = 4, Name = "特力昆公司" });
                    companies.Add(new IdAndName() { Id = 5, Name = "天禹供热公司" });
                    companies.Add(new IdAndName() { Id = 6, Name = "销售公司" });
                }
                else if (是否有分公司权限)
                {
                    companies.Add(new IdAndName() { Id = Company.ItemID, Name = Company.公司 });
                }
                else if (是否有供热中心权限)
                {
                    companies.Add(new IdAndName() { Id = Managership.Company.ItemID, Name = Managership.Company.公司 });
                }
                return companies;
            }
        }

        public List<IdAndName> 含可见所有公司
        {
            get
            {
                var companies = new List<IdAndName>();
                if (是否有集团权限)
                {
                    companies.Add(new IdAndName() { Id = -1, Name = "所有公司" });
                    companies.Add(new IdAndName() { Id = 6, Name = "销售" });
                    companies.Add(new IdAndName() { Id = 0, Name = "创合" });
                    companies.Add(new IdAndName() { Id = 4, Name = "特力昆" });
                    companies.Add(new IdAndName() { Id = 5, Name = "天禹" });
                }
                else if (是否有分公司权限)
                {
                    companies.Add(new IdAndName() { Id = Company.ItemID, Name = Company.公司 });
                }
                else if (是否有供热中心权限)
                {
                    companies.Add(new IdAndName() { Id = Managership.Company.ItemID, Name = Managership.Company.公司 });
                }
                return companies;
            }
        }

        public List<IdAndName> 含所有可见中心
        {
            get
            {
                var managerships = new List<IdAndName>();
                var companies = 含可见所有公司;
                var allManagerships = (new ManagershipRepository()).GetAllManagerships();
                foreach (var company in companies)
                {
                    managerships.AddRange(allManagerships.Where(i => i.公司ID == company.Id).Select(i => new IdAndName() { Id = i.ItemID, Name = i.管理单位 }));
                }
                return managerships;
            }
        }

        public bool 是否有分公司权限

        {
            get
            {
                return (webpages_Roles.Count(i => i.RoleId == (int)Role.分公司调度) > 0 && Company != null);
            }
        }
        public bool 是否有供热中心权限
        {
            get
            {
                return (webpages_Roles.Count(i => i.RoleId == (int)Role.供热中心调度) > 0 && Managership != null);
            }
        }
        public bool 是否有集团权限
        {
            get
            {
                return (webpages_Roles.Count(i => i.RoleId == (int)Role.生产部调度 || i.RoleId == (int)Role.系统管理员 || i.RoleId == (int)Role.系统维护员 || i.RoleId == (int)Role.抄表填报) > 0 && _是否为集团员工);
            }
        }
        public bool 是否可以设置权限
        {
            get
            {
                return (webpages_Roles.Count(i => i.RoleId == (int)Role.系统维护员) > 0 && _是否为集团员工);
            }
        }
        public bool 是否可以修改温度
        {
            get
            {
                return (webpages_Roles.Count(i => i.RoleId == (int)Role.系统管理员) > 0);
            }
        }
        public bool 是否可以修改热指标
        {
            get
            {
                return (webpages_Roles.Count(i => i.RoleId == (int)Role.系统管理员) > 0 );
            }
        }
        public bool 是否可以修改回温线
        {
            get
            {
                return (webpages_Roles.Count(i => i.RoleId == (int)Role.系统管理员) > 0 && _是否为集团员工);
            }
        }
        public string LandingPage
        {
            get
            {
                if (是否有集团权限)
                {
                    return "/Home/Index";
                }
                else if (是否有分公司权限)
                {
                    return string.Format("/Companies/Index?companyId={0}", Company.ItemID);
                }
                else if (是否有供热中心权限)
                {
                    return string.Format("/Companies/ManagershipInfo?managershipId={0}", Managership.ItemID);
                }
                else
                {
                    return "/Weather/WeatherForecast";
                }
            }
        }

        public string 权限们
        {
            get
            {
                return string.Join(",", 权限);
            }
        }

        public IList<string> 权限
        {
            get
            {
                var result = new List<string>();
                foreach (var role in this.webpages_Roles)
                {
                    switch((Role)role.RoleId) {
                        case Role.供热中心调度 :
                            if (Managership != null)
                            {
                                result.Add(string.Format( "{0} ({1})", role.RoleName, Managership.管理单位 ));
                            }
                            break;
                        case Role.分公司调度:
                            if (Company != null)
                            {
                                result.Add(string.Format("{0} ({1})", role.RoleName, Company.公司));
                            }
                            break;
                        case Role.抄表填报:
                            break;
                        case Role.生产部调度:
                            if (_是否为集团员工)
                            {
                                result.Add(role.RoleName);
                            }
                            break;
                        case Role.系统管理员:
                        case Role.系统维护员:
                            result.Add(role.RoleName);
                            break;
                    }
                }
                return result;
            }
        }

        public int UserId
        {
            get
            {
                return _userId;
            }
        }

        public string UserName
        {
            get
            {
                return _email;
            }
        }

        public string FullName
        {
            get
            {
                return _姓名;
            }
        }

        public List<Role> Roles
        {
            get
            {
                return this.webpages_Roles.Select(i => (Role)i.RoleId).ToList();
            }
        }
    }
}