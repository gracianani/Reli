using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using ReliDemo.ViewModels;
using ReliDemo.Infrastructure.Repositories;
using ReliDemo.Core.Interfaces;
using ReliDemo.Models;

namespace ReliDemo.Controllers
{

    [Authorize]
    public class AccountsController : Controller
    {

        IUserRepository _userRepo;

        public AccountsController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [AllowAnonymous]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [HandleError(ExceptionType = typeof(CookiesNotEnabledException), View = "NoCookies")]
        public ActionResult LogIn(LogInViewModel model)
        {
            try
            {
                var result = WebSecurity.Login(model.UserName, model.Password, model.RememberMe);
                if (!result)
                {
                    ModelState.AddModelError("", "用户名或密码不正确");
                }
                else
                {
                    var currentUser = ((User)_userRepo.FindByUserName(model.UserName));
                    var landingPage = currentUser.LandingPage;
                    return Redirect(landingPage);
                }
            }catch{
                FormsAuthentication.SetAuthCookie(model.UserName, true);
                var currentUser = ((User)_userRepo.FindByUserName(model.UserName));
                var landingPage = currentUser.LandingPage;
                return Redirect(landingPage);
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(RegisterViewModel model)
        {
            var message = WebSecurity.CreateUserAndAccount(model.Email, model.Password, new { 姓名 = model.姓名}, false);
            
            return RedirectToAction("RegisterSuccessful");
        }

        [AllowAnonymous]
        public ActionResult RegisterSuccessful()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            var result = WebSecurity.ChangePassword(WebSecurity.CurrentUserName, model.CurrentPassword, model.NewPassword);
            if (!result)
            {
                ModelState.AddModelError("", "当前密码不正确");
            }
            else
            {
                return RedirectToAction("ChangePasswordSuccessful");
            }
            return View(model);
        }

        public ActionResult LogOut()
        {
            WebSecurity.Logout();
            return RedirectToAction("LogIn");
        }

        public ActionResult UserProfile()
        {
            var membership = Membership.GetUser();
            var userProfile = (User) new UserRepository().FindByUserName(membership.UserName);
            return View(new ProfileViewModel { UserId = userProfile.userId, UserName = WebSecurity.CurrentUserName, 姓名 = userProfile.姓名 });
        }

        [HttpPost]
        public ActionResult UserProfile(ProfileViewModel model)
        {
            var membership = Membership.GetUser();
            var userProfile = (User) _userRepo.FindByUserName(membership.UserName);
            userProfile.姓名 = model.姓名;
            _userRepo.Update(userProfile);
            return View(model);
        }
    }
}
