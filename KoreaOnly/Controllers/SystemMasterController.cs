using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KoreaOnly.Views
{
    public class SystemMasterController : Controller
    {
        [Route("SystemMaster/")]
        public ActionResult Index()
        {
            if (MainController.checkAdminLogin())
            {
                return RedirectToAction("MainSetup");
            }
            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexPOST(string User, string Pass)
        {
            try
            {
                if (User == "FLIGHT" && Pass == "ADMIN!@#")
                {
                    Session["ADMINLOGINED"] = "ADMIN#@$";
                    return RedirectToAction("MainSetup");
                }
            }
            catch (Exception ex)
            {

            }


            return View("Index");
        }

        
        public ActionResult MainSetup()
        {
            if (MainController.checkAdminLogin())
            {
                return View();
            }

            return RedirectToAction("Index");
        }

    }
}