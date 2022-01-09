using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoCoWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "SoCo - your trusted SOftware COmpany.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact page: SoCo";

            return View();
        }
    }
}