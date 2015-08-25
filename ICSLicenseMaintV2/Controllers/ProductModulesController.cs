using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ICSLicenseMaintV2.Controllers
{
    [PermissionsAuthorize]
    public class ProductModulesController : Controller
    {
        private ICSLicenses db = DbContextFactory.CreateInstance();

        // GET: ProductModules
        public ActionResult Index()
        {
            var productModules = db.ProductModules.Include(p => p.Product);
            return View(productModules.OrderBy(p=>p.ModuleName).ToList());
        }
        
        // GET: ProductModules/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            ProductModule productModule = db.ProductModules.Single(pm => pm.ModuleID == id);
            if (productModule == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", productModule.ProductID);
            return View(productModule);
        }

        // POST: ProductModules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ModuleID,ProductID,ModuleName,CrypKeyOptCode,IncludeInDemo")] ProductModule productModule)
        {
            if (ModelState.IsValid)
            {
                var module = db.ProductModules.Single(m => m.ModuleID == productModule.ModuleID);
                module.ModuleName = productModule.ModuleName;
                if (this.TryAndHandleErrorWithAlert(() => db.SaveChanges()))
                {
                    return RedirectToAction("Index");
                }
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", productModule.ProductID);
            return View(productModule);
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
