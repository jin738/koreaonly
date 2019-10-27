using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KoreaOnly
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
               name: "Sitemap",
               url: "sitemap.xml",
               defaults: new { controller = "Sitemap", action = "index" }
           );


            routes.MapRoute(
                name: "DefaultFlight",
                url: "Search/Flight/{departureCode}/{arrivalCode}/{text}",
                defaults: new { controller = "index", action = "index", departureCode = UrlParameter.Optional, arrivalCode = UrlParameter.Optional, text = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DefaultFlight2",
                url: "Flight/{departureCode}/{arrivalCode}/{text}",
                defaults: new { controller = "index", action = "index", departureCode = UrlParameter.Optional, arrivalCode = UrlParameter.Optional, text = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Index", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
