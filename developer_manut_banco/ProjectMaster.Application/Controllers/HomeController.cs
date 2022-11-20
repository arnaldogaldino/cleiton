using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectMaster.Bussiness;
using ProjectMaster.Core;

namespace ProjectMaster.Application.Controllers
{
    public class HomeController : ControllerMaster
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
