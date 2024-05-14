using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobPortal.Controllers
{
    public class ErrorController : Controller
    {
        // GET: /Error/
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }

            return View("Error");
        }

        // GET: /Error/NotFound
        public ActionResult NotFound()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }

            Response.StatusCode = 404;
            return View("NotFound");
        }
    }
}