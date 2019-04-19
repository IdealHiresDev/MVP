using IdealHires.Web.TokenProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdealHires.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string ReturnUrl = null)
        {
            if(Request.IsAuthenticated)
            {
                if(User.UserType() == "Candidate")
                {
                    return RedirectToAction("Jobs", "Candidate");
                }
                else
                {
                    return RedirectToAction("EmployerDashboard", "Employer");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(ReturnUrl))
                {
                    ViewBag.ReturnUrl = ReturnUrl;
                }
                else
                {
                    ViewBag.ReturnUrl = string.Empty;
                }
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}