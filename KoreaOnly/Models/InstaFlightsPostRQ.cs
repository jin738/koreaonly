﻿using System;
using SACS.Library.Activities.InputData;

namespace KoreaOnly.Models
{
    public class InstaFlightsPostRQ : IInstaFlightsData
    {
        public string Origin { get; set; }

        public string Destination { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime ReturnDate { get; set; }
    }
}