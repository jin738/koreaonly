using System;
using SACS.Library.Activities.InputData;

namespace KoreaOnly.Models
{
    /// <summary>
    /// The POST request from the BargainFinderMax form.
    /// </summary>
    public class BargainFinderMaxPostRQ : IBargainFinderMaxData
    {
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }

        public string TripType { get; set; }
        public string OriginAirportCode { get; set; }
        public string DestinationAirportCode { get; set; }

        public int NumberOfChild { get; set; }
        public int NumberOfAdult { get; set; }

        // Chlid + Adult 
        public int NumberOfPassengers { get; set; }
        public string RequestType { get; set; }

        public string RPH { get; set; }
        public string RequestorID { get; set; }
        public string RequestorType { get; set; }
        public string RequestorCompanyCode { get; set; }
        public string PassengerTypeCode { get; set; }

    }

}