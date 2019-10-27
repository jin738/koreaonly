using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KoreaOnly.Controllers
{
    [RoutePrefix("SystemMaster/CheapFlights")]
    public class CheapFlightsController : Controller
    {
        [Route("")]
        [Route("SystemMaster/CheapFlights/Index")]
        // GET: CheapFlights
        public ActionResult Index()
        {
            if (!MainController.checkAdminLogin())
            {
                return Redirect("/SystemMaster/");
            }

            using (var DB = new DbConnection())
            {
                var res = DB.GetResult<CheapFlights>("Select * from CheapFlights Order by ID desc", null);

                if (res != null)
                    return View(res);

                return View(new List<CheapFlights>());
            }


        }

        [Route("SystemMaster/CheapFlights/Create")]
        public ActionResult Create()
        {
            return View();
        }

        [Route("SystemMaster/CheapFlights/Update")]
        public ActionResult Update(string ID)
        {
            using (var DB = new DbConnection())
            {
                var res = DB.GetResult<CheapFlights>($"Select * from CheapFlights WHERE ID = {ID} Order by ID desc", null);

                if (res != null)
                    return View(res.First());


            }
            return RedirectToAction("index");
        }

        [Route("SystemMaster/CheapFlights/Update")]
        [HttpPost]
        public ActionResult Update(CheapFlights cheapFlights)
        {
            string Q = $"Update CheapFlights set AirportCode = '{cheapFlights.AirportCode}', DepartureCode = '{cheapFlights.DepartureCode}', Price= '{cheapFlights.Price}', AirlineCode = '{cheapFlights.AirlineCode}', DepartureDate = '{cheapFlights.DepartureDate}', DeptShortName = '{cheapFlights.DeptShortName}', ArrvShortName = '{cheapFlights.ArrvShortName}'  where ID = {cheapFlights.ID.Replace("'", "")}";
            using (var db = new DbConnection())
            {
                var DB = db.InsertData(Q);
            }

            return RedirectToAction("index");
        }

        [Route("SystemMaster/CheapFlights/Mark")]
        public ActionResult Mark(string ID, string currentstatus)
        {
            using (var db = new DbConnection())
            {
                var DB = db.InsertData($"update CheapFlights set isactive = {(currentstatus == "True" ? "0" : "1")} where ID = {ID.Replace("'", "").ToString()}");
            }

            return RedirectToAction("index");
        }

        [Route("SystemMaster/CheapFlights/Delete")]
        public ActionResult Delete(string ID)
        {
            using (var db = new DbConnection())
            {
                var DB = db.InsertData($"Delete From CheapFlights where ID = {ID.Replace("'", "").ToString()}");
            }

            return RedirectToAction("index");
        }

        [Route("SystemMaster/CheapFlights/Create")]
        [HttpPost]
        public ActionResult Create(CheapFlights cheapFlights)
        {
            using (var db = new DbConnection())
            {
                var res = db.InsertData($"Insert Into CheapFlights (AirportCode,DepartureCode,Price,AirlineCode,DepartureDate, DeptShortName,ArrvShortName ) values ('{cheapFlights.AirportCode}','{cheapFlights.DepartureCode}','{cheapFlights.Price}','{cheapFlights.AirlineCode}','{cheapFlights.DepartureDate}', '{cheapFlights.DeptShortName}', '{cheapFlights.ArrvShortName}')");
            }


            return RedirectToAction("index");
        }

    }
}