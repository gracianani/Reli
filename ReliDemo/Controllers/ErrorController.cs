using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ReliDemo.Controllers
{
    public class ErrorController : Controller
    {

        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}
