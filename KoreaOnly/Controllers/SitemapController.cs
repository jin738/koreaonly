
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KoreaOnly.Controllers
{
    public class SitemapController : Controller
    {
        // GET: Sitemap

        public XmlSitemapResult Index()
        {
            var F = Server.MapPath("~") + "/Content/Loc.x";
            if (!System.IO.File.Exists(F))
            {
                using (System.IO.File.Create(F))
                {

                }
            }

            var r = System.IO.File.ReadAllLines(F);

            var LX = (from f in r
                      select
                      new SitemapItem(f)
                      {
                          Priority = 1,
                          LastModified = DateTime.Now,
                          ChangeFrequency = ChangeFrequency.Daily
                      }
                                     ).ToList();

            //List<ISitemapItem> L = new List<ISitemapItem>()
            //{
            //    new SitemapItem("https://matrixinn.net/Search/Flight/LOS ANGLES/LOS/KARACHI/KHI")
            //    {
            //        Priority = 1,
            //        LastModified = DateTime.Now,
            //        ChangeFrequency = ChangeFrequency.Daily
            //    },
            //    new SitemapItem("https://matrixinn.net/Search/Flight/ISLAMABAD/ISB/KARACHI/KHI")
            //    {
            //        Priority = 1,
            //        LastModified = DateTime.Now,
            //        ChangeFrequency = ChangeFrequency.Daily
            //    },
            //    new SitemapItem("https://matrixinn.net/Search/Flight/KARACHI/KHI/ISLAMABAD/ISB")
            //    {
            //        Priority = 1,
            //        LastModified = DateTime.Now,
            //        ChangeFrequency = ChangeFrequency.Daily
            //    }
            //};

            return new XmlSitemapResult(LX);
        }
    }
}