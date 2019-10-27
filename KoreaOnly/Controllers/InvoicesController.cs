using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KoreaOnly.Controllers
{
    [RoutePrefix("SystemMaster/Invoices")]
    public class InvoicesController : Controller
    {
        [Route("")]
        [Route("SystemMaster/Invoices/Index")]
        public ActionResult Index()
        {
            if (MainController.checkAdminLogin())
            {

                using (var DB = new DbConnection())
                {
                    var L = DB.GetResult<Models.Invoices>("SELECT * FROM Invoices ORDER BY IID DESC");

                    if (L != null)
                        return View(L);
                }

                return View(new List<Models.Invoices>());


            }

            return Redirect("/SystemMaster/");
        }

        
        public ActionResult Details(string ID)
        {
            if (MainController.checkAdminLogin())
            {
                using (var DB = new DbConnection())
                {
                    var L = DB.GetResult<Models.Invoices>($"SELECT * FROM Invoices WHERE IInvoiceNo = '{ID.Replace("'", "")}' ");

                    if (L != null)
                        return View(L.First());
                }

                return Redirect("/SystemMaster/");


            }

            return Redirect("/SystemMaster/");
        }

    }
}