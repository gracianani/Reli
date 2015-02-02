using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReliMobileAdmin.Models;
using System.Web.Security;

namespace ReliMobileAdmin.Controllers
{
    public class AccountController : Controller
    {
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
            
            if (ModelState.IsValid)
            {
                if (model.IsValid(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }
            }
            return View(model);
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }
    }
}
