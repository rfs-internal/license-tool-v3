using ICSLicenseMaintV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ICSLicenseMaintV2.Controllers
{
    public class LicenseSearchController : Controller
    {
        private ICSLicenses db = new ICSLicenses();
        private const int LIMIT = 200;

        // GET: LicenseSearch
        public ActionResult Search(string searchText)
        {
            if(string.IsNullOrWhiteSpace(searchText))
            {
                this.AddAlert(AlertModel.Error("No search criteria provided"));
                return View(Enumerable.Empty<License>());
            }

            searchText = searchText.Trim();

            int licenseId = -1;
            if(Int32.TryParse(searchText, out licenseId))
            {
                var license = db.Licenses.SingleOrDefault(l => l.LicenseID == licenseId);
                if(license != null)
                {
                    return RedirectToAction("Edit", "Licenses", new { id = license.LicenseID });
                }
            }

            var licenses = db.Licenses.AsQueryable();
            foreach(var part in searchText.Split(' '))
            {
                licenses = licenses.Where(l =>
                l.MachineID.Contains(part) ||
                l.MachineName.Contains(part) ||
                l.CustomerSite.Customer.CustomerName.Contains(part) ||
                l.CustomerSite.SiteName.Contains(part));
            }

            var results = licenses.Take(LIMIT).ToList();

            if (results.Count == 0)
            {
                this.AddAlert(AlertModel.Warning(string.Format("No results found for: <b>{0}</b>", searchText)));
            }
            else if (results.Count == 1)
            {
                return RedirectToAction("Edit", "Licenses", new { id = results[0].LicenseID });
            }
            else if (results.Count == LIMIT)
            {
                this.AddAlert(AlertModel.Warning(string.Format("Only showing {0} results. Narrow down your search.", LIMIT)));
            }

            return View(results);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}