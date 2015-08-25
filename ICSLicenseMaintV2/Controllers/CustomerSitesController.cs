using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ICSLicenseMaintV2;
using ICSLicenseMaintV2.ViewModels;

namespace ICSLicenseMaintV2.Controllers
{
    public class CustomerSitesController : Controller
    {
        private ICSLicenses db = DbContextFactory.CreateInstance();

        // GET: CustomerSites
        public ActionResult Index(string id)
        {
            var customer = db.Customers.Include(c=>c.CustomerSites).SingleOrDefault(c => c.CustomerID == id);
            if(customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        public JsonResult All()
        {
            var sites = db.CustomerSites.Select(s => new { id = s.SiteID, customerid = s.CustomerID, name = s.SiteName }).ToList();
            return Json(sites, JsonRequestBehavior.AllowGet);
        }

        // GET: CustomerSites/Details/5
        public ActionResult Details(string customerId, string siteId)
        {
            if (customerId == null || siteId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerSite customerSite = db.CustomerSites.Find(customerId, siteId);
            if (customerSite == null)
            {
                return HttpNotFound();
            }
            return View(customerSite);
        }

        // GET: CustomerSites/Create
        public ActionResult Create(string id)
        {
            ViewBag.CustomerID = new SelectList(db.Customers.OrderBy(c=>c.CustomerName), "CustomerID", "CustomerName", id);
            return View();
        }

        // POST: CustomerSites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerID,SiteID,SiteName,SiteDescription")] CustomerSite customerSite)
        {
            if (ModelState.IsValid)
            {
                db.CustomerSites.Add(customerSite);
                if(this.TryAndHandleErrorWithAlert(() => db.SaveChanges()))
                {
                    return RedirectToAction("Index", new { id = customerSite.CustomerID });
                }
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerName", customerSite.CustomerID);
            return View(customerSite);
        }

        // GET: CustomerSites/Edit/5
        public ActionResult Edit(string customerId, string siteId)
        {
            if (customerId == null || siteId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerSite customerSite = db.CustomerSites.Find(customerId, siteId);
            if (customerSite == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerName", customerSite.CustomerID);
            return View(customerSite);
        }

        // POST: CustomerSites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,SiteID,SiteName,SiteDescription")] CustomerSite customerSite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerSite).State = EntityState.Modified;
                if(this.TryAndHandleErrorWithAlert(() => db.SaveChanges()))
                {
                    return RedirectToAction("Index", new { id = customerSite.CustomerID });
                }
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerName", customerSite.CustomerID);
            return View(customerSite);
        }

        // GET: CustomerSites/Delete/5
        public ActionResult Delete(string customerId, string siteId)
        {
            if (customerId == null || siteId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerSite customerSite = db.CustomerSites.Find(customerId, siteId);
            if (customerSite == null)
            {
                return HttpNotFound();
            }
            return View(customerSite);
        }

        // POST: CustomerSites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string customerId, string siteId)
        {
            CustomerSite customerSite = db.CustomerSites.Find(customerId, siteId);
            db.CustomerSites.Remove(customerSite);
            this.TryAndHandleErrorWithAlert(() => db.SaveChanges());
            return RedirectToAction("Index", new { id = customerId });
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
