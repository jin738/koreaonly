using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace KoreaOnly
{
    public class DatabaseData
    {
        public static string AIRPORT_CODE_JSON = File.ReadAllText(HostingEnvironment.MapPath("/Content/AIRPORTCODE.json"));
    }
}