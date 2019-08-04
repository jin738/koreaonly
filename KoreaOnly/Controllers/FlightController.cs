using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KoreaOnly.Controllers
{

    [RoutePrefix("SystemMaster/Flight")]
    public class FlightController : Controller
    {
        // GET: Admin
        [Route("")]
        [Route("SystemMaster/Flight/Index")]
        public ActionResult Index()
        {
            if (MainController.checkAdminLogin())
            {
                MainController.GetAirline();
                var L = (List<Airlines>)Session["AllAirline"];

                return View(L);
            }

            return Redirect("/SystemMaster/");

        }

        [Route("SystemMaster/Flight/Index")]
        [HttpPost]
        public ActionResult Index(string txtSearch = "")
        {
            MainController.GetAirline();
            var L = (List<Airlines>)Session["AllAirline"];

            return View(txtSearch == "" ? L : L.Where(x => x.AirlineCode == txtSearch).ToList());
        }


        [Route("SystemMaster/Flight/Create")]
        public ActionResult Create()
        {
            if (MainController.checkAdminLogin())
            {
                return View();
            }
            return Redirect("/SystemMaster/");
        }


        [Route("SystemMaster/Flight/AirlineLogo")]
        public ActionResult AirlineLogo()
        {
            if (MainController.checkAdminLogin())
            {
                return View();
            }
            return Redirect("/SystemMaster/");

        }

        [Route("SystemMaster/Flight/Create")]
        [HttpPost]
        public ActionResult Create(Airlines Airline)
        {
            using (var DB = new DbConnection())
            {
                DB.InsertData("insert into Airlines (AirlineCode,AirlineName) values ('" + Airline.AirlineCode + "','" + Airline.AirlineName + "')");
            }

            return RedirectToAction("Index");
        }

        [Route("SystemMaster/Flight/AirlineLogo")]
        [HttpPost]
        [ActionName("AirlineLogo")]
        public ActionResult UploadAirlineLogo(Airlines Airline)
        {

            foreach (var file in Airline.Files)
            {
                file.SaveAs(Server.MapPath("~/Images/Airlines/" + file.FileName));
            }

            return RedirectToAction("Index");
        }

        [Route("SystemMaster/Flight/Edit")]
        public ActionResult Edit(string aircode)
        {
            if (MainController.checkAdminLogin())
            {
                MainController.GetAirline();
                var airline = MainController.GetAirline(aircode);
                if (airline != null)
                {
                    return View(airline);
                }
                else
                {
                    return RedirectToAction("index");
                }
            }
            return Redirect("/SystemMaster/");
        }

        [Route("SystemMaster/Flight/Edit")]
        [HttpPost]
        public ActionResult Edit(Airlines Airlinee)
        {
            using (var DB = new DbConnection())
            {
                DB.InsertData("update airlines set airlinecode = '" + Airlinee.AirlineCode + "',airlineName = '" + Airlinee.AirlineName + "' where id =" + Airlinee.ID);
            }
            return RedirectToAction("Index");
        }

        [Route("SystemMaster/Flight/Delete")]
        public ActionResult Delete(string aircode)
        {
            if (MainController.checkAdminLogin())
            {
                var airline = MainController.GetAirline(aircode);
                if (airline != null)
                {
                    return View(airline);
                }
                else
                {
                    return RedirectToAction("index");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ActionName("Delete")]
        [Route("SystemMaster/Flight/Delete")]
        public ActionResult Delete2(string aircode)
        {
            using (var DB = new DbConnection())
            {
                DB.InsertData("delete from Airlines where airlinecode = '" + aircode + "'");
            }
            return RedirectToAction("Index");
        }

    }
}