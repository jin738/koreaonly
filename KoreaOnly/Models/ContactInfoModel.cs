using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KoreaOnly.Models
{
    public class ContactInfoModel
    {

        // Time 
        public string Flight_Duration { get; set; }


        // Departure
        public string From { get; set; }
        public string TO { get; set; }
        public string Take_Off_DateTime { get; set; }
        public string Take_Off_TimeZone { get; set; }
        public string FlightNo { get; set; }


        public string Landing_DateTime { get; set; }
        public string Landing_TimeZone { get; set; }

        public string Take_Off_Location_Airport
        {
            get
            {
                return From.ToString() != "" ? MainController.GetAirport(From).Result.AirportLocation : "";
            }
        }

        public string Take_Off_Location_Terminal { get; set; }
        public string Take_Off_Location_City
        {
            get
            {
                return From.ToString() != "" ? "" : "";
            }
        }


        public string Landing_Location_Airport
        {
            get
            {
                return TO.ToString() != "" ? MainController.GetAirport(TO).Result.AirportLocation : "";
            }
        }
        public string Landing_Location_City
        {
            get
            {
                return TO.ToString() != "" ? "" : "";
            }
        }
        public string Landing_Location_Terminal { get; set; }



        public string Mix_Duration
        {
            get
            {
                if (Take_Off_DateTime?.ToString() != "" && Take_Off_TimeZone?.ToString() != "" && Landing_DateTime?.ToString() != "" && Landing_TimeZone?.ToString() != "")
                {
                    var de1_ = MainController.GetDateTimeWithTimezone(Landing_TimeZone, Landing_DateTime);
                    var de2_ = MainController.GetDateTimeWithTimezone(Take_Off_TimeZone, Take_Off_DateTime);

                    var TotalTime = de1_ - de2_;

                    return TotalTime.Hours + "h " + TotalTime.Minutes + "m ";
                }

                return "";
            }
        }

        public string Equipment_Type { get; set; }
        public string Air_Miles { get; set; }
        public string Marketing_Airline { get; set; }
        public string Operating_Airline { get; set; }
        public string Operating_Airline_CompanyName { get; set; }
    }
}