using Portal.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Areas.Vendor.Controllers
{
    [UserAuthenticationFilter]
    public class VendorBaseController : Controller
    {
        // GET: Vendor/VendorBase
        public ActionResult Index()
        {
            return View();
        }
    }
}