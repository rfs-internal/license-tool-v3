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
        private ICSLicenses db = DbContextFactory.CreateInstance();
        private const int LIMIT = 200;

        [HttpPost]
        public JsonResult SearchAjax(string searchText, int page = 0)
        {
            var licenses = db.Licenses.AsQueryable();
            foreach (var part in searchText.Split(' '))
            {
                licenses = licenses.Where(l =>
                l.LicenseID.ToString().Contains(part) ||
                l.MachineID.Contains(part) ||
                l.MachineName.Contains(part) ||
                l.CustomerSite.Customer.CustomerName.Contains(part) ||
                l.CustomerSite.SiteName.Contains(part));
            }

            var results = licenses.OrderByDescending(l=>l.LicenseID).Skip(LIMIT * page).Take(LIMIT).ToList().Select(l => new
            {
                IsPermanent = l.IsPermanent,
                TotalUserCount = l.TotalUserCount,
                MachineName = l.MachineName,
                MachineID = l.MachineID,
                CustomerName = (l.CustomerSite ?? new CustomerSite { Customer = new Customer { CustomerName = "?" } }).Customer.CustomerName,
                LicenseID = l.LicenseID,
                IsExpired = l.IsExpired,
                InstallPath = l.InstallPath,
                ExpiryDate = l.ExpiryDate.ToShortDateString(),
                LastRequestedUpdate = l.LastRequestedUpdate.ToShortDateString(),
                ShortMachineID = l.ShortMachineID
            }).ToList();


            return Json(new { results = results, canLoadMore = results.Count == LIMIT });
        }

        // GET: LicenseSearch
        public ActionResult Index(string searchText)
        {
            var last10Demos = db.Licenses.Where(l => l.CustomerID == "DEMO").OrderByDescending(l => l.LastRequestedUpdate).Take(10);
            return View(last10Demos);
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