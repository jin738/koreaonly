using System.Collections.Generic;

namespace KoreaOnly.Models
{
    public class SearchFillter
    {
        public List<string[]> Airlines { get; set; }

        public string Stop1 { get; set; }
        public string Stop2 { get; set; }
        public string Stop3 { get; set; }

        public List<string> Stop1MinPrice { get; set; }
        public List<string> Stop2MinPrice { get; set; }
        public List<string> Stop3MinPrice { get; set; }
        
        public string[] PriceFillter { get; set; }        
    }
}