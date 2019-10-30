using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using static KoreaOnly.Controllers.IndexController;

namespace KoreaOnly.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class AsyncController : Controller
    {

        [OutputCache(Duration = 3600, VaryByParam = "filename;W;H")]
        public ActionResult Thumbnail(string filename, int W, int H)
        {
            var img = new WebImage(filename)
                .Resize(W, H, false, true);
            return new ImageResult(new MemoryStream(img.GetBytes()), "binary/octet-stream");
        }

        // GET: Async
        public ActionResult Index()
        {
            return View();
        }


       // [OutputCache(Duration = 60 * 1, VaryByParam = "location")]
        public ActionResult GetRecommendedFlights(Location location)
        {
            try
            {
                var r = location;// JsonConvert.DeserializeObject<Location>(responseString);
                //System.Web.Script.Serialization.JavaScriptSerializer


                var R = new List<string[]>();

                using (var DB = new DbConnection())
                {
                    string LoadQ = System.IO.File.ReadAllText(Server.MapPath("~/Security/CheapFlightQ.txt"));

                    LoadQ = LoadQ.Replace("Pram_Country", r.country).Replace("Pram_City", r.city).Replace("Pram_Region", r.region).Replace("Pram_TimeZ", r.timezone.Split('/')[1]);

                    //var Re = DB.GetResult<Airports>($"Declare @Code varchar(20); Declare @Country varchar(20); select @Country = CountryName from CountryList where CountryCode like '{r.country}%';  Select top 1 @Code = AirportCode from Airport_new Where AirportFullName like '%'+@country+ '%'  and (AirportFullName like '%{r.city}%' and AirportFullName like '%{r.region}%'  or AirportFullName like '%{r.timezone.Split('/')[1]}%') Select * from Airport_new where AirportCode in ( Select distinct top 12 FIDepartureCode as Code From flightinfo where FIDepartureCode <> @Code union Select distinct top 12 FIDestinationCode as Code From flightinfo where FIDestinationCode <> @Code) union select AirportCode, AirportLocation,ID, '__' as AirportFullName  from airport_new where AirportCode = @Code");
                    var Re = DB.GetResult<Airports>(LoadQ);
                    var Ld = Re.Where(d => d.AirportFullName == "__").FirstOrDefault();



                    var X = Parallel.ForEach(Re.Where(d => d.AirportFullName != "__"), fl =>
                    {
                        R.Add(new string[] {
                                fl.AirportLocation.Split(',')[0],
                                Ld.AirportCode,
                                fl.AirportCode,
                                Ld.AirportLocation.Split(',')[0] + "-to-" + fl.AirportLocation.Split(',')[0],
                                fl.ID
                          });


                    });

                    while (!X.IsCompleted)
                    {

                    }

                    var Xc = new List<string[]>();
                    foreach (var EL in R)
                    {
                        var ff = Xc.FindIndex(x => x[0] == EL[0]);
                        if (ff == -1)
                            Xc.Add(EL);
                        else
                        {
                            if (Convert.ToInt32(Xc[ff][4]) < Convert.ToInt32(EL[4]))
                            {
                                Xc[ff] = EL;
                            }
                        }
                    }

                    R = Xc;


                }



                return PartialView("/Views/Index/RECMDFLights.cshtml", R);
            }
            catch (Exception ex)
            {
                MainController.WriteException(ex);
            }

            return PartialView("/Views/Index/RECMDFLights.cshtml", new List<string[]>());
        }


        [HttpPost]
        //[OutputCache(Duration = 60 * 2, VaryByParam = "OR;DS")]
        public ActionResult LoadRoutedFlights(string OR, string DS)
        {

            string Q = $"Select * from Flightinfo where convert(Date,FIxdatetime) > convert(date,getdate() -7) and convert(Date,left(FIDepartureDatetime,10)) > convert(date,getdate() -3) and  FIDepartureCode = '{OR.Replace("'", "")}' and FIDestinationCode = '{DS.Replace("'", "")}'; ";

            var l = new List<FlightInfo>();

            using (var DB = new DbConnection())
            {
                l = DB.GetResult<FlightInfo>(Q);
            }

            var L = new List<FlightInfo>();

            foreach (var C in l)
            {
                var index = L.FindIndex(de => de.FIAirlineCode == C.FIAirlineCode);

                if (index == -1)
                {
                    L.Add(C);
                }
                else
                {
                    // Price Checked

                    if (Convert.ToDouble(L[index].FIPrice) > Convert.ToDouble(C.FIPrice))
                    {
                        L[index] = C;
                    }
                }

            }



            return PartialView("/Views/Index/PricedFlights.cshtml", L);
        }

    }
}