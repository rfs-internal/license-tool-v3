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
    public class CustomersController : Controller
    {
        private ICSLicenses db = new ICSLicenses();

        // GET: Customers
        public ActionResult Index()
        {
            var customerInfos = db.Database.SqlQuery<CustomerInfo>("SELECT C.CustomerID, C.CustomerName, COUNT(DISTINCT CS.SiteID) AS SiteCount, COUNT(DISTINCT L.LicenseID) AS LicenseCount FROM Customers C LEFT JOIN CustomerSites CS ON CS.CustomerID = C.CustomerID LEFT JOIN Licenses L ON L.CustomerID = C.CustomerID GROUP BY C.CustomerID, C.CustomerName ORDER By C.CustomerName");
            return View(customerInfos.ToList());
        }

        public JsonResult All()
        {
            var customers = db.Customers.Select(c => new { id = c.CustomerID, name = c.CustomerName }).ToList();
            return Json(customers, JsonRequestBehavior.AllowGet);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerID,CustomerName")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                if(this.TryAndHandleErrorWithAlert(() => db.SaveChanges()))
                {
                    return RedirectToAction("Index");
                }
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,CustomerName")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                if(this.TryAndHandleErrorWithAlert(() => db.SaveChanges()))
                {
                    return RedirectToAction("Index");
                }
            }
            return View(customer);
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
