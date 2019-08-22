using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace KoreaOnly.Controllers
{
    [RoutePrefix("SystemMaster/Airport")]
    public class AirportController : Controller
    {
        // GET: Airport
        [Route("")]
        [Route("SystemMaster/Airport/Index")]
        public ActionResult Index()
        {
            if (MainController.checkAdminLogin())
            {
                //MainController.WriteAirportSQl();
                WriteAirportOffline();
                MainController.GetAirport(true);
                var L = (List<Airports>)Session["AllAirports"];

                return View(L);
            }

            return Redirect("/SystemMaster/");
        }

        [Route("SystemMaster/Airport/Create")]
        public ActionResult Create()
        {
            if (MainController.checkAdminLogin())
            {
                return View();
            }

            return Redirect("/SystemMaster/");
        }

        [Route("SystemMaster/Airport/Create")]
        [HttpPost]
        public ActionResult Create(Airports Aiport)
        {
            using (var DB = new DbConnection())
            {
                DB.InsertData("insert into airports (airportcode,airportlocation,airportfullname) values ('" + Aiport.AirportCode + "','" + Aiport.AirportLocation + "','" + ($"({Aiport.AirportCode}) {Aiport.AirportLocation} - {Aiport.AirportFullName}") + "')");
                WriteAirportOffline();
            }

            return RedirectToAction("Index");
        }

        public void WriteAirportOffline()
        {
            using (var DB = new DbConnection())
            {
                DB.GetResult<Airports>("Select * From Airports", "AllAirports");

                var Airports = (List<Airports>)Session["AllAirports"];

                System.IO.File.WriteAllText(HostingEnvironment.MapPath("/Content/AIRPORTCODE.json"), (new JavaScriptSerializer().Serialize(Airports)).ToString());
            }
        }

        [Route("SystemMaster/Airport/Edit")]
        public ActionResult Edit(string AirportCode)
        {
            if (MainController.checkAdminLogin())
            {
                var L = MainController.GetAirport(AirportCode);

                return View(L);
            }

            return Redirect("/SystemMaster/");
        }

        [Route("SystemMaster/Airport/Edit")]
        [HttpPost]
        public ActionResult Edit(Airports Aiport)
        {
            using (var DB = new DbConnection())
            {
                DB.InsertData("update airports set airportcode = '" + Aiport.AirportCode + "' ,airportlocation = '" + Aiport.AirportLocation + "' ,airportfullname= '" + ($"({Aiport.AirportFullName}") + "' where airportcode = '" + Aiport.AirportCode + "'");
            }
            return RedirectToAction("Index");
        }

        [Route("SystemMaster/Airport/Delete")]
        public ActionResult Delete(string AirportCode)
        {
            if (MainController.checkAdminLogin())
            {
                var L = MainController.GetAirport(AirportCode);

                return View(L);
            }

            return Redirect("/SystemMaster/");
        }

        [Route("SystemMaster/Airport/Delete")]
        [HttpPost]
        public ActionResult Delete2(string airportCode)
        {
            using (var DB = new DbConnection())
            {
                DB.InsertData("delete from airports where airportcode = '" + airportCode + "'");
            }
            return RedirectToAction("Index");
        }

    }
}