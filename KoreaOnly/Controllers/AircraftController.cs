using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KoreaOnly.Controllers
{
    [RoutePrefix("SystemMaster/Aircraft")]
    public class AircraftController : Controller
    {
        [Route("")]
        [Route("SystemMaster/Aircraft/Index")]
        // GET: Aircraft
        public ActionResult Index()
        {
            if (MainController.checkAdminLogin())
            {
                MainController.GetAircraftType();
                var L = (List<AircraftType>)Session["AircraftType"];

                return View(L);
            }

            return Redirect("/SystemMaster/");

        }

        [Route("SystemMaster/Aircraft/Index")]
        [HttpPost]
        public ActionResult Index(string txtSearch = "")
        {
            MainController.GetAirline();
            var L = (List<AircraftType>)Session["AircraftType"];

            return View(txtSearch == "" ? L : L.Where(x => x.Code == txtSearch).ToList());
        }


        [Route("SystemMaster/Aircraft/Create")]
        public ActionResult Create()
        {
            if (MainController.checkAdminLogin())
            {
                return View();
            }
            return Redirect("/SystemMaster/");
        }

        [Route("SystemMaster/Aircraft/Create")]
        [HttpPost]
        public ActionResult Create(AircraftType Aircraft)
        {
            using (var DB = new DbConnection())
            {
                DB.InsertData("insert into aircrafttype (code,type,Description) values ('" + Aircraft.Code + "','" + Aircraft.Type + "','" + Aircraft.Description + "')");
            }

            return RedirectToAction("Index");
        }


        [Route("SystemMaster/Aircraft/Edit")]
        public ActionResult Edit(string aircode)
        {
            if (MainController.checkAdminLogin())
            {
                MainController.GetAircraftType();
                var airline = MainController.GetAircraftTypeByCode(aircode);
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

        [Route("SystemMaster/Aircraft/Edit")]
        [HttpPost]
        public ActionResult Edit(AircraftType EdAircraft)
        {
            using (var DB = new DbConnection())
            {
                DB.InsertData("update aircrafttype set code = '" + EdAircraft.Code + "', type = '" + EdAircraft.Type + "', Description = '" + EdAircraft.Description + "' where id = " + EdAircraft.ID + "");
            }
            return RedirectToAction("Index");
        }


        [Route("SystemMaster/Aircraft/Delete")]
        public ActionResult Delete(string aircode)
        {
            if (MainController.checkAdminLogin())
            {
                var airline = MainController.GetAircraftTypeByCode(aircode);
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
        [Route("SystemMaster/Aircraft/Delete")]
        public ActionResult Delete2(string aircode)
        {
            using (var DB = new DbConnection())
            {
                DB.InsertData("delete from aircrafttype where id = " + aircode + "");
            }
            return RedirectToAction("Index");
        }

    }
}