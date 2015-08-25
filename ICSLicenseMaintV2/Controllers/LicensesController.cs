using ICSLicenseMaintV2.ViewModels;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ICSLicenseMaintV2.Controllers
{
    public class LicensesController : Controller
    {
        private ICSLicenses db = DbContextFactory.CreateInstance();
        private readonly IPermissionAuthorized permissionAuth = new PermissionAuthorized();

        public ActionResult All()
        {
            var licenses = db.Licenses.ToList();
            return View(licenses);
        }

        // GET: Licenses
        public ActionResult Index(string customerId, string siteId = null, string sort = "LicenseID", bool ascending = true)
        {
            if (customerId == null)
            {
                return RedirectToAction("Search", "LicenseSearch");
            }

            var licenses = db.Licenses.Where(l => l.CustomerID == customerId);

            ViewBag.CustomerID = customerId;
            ViewBag.CustomerName = db.Customers.Single(c => c.CustomerID == customerId).CustomerName;
            ViewBag.SiteID = siteId;

            if (siteId != null)
            {
                licenses = licenses.Where(l => l.SiteID == siteId);
                var customerSite = db.CustomerSites.SingleOrDefault(cs => cs.CustomerID == customerId && cs.SiteID == siteId);
                if(customerSite == null)
                {
                    return RedirectToAction("Index", new { customerId = customerId, sort = sort, ascending = ascending });
                }
                ViewBag.SiteName = customerSite.SiteName;
            }

            return View(licenses.OrderByField(sort, ascending).ToList());
        }

        // GET: Licenses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            License license = db.Licenses.Find(id);
            if (license == null)
            {
                return HttpNotFound();
            }

            ViewBag.CustomerName = db.Customers.Single(c => c.CustomerID == license.CustomerID).CustomerName;
            
            return View(license);
        }

        // POST: Licenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LicenseID,CustomerID,SiteID,AddDays,UserCount,IsPermanent")] LicenseEditModel edit)
        {
            var license = db.Licenses.Find(edit.LicenseID);
            license.CustomerID = edit.CustomerID;
            license.SiteID = edit.SiteID;

            if(permissionAuth.IsAuthorized(HttpContext.User.Identity.Name))
            {
                license.TimeOut = !edit.IsPermanent;
            }
            if(edit.AddDays > 0)
            {
                var diff = (DateTime.UtcNow.AddDays(edit.AddDays) - license.DateIssued).TotalDays;
                license.DaysRemaining = (int)Math.Ceiling(diff);
            }
            db.SaveChanges();

            this.AddAlert(AlertModel.Success(string.Format("License <b>{0}</b> updated", license.LicenseID)));

            return RedirectToAction("Edit", new { id = license.LicenseID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditModules(LicenseModulesEditModel edit)
        {
            var license = db.Licenses.Find(edit.LicenseID);
            var licenseModules = license.LicensedModules.ToList();
            var newModuleSet = edit.Modules.ToDictionary(k => k, v => true, StringComparer.CurrentCultureIgnoreCase);
            var oldModuleSet = licenseModules.ToDictionary(k => k.ModuleID, v => true, StringComparer.CurrentCultureIgnoreCase);
            
            var oldModules = licenseModules.Where(m => !newModuleSet.ContainsKey(m.ModuleID));
            foreach (var oldModule in oldModules)
            {
                db.LicensedModules.Remove(oldModule);
            }

            foreach(var newModule in newModuleSet.Keys)
            {
                if(!oldModuleSet.ContainsKey(newModule))
                {
                    db.LicensedModules.Add(new LicensedModule
                    {
                        ModuleID = newModule,
                        DateIssued = license.DateIssued,
                        DaysRemaining = license.DaysRemaining,
                        LastRequestedUpdate = license.LastRequestedUpdate,
                        LicenseID = license.LicenseID,
                        ProductID = license.ProductID,
                        TimeOut = license.TimeOut,
                        UserCount = license.TotalUserCount
                    });
                }
            }

            db.SaveChanges();

            return RedirectToAction("Edit", new { id = license.LicenseID });
        }

        public JsonResult Modules(int id)
        {
            var allModules = db.ProductModules
                .OrderBy(m=>m.ModuleName)
                .Select(m => new { id = m.ModuleID, name = m.ModuleName })
                .ToList();

            var myModules = from licensedModule in db.LicensedModules
                            join productModule in db.ProductModules on licensedModule.ModuleID equals productModule.ModuleID 
                            where licensedModule.LicenseID == id
                            orderby productModule.ModuleName
                            select new { id = productModule.ModuleID, name = productModule.ModuleName };

            return Json(new { modules = allModules, licensedModules = myModules }, JsonRequestBehavior.AllowGet);
        }

        // GET: Licenses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            License license = db.Licenses.Find(id);
            if (license == null)
            {
                return HttpNotFound();
            }
            return View(license);
        }

        // POST: Licenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            License license = db.Licenses.Find(id);
            var customerId = license.CustomerID;
            var siteId = license.SiteID;

            db.Licenses.Remove(license);
            db.SaveChanges();
            this.AddAlert(AlertModel.Success(string.Format("License <b>{0}</b> deleted", license.LicenseID)));
            return RedirectToAction("Index", new { customerId = customerId, siteId = siteId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignToNewCustomerAndSite(NewCustomerAndSiteModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = db.Customers.SingleOrDefault(c => c.CustomerID == model.CustomerID);
                if (customer == null)
                {
                    customer = new Customer
                    {
                        CustomerID = model.CustomerID.ToUpper(),
                        CustomerName = model.CustomerName
                    };
                    db.Customers.Add(customer);
                }
                model.CustomerID = customer.CustomerID;

                var customerSite = db.CustomerSites.SingleOrDefault(cs => cs.CustomerID == model.CustomerID && cs.SiteID == model.SiteID);
                if (customerSite == null)
                {
                    customerSite = new CustomerSite
                    {
                        CustomerID = model.CustomerID,
                        SiteID = model.SiteID.ToUpper(),
                        SiteDescription = model.SiteDescription,
                        SiteName = model.SiteName
                    };
                    db.CustomerSites.Add(customerSite);
                }
                model.SiteID = customerSite.SiteID;

                var license = db.Licenses.Single(l => l.LicenseID == model.LicenseID);
                license.CustomerID = model.CustomerID;
                license.SiteID = model.SiteID;

                db.SaveChanges();
            }

            this.AddAlert(AlertModel.Success(string.Format("Assigned to <b>{0}</b>", model.CustomerName)));
            return RedirectToAction("Edit", new { id = model.LicenseID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignToNewSite(NewSiteModel model)
        {
            if (ModelState.IsValid)
            {
                var license = db.Licenses.Single(l => l.LicenseID == model.LicenseID);

                var customerSite = db.CustomerSites.SingleOrDefault(cs => cs.CustomerID == license.CustomerID && cs.SiteID == model.SiteID);
                if (customerSite == null)
                {
                    customerSite = new CustomerSite
                    {
                        CustomerID = license.CustomerID,
                        SiteID = model.SiteID.ToUpper(),
                        SiteDescription = model.SiteDescription,
                        SiteName = model.SiteName
                    };
                    db.CustomerSites.Add(customerSite);
                }
                model.SiteID = customerSite.SiteID;
                license.SiteID = model.SiteID;

                db.SaveChanges();
            }

            this.AddAlert(AlertModel.Success(string.Format("Created site <b>{0}</b> and assigned to <b>{1}</b>", model.SiteName, model.LicenseID)));
            return RedirectToAction("Edit", new { id = model.LicenseID });
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
