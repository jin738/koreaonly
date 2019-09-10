using KoreaOnly.Models;
using Newtonsoft.Json;
using SACS.Library.SabreSoapApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;


namespace KoreaOnly.Controllers
{
    public class IndexController : Controller
    {
        // GET: Index

        public ActionResult Index()
        {
			//Enable Security
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            MainController.OTA_AIRLOWFARESEARCH_Response = null;
            Session["ViewModel"] = null;

            ViewBag.Error = Session["Error"];
            Session["Error"] = "";

            return View();
 
        }



        public ActionResult ViewLogs(string flush = "*", string Activate = "*", string TestXML = "*")
        {
            if (flush == "y")
            {
                MainController.ClearLog();
                return RedirectToAction("ViewLogs");
            }

            if (Activate == "y")
            {
                MainController.CasheCheck = !MainController.CasheCheck;
                return RedirectToAction("ViewLogs");
            }

            if (TestXML != "*")
            {
                System.IO.File.WriteAllText(Server.MapPath("~/Content/SearchTemplate.Fltemp"), TestXML);
            }

            ViewBag.XML = MainController.ReadLog();
            return View();
        }


        public ActionResult SearchFlight(SoapWorkflowPostRQ requestModel)
        {
            var Control = new MainController();

			
             requestModel.DestinationAirportCode = (from code in requestModel.DestinationAirportCode
                                                   where code != ""
                                                   select code.Substring(0, 3).ToString()).ToArray();

            requestModel.OriginAirportCode = (from code in requestModel.OriginAirportCode
                                              where code != ""
                                              select code.Substring(0, 3).ToString()).ToArray();

            requestModel.DepartureDate = (from deptDate in requestModel.DepartureDate
                                          where deptDate.ToString("ddMMyyyy") != "01010001"
                                          select deptDate).ToArray();


            if (requestModel.triptype == "0")
            {
                var li1 = requestModel.DestinationAirportCode.ToList();
                li1.Add(requestModel.OriginAirportCode.First());
                requestModel.DestinationAirportCode = li1.ToArray();

                var li2 = requestModel.OriginAirportCode.ToList();
                li2.Add(requestModel.DestinationAirportCode.First());
                requestModel.OriginAirportCode = li2.ToArray();

                var li3 = requestModel.DepartureDate.ToList();
                li3.Add(requestModel.arrivedate);
                requestModel.DepartureDate = li3.ToArray();
            }





            return View();
        }

        [HttpPost]
        [ActionName("LoadAllFlights")]
        public ActionResult LoadAllData()
        {
            if (Session["FSearch"] != null)
            {
                var MList = (List<SearchFlightResult>)Session["FSearch"];

                Session["FSearch"] = null;

                return View(MList);
            }
            try
            {
                var Control = new MainController();


                if (Flights.Count != 0)
                {
                    if (MainController.AllowRecoding)
                    {
                        WriteFlights(ResFinderFlight);
                        MainController.AllowRecoding = false;
                    }

                    List<double> Price = new List<double>();
                    List<string> stop1Price = new List<string>();
                    List<string> stop2Price = new List<string>();
                    List<string> stop3Price = new List<string>();



                    foreach (var fl in Flights)
                    {
                        Price.Add(Convert.ToDouble(fl.Total_Price));
                    }


                    Session["SearchFillter"] = new SearchFillter()
                    {
                        Stop1 = "on",
                        Stop2 = "on",
                        Stop3 = "on",
                        Airlines = SearchAirlines,
                        Stop1MinPrice = stop1Price,
                        Stop2MinPrice = stop2Price,
                        Stop3MinPrice = stop3Price,
                        PriceFillter = new string[] { Convert.ToDouble(Price.Min()).ToString("0.00"), Convert.ToDouble(Price.Max()).ToString("0.00"), Convert.ToDouble(Price.Min()).ToString("0.00"), Convert.ToDouble(Price.Max()).ToString("0.00") }
                    };


                    return PartialView("_ViewAllFlights", Flights);
                }


                return null;

            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                Session["Error"] = ex.Message;
                Session["ErrorDate"] = DateTime.Now;
                MainController.WriteLog("---------Exception-----------");
                MainController.WriteLog(ex.Message + " LINE NUMBER  " + line + Environment.NewLine + "--------Complete Exception-----------" + "" + ex.InnerException + "" + Environment.NewLine + "" + ex.StackTrace);
                return null;
            }

        }

        public ActionResult GetCheapestFlights()
        {
            return PartialView("_CheapFlights");
        }

        public ActionResult GetSearchFlightsPanel()
        {
            return PartialView("_SearchFillter");
        }

		public ActionResult GetCheapFlights()
        {
            using (var DB = new DbConnection())
            {
                var Res = DB.GetResult<CheapFlights>("Select * from CheapFlights where isactive = 1 ", null);

                if (Res != null)
                    return PartialView("_cheapflightspanel", Res);
                return PartialView("_cheapflightspanel", new List<CheapFlights>());
            }


        }
		
        public List<SearchFlightResult> GetDistinctFlights(List<SearchFlightResult> FlightsList)
        {
            try
            {
                List<SearchFlightResult> FlightList = new List<SearchFlightResult>();
                List<string> FlightNumber = new List<string>();
                foreach (var FL in FlightsList)
                {
                    string UniFlightCode = "";
                    foreach (var FLDetails in FL.FlightDetails)
                    {

                    }
                    if (!FlightNumber.Contains(UniFlightCode))
                    {
                        FlightList.Add(FL);
                        FlightNumber.Add(UniFlightCode);
                    }

                }

                return FlightList.Where(fl => fl.ProcessNOCOMBINABLE == false).ToList();

            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public Tuple<List<SearchFlightResult>, List<string[]>> CreateViewResults(Additional.SearchFares.OTA_AirLowFareSearchRS ResFinder, SoapWorkflowPostRQ requestModel, List<string[]> _SearchAirlines)
        {
            try
            {
                List<SearchFlightResult> Flights = new List<SearchFlightResult>();

                string NotAlowed = "WN";

                List<string[]> SearchAirlines = _SearchAirlines;

                if (ResFinder != null && ResFinder.ApplicationResults.status == Additional.SearchFares.CompletionCodes.Complete)
                {
                    if (ResFinder.PricedItineraries != null)
                    {
                        var Flght = ResFinder.PricedItineraries;
                        int id = 0;
                        int thisflight = new Random().Next(50000, 90000);
                        foreach (var FlightItem in Flght)
                        {
                            if (NotAlowed.Contains(FlightItem.HeaderInformation.ValidatingCarrier.Code))
                            {
                                continue;
                            }

                            var Searchflight = new SearchFlightResult();

                            if (ResFinder.ApplicationResults.Warning != null)
                            {
                                foreach (var item in ResFinder.ApplicationResults.Warning)
                                {
                                    foreach (var items in item.SystemSpecificResults)
                                    {
                                        if ((items.Message.ToList().Where(x => x.code == "MIP:PROCESS" && x.Value == "NO COMBINABLE FARES FOR CLASS USED")).ToList().Count == 0)
                                        {
                                            Searchflight.ProcessNOCOMBINABLE = true;
                                        }

                                    }

                                }
                            }
                            else if (ResFinder.ApplicationResults.Error != null)
                            {
                                foreach (var item in ResFinder.ApplicationResults.Error)
                                {
                                    foreach (var items in item.SystemSpecificResults)
                                    {
                                        if ((items.Message.ToList().Where(x => x.code == "MIP:PROCESS" && x.Value == "NO COMBINABLE FARES FOR CLASS USED")).ToList().Count == 0)
                                        {
                                            Searchflight.ProcessNOCOMBINABLE = true;
                                        }

                                    }

                                }
                            }


                            int value = 0;
                            foreach (var item in FlightItem.OriginDestinationOption)
                            {
                                //Searchflight.AirlineCode.Add(item.FlightSegment.First().MarketingAirline.Code);


                                Searchflight.Flight_OriginTimeZone = (from ii in item.FlightSegment
                                                                      select ii.OriginTimeZone).ToList();

                                Searchflight.Flight_DestinationTimeZone = (from ii in item.FlightSegment
                                                                           select ii.DestinationTimeZone).ToList();


                                TimeSpan totalTime = TimeSpan.FromMinutes(total_time_Flight);
                                totalTime = totalTime + totalhoursFlightSegment;


                                Searchflight.GetTotalTime.Add(totalTime.TotalHours.ToString(".0").ToString().Split('.')[0].ToString() + "h" + " " + totalTime.Minutes + "m");

                                // Calculate Time  END

                                // Search Flight 
                                bool Ress = true;
                                foreach (var airline in SearchAirlines)
                                {
                                    if (airline[0] == FlightItem.HeaderInformation.ValidatingCarrier.Code)
                                    //if (airline[0] == item.FlightSegment.First().MarketingAirline.Code)
                                    {
                                        Ress = false;
                                        break;
                                    }
                                }


                                // SearchAirlines.Add(new string[] { item.FlightSegment.First().MarketingAirline.Code, "1" }); // apply this code in if condition

                                if (Ress)
                                    SearchAirlines.Add(new string[] { FlightItem.HeaderInformation.ValidatingCarrier.Code, "1" });

                                Searchflight.ID = "FlightREF" + id.ToString() + "" + FlightItem.HeaderInformation.ValidatingCarrier.Code + "" + thisflight + "";

                                var ozflight = (from ozlist in item.FlightSegment
                                                where ozlist.MarketingAirline.Code == "OZ"
                                                select ozlist).ToList();

                                if (ozflight.Count == 0)
                                {
                                    Searchflight.Total_Price = (Convert.ToDouble(FlightItem.TotalAmount.ToString()) + (Convert.ToDouble(Convert.ToInt32(requestModel.trvaud) + (Convert.ToInt32(requestModel.trvchild))) * 5.00)).ToString("0.00");
                                }
                                else
                                {
                                    Searchflight.Total_Price = (Convert.ToDouble(FlightItem.TotalAmount)).ToString("0.00");
                                }

                                value++;
                            }

                            Searchflight.FlightDetails = FlightItem.OriginDestinationOption;
                            Searchflight.PriceType = FlightItem;
                            Flights.Add(Searchflight);

                            value++;

                            id++;
                        }
                    }
                }
                else
                {
                    Session["Error"] = "NO Flight Schedule, Please Change Your Destination";
                    Session["ErrorDate"] = DateTime.Now;
                    return null;
                }


                return new Tuple<List<SearchFlightResult>, List<string[]>>(Flights, SearchAirlines);

            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                Session["Error"] = ex.Message;
                Session["ErrorDate"] = DateTime.Now;
                MainController.WriteLog("---------Exception-----------");
                MainController.WriteLog(ex.Message + " LINE NUMBER  " + line + Environment.NewLine + "--------Complete Exception-----------" + "" + ex.InnerException + "" + Environment.NewLine + "" + ex.StackTrace);
                return null;
            }
        }


        public void WriteFlights(List<Additional.SearchFares.OTA_AirLowFareSearchRS> ListofFlights)
        {
            List<string> Aircode = new List<string>();
            foreach (var flight in ListofFlights)
            {
                if (flight.ApplicationResults.status == Additional.SearchFares.CompletionCodes.Complete && flight.PricedItineraries != null)
                {
                    foreach (var flightP in flight.PricedItineraries)
                    {

                    }
                }
            }

            List<Tuple<string, List<Additional.SearchFares.OTA_AirLowFareSearchRSPricedItinerary>>> Airlines = new List<Tuple<string, List<Additional.SearchFares.OTA_AirLowFareSearchRSPricedItinerary>>>();
            for (int i = 0; i < Aircode.Count; i++)
            {
                foreach (var flight in ListofFlights)
                {
                    if (flight.ApplicationResults.status == Additional.SearchFares.CompletionCodes.Complete && flight.PricedItineraries != null)
                    {
                        var L = (from yt in flight.PricedItineraries
                                 where yt.HeaderInformation.ValidatingCarrier.Code == Aircode[i]
                                 select yt).ToList();




                    }
                }
            }


            foreach (var FLige in Airlines)
            {
                System.IO.File.WriteAllText(HostingEnvironment.MapPath("~") + "/DataFile/FlightLog/" + FLige.Item1 + "-" + FLige.Item2.Count + ".xml", FLige.Item1 + "" + Environment.NewLine + "" + MainController.GetXMLFromObject(FLige.Item2));
            }


        }

        [HttpPost]
        [ActionName("SearchFlight")]
        public ActionResult SearchFlight_Post(string[] Airline, string stop1, string stop2, string stop3, string pricefrom, string priceto, string stamount2, string stamount2val)
        {
            var List = (List<SearchFlightResult>)Session["ViewModel"];

            var pricefromD = Convert.ToDouble(pricefrom.Substring(1));
            var pricetoD = Convert.ToDouble(priceto.Substring(1));

            List<SearchFlightResult> SelectedFlights = new List<SearchFlightResult>();

            foreach (var Flight in List)
            {

            }

            bool checkfeild = true;

            List<SearchFlightResult> SelectedFlights_final = new List<SearchFlightResult>();

            if (stop1?.ToString() == "on" && checkfeild)
            {
                SelectedFlights_final.AddRange((from ii in SelectedFlights
                                                where Convert.ToInt32(ii.StopType) == 0
                                                select ii).ToList());



                if (SelectedFlights.Count != 0)
                {

                }
            }

            if (stop2?.ToString() == "on" && checkfeild)
            {
                SelectedFlights_final.AddRange((from ii in SelectedFlights
                                                where Convert.ToInt32(ii.StopType) == 1
                                                select ii).ToList());


            }

            if (stop3?.ToString() == "on" && checkfeild)
            {
                SelectedFlights_final.AddRange((from ii in SelectedFlights
                                                where Convert.ToInt32(ii.StopType) > 1
                                                select ii).ToList());

                if (SelectedFlights.Count != 0)
                {

                }
            }



            var SearchFillter = (SearchFillter)Session["SearchFillter"];

            SearchFillter.Airlines = (from i in Airline
                                      select new string[] { i.Substring(0, i.Length - 1), i.ToArray().Last().ToString() }).ToList();


            return PartialView("_ViewAllFlights", SelectedFlights_final);
        }

        public ActionResult SelectFlight(string SelectedFlight)
        {
            try
            {
                var Control = new MainController();
                var SelFlightItem = ((List<SearchFlightResult>)Session["ViewModel"]).Find(fl => fl.ID == SelectedFlight);

                var SelectedFlightEn = SelFlightItem.FlightDetails;
                var SelectFlightFare = SelFlightItem.PriceType;

                // changes Made here

                var Responce = Control.SendEnhancedAirBookRequest(SelectedFlightEn);

                if (Responce.ApplicationResults.status == Additional.Enhanced.CompletionCodes.Complete)
                {
                    var Seg = new Additional.OTA_AirRule.OTA_AirRulesRQOriginDestinationInformationFlightSegment()
                    {


                    };

                  List<string> BookCodes = new List<string>();
                        foreach (var BookCode in Responce.OTA_AirPriceRS.PriceQuote.PricedItinerary.AirItineraryPricingInfo)
                        {
                            BookCodes.AddRange(from i in BookCode.FareCalculationBreakdown select i.FareBasis.Code.ToString());
                        }


                        // Change AirRule RQ

                        var Response2 = Control.SendAirRulellSRequest(new Additional.OTA_AirRule.OTA_AirRulesRQOriginDestinationInformation()
                        {
                            FlightSegment = RQ

                        }, BookCodes.Distinct().ToList());


                        foreach (var item in Response2)
                        {
                            if (item.ApplicationResults.status != Additional.OTA_AirRule.CompletionCodes.Complete)
                            {
                               // var SabreRes = new MainController().SendSabreCommand("WPRD*");
                            }
                        }
                        
                        Session["AirRulles"] = Response2;
                }
                else
                {
                    // Error 
                }

                return RedirectToAction("ContactInfo");
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                Session["Error"] = ex.Message;
                Session["ErrorDate"] = DateTime.Now;
                MainController.WriteLog("---------Exception-----------");
                MainController.WriteLog(ex.Message + " LINE NUMBER  " + line + Environment.NewLine + "--------Complete Exception-----------" + "" + ex.InnerException + "" + Environment.NewLine + "" + ex.StackTrace);
                return RedirectToAction("Index");
            }
        }

        public ActionResult ContactInfo()
        {
            try
            {
                var Enhance = (Additional.Enhanced.EnhancedAirBookRS)Session["FlightReserve"];

                List<ContactInfoModel> List = new List<ContactInfoModel>();

                var Flight = Enhance.TravelItineraryReadRS.TravelItinerary.ItineraryInfo.ReservationItems;

                for (int i = 0; i < Flight.Length; i++)
                {
                    List.Add(new ContactInfoModel()
                    {
                        From = Flight[i].FlightSegment.First().OriginLocation.LocationCode,
                        TO = Flight[i].FlightSegment.First().DestinationLocation.LocationCode,
                        Take_Off_DateTime = Flight[i].FlightSegment.First().DepartureDateTime.ToString(),
                        FlightNo = Flight[i].FlightSegment.First().FlightNumber,
                        Operating_Airline = Flight[i]?.FlightSegment?.First()?.OperatingAirline?.First()?.Code,
                        Operating_Airline_CompanyName = Flight[i]?.FlightSegment?.First()?.OperatingAirline?.First()?.CompanyShortName,
                        Marketing_Airline = Flight[i]?.FlightSegment?.First()?.MarketingAirline?.Code,
                        Take_Off_Location_Terminal = Flight[i].FlightSegment.First().OriginLocation.Terminal == "" ? "Not Defined" : Flight[i].FlightSegment.First().OriginLocation.Terminal
                    });

                }

                var FareList = (Additional.SearchFares.OTA_AirLowFareSearchRSPricedItinerary)Session["FlightFareView"];

                List<FarePrice> FarePrice = new List<FarePrice>();

                int ii = 0;

                foreach (var Price in FareList.AirItineraryPricingInfo)
                {
                    double Tax = 0;

                    for (int i = 0; i < Price.ItinTotalFare.Taxes.Length; i++)
                    {
                        Tax += Convert.ToDouble(Price.ItinTotalFare.Taxes[i].Amount.ToString());
                    }

                    var Lis = ((List<string[]>)Session["Traveller"])[ii];

                    FarePrice.Add(new FarePrice()
                    {
                        NoOfPassenger = Price.PassengerTypeQuantity.Quantity,
                        BaseFare = (Convert.ToDouble(Price.ItinTotalFare.BaseFare.Amount) + 5.00).ToString("0.00"),
                        Tax = Tax.ToString("0.00")
                    });

                    ii++;
                }

                Session["Prices"] = FarePrice;

                var Bagg = Enhance.OTA_AirPriceRS?.PriceQuote?.PricedItinerary?.AirItineraryPricingInfo?.First().ItinTotalFare?.BaggageInfo?.US_DOT_Disclosure;

                if (Bagg != null && Bagg?.Length != 0)
                {
                    Session["BagDetails"] = Bagg;
                }
                else
                {
                    Session["BagDetails"] = new string[] { "Baggage Details Not Found" };
                }

                return View(List);
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                Session["Error"] = ex.Message;
                Session["ErrorDate"] = DateTime.Now;
                MainController.WriteLog("---------Exception-----------");
                MainController.WriteLog(ex.Message + " LINE NUMBER  " + line + Environment.NewLine + "--------Complete Exception-----------" + "" + ex.InnerException + "" + Environment.NewLine + "" + ex.StackTrace);
                return RedirectToAction("Index");
            }
        }

        public string GetAllAirlines(string AirCode = "*")
        {
            try
            {
                var DD = (List<Airlines>)Session["AllAirline"];
                if (DD == null)
                {
                    MainController.GetAirline();
                    DD = (List<Airlines>)Session["AllAirline"];
                }

                List<object> List = new List<object>();

                List = (from u in DD
                        where u.AirlineCode == AirCode
                        select (object)new { name = u.AirlineCode + " " + u.AirlineName }).ToList();

                if (List == null || List.Count() == 0)
                {
                    List = (from u in DD
                            where u.AirlineCode.Contains(AirCode) || u.AirlineName.Contains(AirCode)
                            select (object)new { name = u.AirlineCode + " " + u.AirlineName }).ToList();
                }

                string JS = JsonConvert.SerializeObject(List.ToArray());

                return JS;

            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MainController.WriteLog(ex.Message + " LINE NUMBER  " + line + Environment.NewLine + "--------Complete Exception-----------" + "" + ex.InnerException + "" + Environment.NewLine + "" + ex.StackTrace);
                string[] Airports = new string[] { "LAX", "MEX", "SAN" };

                string JS = JsonConvert.SerializeObject(Airports);

                return JS;
            }
        }


		public class AllowJsonGetAttribute : ActionFilterAttribute
        {
            public override void OnResultExecuting(ResultExecutingContext filterContext)
            {
                var jsonResult = filterContext.Result as JsonResult;

                if (jsonResult == null)
                    throw new ArgumentException("Action does not return a JsonResult, attribute AllowJsonGet is not allowed");


                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                base.OnResultExecuting(filterContext);
            }
        }
		
		// Make Offline
        [HttpGet]
        [AllowJsonGet]
        public JsonResult GetAllAirports(string AirCode = "*")
        {
            ViewBag.SyncType = "Asynchronous";

            try
            {
                var RR = System.IO.File.ReadAllText(HostingEnvironment.MapPath("/Content/AIRPORTCODE.json"));
                AirCode = AirCode.ToUpper();
                var R = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<AirportOffline>>(RR);


                var SList = (from u in R
                             where u.name.ToUpper().Contains(AirCode)
                             select u).ToList();

                if (SList == null)
                {
                    return Json(new { });
                }



                var resultWithCode = new List<AirportOffline>(SList.Where(word => word.name.ToUpper().StartsWith("(" + AirCode + ")"))?.OrderBy(word => word));

                var EE = SList.Where(word => word.name.ToString().ToUpper().Contains("USA"))?.ToList();

                var result = new List<AirportOffline>(EE.OrderBy(w => w.name));

                if (resultWithCode == null)
                {
                    resultWithCode = new List<AirportOffline>();
                }

                if (result == null)
                {
                    result = new List<AirportOffline>();
                }

                result.AddRange(SList.Except(result).ToList());


                resultWithCode.AddRange(result.Except(resultWithCode).ToList());


                return Json(resultWithCode.ToArray());
            }
            catch (Exception ex)
            {
                return Json(new { });
            }

        }
		
        public string PayNow(Models.PassengerDetailsRQ Info)
        {
            var Enhance = (Additional.Enhanced.EnhancedAirBookRS)Session["FlightReserve"];

            Info.Airline = (from i in Enhance.OTA_AirBookRS.OriginDestinationOption select i.MarketingAirline.Code).Distinct().ToArray();

		    
            var Response = new MainController().SendPassengerDetailsRequest(Info);
            if (Response == null)
            {
                return "{\"Code\": \"104\", \"Message\":\"Fill Required Feilds\"}";
            }
            else if (Response.ApplicationResults.status == Additional.Passengers.CompletionCodes.Complete)
            {
                Session["UniqueID"] = Response?.TravelItineraryReadRS?.TravelItinerary.ItineraryRef?.ID?.ToString() == "" ? "" : Response?.TravelItineraryReadRS?.TravelItinerary.ItineraryRef?.ID;
                var responce = (object[])new MainController().SendAllCommandFlow(Info);

				
            var Request = new Additional.CreditVerification.CreditVerificationRQ()
            {
                Version = "2.2.0",
                Credit = new Additional.CreditVerification.CreditVerificationRQCredit()
                {
                    CC_Info = new Additional.CreditVerification.CreditVerificationRQCreditCC_Info()
                    {
                        PaymentCard = new Additional.CreditVerification.CreditVerificationRQCreditCC_InfoPaymentCard()
                        {
                            AirlineCode = Payment.Airline.First(),
                            Code = Payment.CardType,
                            ExpireDate = Payment.CardExpY + "-" + Payment.CardExpM,
                            Number = Payment.CardNumber.Replace("-", "")
                        }
                    },
                    ItinTotalFare = new Additional.CreditVerification.CreditVerificationRQCreditItinTotalFare()
                    {
                        TotalFare = new Additional.CreditVerification.CreditVerificationRQCreditItinTotalFareTotalFare()
                        {
                            Amount = totalamount.ToString("0.00") + "",
                            CurrencyCode = "USD"
                        }
                    }
                }
            };
				
				
				
                if (responce[0].ToString() == "2")
                {
                    return "{\"Code\": \"100\", \"Message\":\"Enter Correct Card Number\"}";
                }

            }

            return "{\"Code\": \"200\", \"Message\":\"Success\"}";
        }


        public void SetSession(string Key, string Value)
        {
            Session[Key] = Value;
        }

    }
}