using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectMaster.Core;

namespace ProjectMaster.Application.Controllers
{
    public class HomeController : ControllerMaster
    {
        
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";
            
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
