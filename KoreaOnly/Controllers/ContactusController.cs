using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KoreaOnly.Controllers
{
    [RoutePrefix("SystemMaster/Contactus")]
    public class ContactusController : Controller
    {
        [Route("")]
        [Route("SystemMaster/Contactus/Index")]
        // GET: Contactus
        public ActionResult Index()
        {

            if (MainController.checkAdminLogin())
            {
                using (var DB = new DbConnection())
                {
                    var Re = DB.GetResult<KoreaOnly.Controllers.IndexController.CommentForm>("select  * From CommentForm order by CommentFormID desc");

                    return View(Re);
                }

            }

            return Redirect("/SystemMaster/");
        }
    }
}