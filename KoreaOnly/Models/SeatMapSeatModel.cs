using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KoreaOnly.Models
{
    public class SeatMapSeatModel
    {
        public Additional.EnhancedSeatMap.Seat_Air Seats { get; set; }
        public string RowNumber { get; set; }
        public string SeatNumner { get; set; }
        public string SeatLocation { get; set; }


        public bool IsDisabledSeats { get; set; }
        public bool isNewSeats { get; set; } = false;
        public string isWindowSeats { get; set; } = "";

        public string SeatPrice { get; set; } = "0.00";
    }

    public class SeatingArragment
    {
        public int TotalSeats { get; set; }
        public int SpecialPadding { get; set; }

        public List<int> SeatsBreaks { get; set; }

    }

}