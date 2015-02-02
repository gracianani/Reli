using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using ReliDemo.Core.Interfaces;
using System.Web.Mvc;
using ReliDemo.Infrastructure.Repositories;
using ReliDemo.ViewModels;
using WebMatrix.WebData;
using ReliDemo.Models;

namespace ReliDemo.Controllers
{
    [Authorize]
    public class SecurityController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly ICompanyRepository _companyRepo;
        private readonly IManagershipRepository _managershipRepo;

        public SecurityController(IUserRepository userRepo, ICompanyRepository companyRepo, IManagershipRepository managershipRepo)
        {
            _userRepo = userRepo;
            _companyRepo = companyRepo;
            _managershipRepo = managershipRepo;
        }

        [Authorize(Roles = "系统管理员")]
        public ActionResult UserInfo(int pageIndex=0, int pageSize=10,int? selectedCompanyId = null, int? selectedManagershipId = null, string keyword = "")
        {
            var users = _userRepo.GetAllUsers().Select(i=>(User)i).ToList().Where(i =>
                    (!selectedCompanyId.HasValue || (i.Company != null && i.Company.ItemID == selectedCompanyId.Value)) &&
                    (!selectedManagershipId.HasValue || (i.Managership != null && i.Managership.ItemID == selectedManagershipId.Value)) &&
                    (string.IsNullOrEmpty(keyword) || ( !string.IsNullOrEmpty(i.姓名) && i.姓名.IndexOf(keyword)>=0 ))).ToList(); 
            var userInfoViewModel = new UserInfoViewModel()
            {
                Users = new PaginatedList<User>( users, pageIndex, pageSize),
                SelectedCompanyId = selectedCompanyId,
                SelectedManagershipId = selectedManagershipId,
                SearchKeyword = keyword,
                Companies = _companyRepo.GetAllCompanies().Select( i=> new IdAndName() { Id = i.ItemID, Name = i.公司}),
                Managerships = _managershipRepo.GetAllManagerships().Where(i=>selectedCompanyId.HasValue && i.公司ID == selectedCompanyId.Value || !selectedCompanyId.HasValue)
                .Select( i => new IdAndName() { Id= i.ItemID, Name = i.管理单位 }) 
            };
            return View(userInfoViewModel);
        }

        [Authorize(Roles="系统管理员")]
        public ActionResult Configuration()
        {
            return View();
        }

        [Authorize(Roles = "系统管理员")]
        public ActionResult ViewProfile(int userId)
        {
            var userProfile = (User)_userRepo.Find(userId);
            return View(new ProfileViewModel {UserId = userId, UserName = userProfile.email, 姓名 = userProfile.姓名 });
        }

        [System.Web.Mvc.HttpPost]
        [Authorize(Roles = "系统管理员")]
        public ActionResult ViewProfile(FormCollection collection)
        {
            var userId = Convert.ToInt32( collection["UserId"] );
            var userName = collection["userName"];
            var xingming = collection["姓名"];
            var password = collection["密码"];

            var user = (User)_userRepo.Find(userId);

            if (string.Compare(user.姓名, xingming) != 0)
            {
                user.姓名 = xingming;
            }
            if (string.Compare(user.email, userName, true) != 0)
            {
                if (_userRepo.FindByUserName(userName) == null)
                {
                    user.email = userName;
                }
            }
            var isJituan = collection["isCompany"];
            var companyName = collection["Companies"];
            var managershipName = collection["Managerships"];

            user.是否为集团员工 = isJituan == "on";
            var selectedCompany = CompanyHelper.GetAllCompany().FirstOrDefault(i=>i.Name == companyName);
            if (selectedCompany != null)
            {
                user.所属公司 = selectedCompany.Id;
            }
            else
            {
                user.所属公司 = null;
            }
            var selectedManagership =  _managershipRepo.GetAllManagerships().FirstOrDefault(i => i.管理单位 == managershipName);
            if (selectedManagership != null)
            {
                user.所属中心 = selectedManagership.ItemID;
            }
            else
            {
                user.所属中心 = null;
            }
            _userRepo.Update(user);

            if (!string.IsNullOrEmpty(password))
            {
                var resetPasswordToken = WebSecurity.GeneratePasswordResetToken(userName);
                WebSecurity.ResetPassword(resetPasswordToken, password);
            }

            var allRoles = Enum.GetValues(typeof(ReliDemo.Models.Role));
            var roles = (SimpleRoleProvider)System.Web.Security.Roles.Provider;
            foreach (var role in allRoles)
            {
                var roleName = role.ToString();
                var checkbox = collection[roleName];
                if (checkbox != null &&  user.webpages_Roles.Count(i=>i.RoleName == roleName) == 0)
                {
                    roles.AddUsersToRoles(new[] { user.email }, new[] { roleName });
                }
                else if (checkbox == null && user.webpages_Roles.Count(i => i.RoleName == roleName) > 0)
                {
                    roles.RemoveUsersFromRoles(new [] { user.email}, new[] { roleName });
                }
            }
            return RedirectToAction("ViewProfile", new { userId = user.userId } );
        }
    }
}
