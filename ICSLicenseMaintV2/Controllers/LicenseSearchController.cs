using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICSLicenseMaintV2.Controllers
{
    public class LicenseSearchController : Controller
    {
        // GET: LicenseSearch
        public ActionResult Search(string searchText)
        {
            return View();
        }
    }
}