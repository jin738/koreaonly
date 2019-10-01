using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KoreaOnly.Models
{
    public class TicketsModel
    {
        public string TripFromDate { get; set; }
        public string TripToDate { get; set; }

        public string TripTo { get; set; }

        public string AirlineReservationCode { get; set; }
        public string AirlineCode { get; set; }
        public List<TicketsPersonInfo> Passengers { get; set; }
        public List<TicketFligtInfo> TicketFligtInfos { get; set; }
    }

    public class TicketsPersonInfo
    {
        public string Name { get; set; }

        public string ReservationCode { get; set; }

        public string Seats { get; set; } = "Check-In Required";

        public string AirlineReservationCode { get; set; }
        public string AirlineCode { get; set; }

        //   public List<TicketFligtInfo> TicketFligtInfos { get; set; }

    }

    public class TicketFligtInfo
    {
        public string DepartureDate { get; set; }
        public string ArrivalDate { get; set; }

        public string AirlineName { get; set; }
        /// <summary>
        /// FlightCode + Flight Number
        /// </summary>
        public string FlightNumber { get; set; }

        public string Duration { get; set; }
        public string Class { get; set; }
        public string Status { get; set; }


        public string AirportFromCode { get; set; }
        public string AirportFrom { get; set; }

        public string AirportToCode { get; set; }
        public string AirportTo { get; set; }

        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }

        public string DepartureTerminal { get; set; } = "Not Available";
        public string ArrivalTerminal { get; set; } = "Not Available";


        public string AircraftName { get; set; }
        public string Distance { get; set; }

        public string Stops { get; set; }


        public string Meals { get; set; }

        public string PassengerName { get; set; }
        //  public string Seats { get; set; } = "Check-In Required";

    }

}