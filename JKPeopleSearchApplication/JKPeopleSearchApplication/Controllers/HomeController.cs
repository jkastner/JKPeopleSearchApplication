using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JKPeopleSearchApplication.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult About()
        {
            ViewBag.Message = "Person searcher - sample MVC project .";
            return View();
        }

        public ActionResult Contact()
        {

            return View();
        }
    }
}