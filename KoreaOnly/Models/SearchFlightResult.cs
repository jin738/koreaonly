using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KoreaOnly.Models
{
    public class SearchFlightResult
    {
        public SearchFlightResult()
        {

        }

        public string ID { get; set; }

        public List<string> AirlineCode { get; set; } = new List<string>();

        public string GetDeparture_Airline(int index)
        {
            return (AirlineCode.Count == 0 ? "" : MainController.GetAirline(AirlineCode[index]).AirlineName);
        }
        
        

        public bool ProcessNOCOMBINABLE { get; set; } = false;

        //public List<string> Airline_Code { get; set; }

        public List<string> Stop_Count { get; set; } = new List<string>();

        public List<string> Flight_From { get; set; } = new List<string>();
        public List<string> Flight_To { get; set; } = new List<string>();

        public List<string> Flight_From_Time { get; set; } = new List<string>();
        public List<string> Flight_To_Time { get; set; } = new List<string>();

        public List<string> Flight_OriginTimeZone { get; set; } = new List<string>();
        public List<string> Flight_DestinationTimeZone { get; set; } = new List<string>();

        public List<string> GetTotalTime { get; set; } = new List<string>();

        public string GetTotal_Time(int index)
        {

            if (Flight_To_Time != null && Flight_From_Time != null && Flight_To_Time.Count != 0 && Flight_From_Time.Count != 0)
            {
                var date = (Convert.ToDateTime(Flight_To_Time[index].ToString()) - (Convert.ToDateTime(Flight_From_Time[index].ToString())));

                var timezone = (Convert.ToInt32(Flight_DestinationTimeZone[index]) - Convert.ToInt32(Flight_OriginTimeZone[index]));

                int hours = date.Hours - (timezone);

                return hours + "h " + Math.Abs(date.Minutes) + "m";
            }
            else
            {
                return "";
            }

        }

        public Additional.SearchFares.OTA_AirLowFareSearchRSPricedItinerary PriceType { get; set; }
        public Additional.SearchFares.OTA_AirLowFareSearchRSPricedItineraryOriginDestinationOption[] FlightDetails { get; set; }

        public string Total_Price { get; set; }
        public string Is_Return_Flight { get; set; }
        public string Airline_Total_Stop_Count
        {
            get
            {
                if (Stop_Count != null && Stop_Count.Count != 0)
                {
                    int Total = 0;

                    foreach (var stop in Stop_Count)
                    {
                        Total += Convert.ToInt32(stop);
                    }

                    return (Total).ToString();
                }
                else
                {
                    return "";
                }

            }
        }

        public string StopType
        {
            get
            {
                int stoptype = 0;

                if (Stop_Count != null && Stop_Count.Count != 0)
                {
                    foreach (var stop in Stop_Count)
                    {
                        int Total = Convert.ToInt32(stop);
                        stoptype = Total > stoptype ? Total : stoptype;
                    }

                    return (stoptype).ToString();
                }
                else
                {
                    return "0";
                }

            }
        }


    }
}