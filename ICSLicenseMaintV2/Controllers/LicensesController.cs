using ICSLicenseMaintV2.Utils;
using ICSLicenseMaintV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace ICSLicenseMaintV2.Controllers
{
    public class LicensesController : Controller
    {
        private ICSLicenses db = DbContextFactory.CreateInstance();
        private readonly IPermissionAuthorized permissionAuth = new PermissionAuthorized();

        // GET: Licenses
        public ActionResult Index(string id, string sort = "LicenseID", bool ascending = false)
        {
            if (id == null)
            {
                this.AddAlert(AlertModel.Warning(string.Format("Could not find customer id: <b>{0}</b>", id)));
                return RedirectToAction("Index", "LicenseSearch");
            }

            ViewBag.CustomerID = id;
            try
            {
                ViewBag.CustomerName = db.Customers.Single(c => c.CustomerID == id).CustomerName;
            }
            catch(Exception ex)
            {
                this.AddAlert(AlertModel.Warning(string.Format("Could not find customer id: <b>{0}</b>", id)));
                return RedirectToAction("Index", "LicenseSearch");
            }

            var licenses = db.Licenses.Where(l => l.CustomerID == id);
            
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
            if(license.CustomerID != edit.CustomerID)
            {
                edit.CustomerID = edit.CustomerID.ToUpper();
                // They changed customers, set a new CustomerSite
                var customerSite = db.CustomerSites.FirstOrDefault(s => s.CustomerID == edit.CustomerID && s.SiteID == edit.CustomerID);
                if (customerSite == null)
                {
                    var customerName = db.Customers.Where(c => c.CustomerID == edit.CustomerID).Select(c => c.CustomerName).Single();
                    // This customer doesn't have any customer sites, create one:
                    customerSite = new CustomerSite
                    {
                        CustomerID = edit.CustomerID,
                        SiteID = edit.CustomerID,
                        SiteName = customerName,
                        SiteDescription = customerName
                    };
                    db.CustomerSites.Add(customerSite);
                }

                license.CustomerID = customerSite.CustomerID;
                license.SiteID = customerSite.SiteID;
            }

            if(permissionAuth.IsAuthorized(HttpContext.User.Identity.Name))
            {
                license.TimeOut = !edit.IsPermanent;
            }
            if(edit.AddDays > 0)
            {
                var diff = (DateTime.UtcNow.AddDays(edit.AddDays) - license.DateIssued).TotalDays;
                license.DaysRemaining = (int)Math.Ceiling(diff);
            }
            license.TotalUserCount = edit.UserCount;

            db.SaveChanges();

            this.AddAlert(AlertModel.Success(string.Format("License <b>{0}</b> updated", license.LicenseID)));

            return RedirectToAction("Edit", new { id = license.LicenseID });
        }

        public ActionResult CopyExisting(int id)
        {
            var license = db.Licenses.Single(l => l.LicenseID == id);

            ViewBag.Modules = from licensedModule in db.LicensedModules
                              join productModule in db.ProductModules on licensedModule.ModuleID equals productModule.ModuleID
                              where licensedModule.LicenseID == id
                              orderby productModule.ModuleName
                              select productModule.ModuleName;

            ViewBag.SourceLicenseId = db.Licenses.Where(l => l.MachineName == license.MachineName && l.LicenseID != id)
                .OrderByDescending(l => l.ChangeTime)
                .Select(l=>l.LicenseID.ToString())
                .FirstOrDefault();

            return View(license);
        }

        [HttpPost]
        public JsonResult Lookup(int id)
        {
            var lic = db.Licenses.SingleOrDefault(l => l.LicenseID == id);

            if(lic != null)
            {
                var modules = from licensedModule in db.LicensedModules
                              join productModule in db.ProductModules on licensedModule.ModuleID equals productModule.ModuleID
                              where licensedModule.LicenseID == id
                              orderby productModule.ModuleName
                              select productModule.ModuleName;

                return Json(new
                {
                    lic.CustomerSite.SiteName,
                    lic.CustomerSite.Customer.CustomerName,
                    lic.MachineID,
                    lic.InstallPath,
                    lic.MachineName,
                    lic.TotalUserCount,
                    lic.IsPermanent,
                    DateIssued = lic.DateIssued.ToString("M/d/yyyy h:mm tt"),
                    ExpiryDate = lic.ExpiryDate.ToString("M/d/yyyy h:mm tt"),
                    ChangeTime = lic.ChangeTime.ToString("M/d/yyyy h:mm tt"),
                    Modules = modules
                });
            }
            return Json(lic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CopyExisting(int targetLicense, int sourceLicense)
        {
            License target = null, source = null;
            if(targetLicense == sourceLicense)
            {
                this.AddAlert(AlertModel.Error("<i>Target License</i> and <i>Source License</i> must be different."));
                return RedirectToAction("CopyExisting", new { id = targetLicense });
            }

            try
            {
                target = db.Licenses.Single(l => l.LicenseID == targetLicense);
                source = db.Licenses.Single(l => l.LicenseID == sourceLicense);
            }
            catch(Exception ex)
            {
                this.AddAlert(AlertModel.Error(string.Format("No license found with id: <b>{0}</b>", sourceLicense)));
                return RedirectToAction("CopyExisting", new { id = target.LicenseID });
            }
            

            target.CustomerID = source.CustomerID;
            target.SiteID = source.SiteID;
            // target.TimeOut = source.TimeOut;
            target.TotalUserCount = source.TotalUserCount;
            var expiryDate = source.ExpiryDate > target.ExpiryDate ? source.ExpiryDate : target.ExpiryDate;
            target.DaysRemaining = Math.Max(0, (int)(expiryDate - target.DateIssued).TotalDays);

            db.LicensedModules.RemoveRange(target.LicensedModules);
            target.LicensedModules.Clear();
            db.LicensedModules.AddRange(source.LicensedModules.Select(m => new LicensedModule
            {
                DateIssued = DateTime.UtcNow,
                DaysRemaining = target.DaysRemaining,
                LastRequestedUpdate = target.LastRequestedUpdate,
                LicenseID = target.LicenseID,
                ModuleID = m.ModuleID,
                ProductID = m.ProductID,
                TimeOut = m.TimeOut,
                UserCount = m.UserCount
            }));

            if (this.TryAndHandleErrorWithAlert(() => db.SaveChanges()))
            {
                return RedirectToAction("Edit", new { id = target.LicenseID });
            }
            return RedirectToAction("CopyExisting", new { id = target.LicenseID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditModules(LicenseModulesEditModel edit)
        {
            edit.Modules = edit.Modules ?? new string[0];

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
            return new RedirectResult(Url.Action("Edit", new { id = license.LicenseID }) + "#tab_modules");
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

            db.Database.ExecuteSqlCommand("DELETE FROM Licenses WHERE LicenseID = {0}", id);

            // This doesn't work since db.Licenses is a view:
            //db.Licenses.Remove(license);
            //db.SaveChanges();
            this.AddAlert(AlertModel.Success(string.Format("License <b>{0}</b> deleted", license.LicenseID)));
            return RedirectToAction("Index", new { id = customerId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignToNewCustomerAndSite(NewCustomerAndSiteModel model)
        {
            if (ModelState.IsValid)
            {
                model.CustomerID = model.CustomerID.ToUpper();

                var customer = db.Customers.SingleOrDefault(c => c.CustomerID == model.CustomerID);
                if (customer == null)
                {
                    customer = new Customer
                    {
                        CustomerID = model.CustomerID,
                        CustomerName = model.CustomerName
                    };
                    db.Customers.Add(customer);
                }
                model.CustomerID = customer.CustomerID;

                var customerSite = db.CustomerSites.FirstOrDefault(cs => cs.CustomerID == model.CustomerID && cs.SiteID == model.CustomerID);
                if (customerSite == null)
                {
                    customerSite = new CustomerSite
                    {
                        CustomerID = model.CustomerID,
                        SiteID = model.CustomerID,
                        SiteDescription = model.CustomerName,
                        SiteName = model.CustomerName
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

        public ActionResult DownloadLicenseFile(int id)
        {
            string productid = "weblicense";
            string responseData;
            string errorMessage;

            var license = db.Licenses.Single(l => l.LicenseID == id);
            
            if(license.ProductVersion == RfsmartVersion.V4)
            {
                productid = license.ProductID;
            }
            
            if(new LicenseUtil().GetCurrentLicense(id, out responseData, out errorMessage))
            {
                var cd = new System.Net.Mime.ContentDisposition { FileName = productid + ".txt", Inline = false };
                Response.AppendHeader("Content-Disposition", cd.ToString());
                return File(Encoding.UTF8.GetBytes(responseData), "text");
            }
            this.AddAlert(AlertModel.Error(errorMessage));
            return RedirectToAction("Edit", new { id = id });
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
