using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KoreaOnly.Models
{
    public class Invoices
    {
        public string IID { get; set; }
        public string IInvoiceNo { get; set; }
        public string ISabrePNR { get; set; }
        public string IAirlinePNR { get; set; }
        public string IDeparture { get; set; }
        public string IArrival { get; set; }
        public string IFareAmount { get; set; }
        public string IEmail { get; set; }
        public string IPhoneNo { get; set; }
        public string IAddress { get; set; }
        public string xDatetime { get; set; }
    }
}