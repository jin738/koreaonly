using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KoreaOnly.Models
{
    public class PassengerDetailsRQ
    {   
        public string DeviceType { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public string[] TrvTitle { get; set; }
        public string[] TrvFName { get; set; }
        public string[] TrvMName { get; set; }
        public string[] TrvLName { get; set; }
        public string[] TrvDOB { get; set; }
        public string[] TrvType { get; set; }


        public string[] FrequentFlyer { get; set; }
        public string[] Airline { get; set; }


        // Payment

        public string BillFName { get; set; }
        public string BillMName { get; set; }
        public string BillLName { get; set; }

        public string BillAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }

        public string CardType { get; set; }
        public string CardNumber { get; set; }

        public string CardExpM { get; set; }
        public string CardExpY { get; set; }
        public string CSV { get; set; }

    }
}