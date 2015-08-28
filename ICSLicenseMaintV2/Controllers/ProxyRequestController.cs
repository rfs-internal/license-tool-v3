using ICSLicenseMaintV2.ICSLicensing;
using ICSLicenseMaintV2.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ICSLicenseMaintV2.Controllers
{
    public class ProxyRequestController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(string licenseurl, string action)
        {
            if(Request.Files.Count > 0)
            {
                string requestString = null;
                string responseString = null;
                string errorMessage = null;
                using (var streamReader = new StreamReader(Request.Files[0].InputStream))
                {
                    requestString = streamReader.ReadToEnd();
                    streamReader.Close();
                }   
                
                using (RFSLicense rfsLicense = new RFSLicense() { Url = licenseurl })
                {
                    bool updatedLicense = false;
                    if(action.IndexOf("update", StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        updatedLicense = rfsLicense.GetUpdatedLicense(requestString, out responseString, out errorMessage);
                    }
                    else // "demo"
                    {
                        updatedLicense = rfsLicense.GetNewLicense(requestString, out responseString, out errorMessage);
                    }
                    
                    if (updatedLicense)
                    {
                        var cd = new System.Net.Mime.ContentDisposition { FileName = "weblicense.txt", Inline = false };
                        Response.AppendHeader("Content-Disposition", cd.ToString());
                        return File(Encoding.UTF8.GetBytes(responseString), "text");
                    }
                    else
                    {
                        this.AddAlert(AlertModel.Error(errorMessage));
                    }
                }   
            }
            else
            {
                this.AddAlert(AlertModel.Error("No file uploaded"));
            }
            return RedirectToAction("Index");
        }
    }
}