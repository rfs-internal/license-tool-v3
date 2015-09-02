using ICSLicenseMaintV2.ICSLicensing;
using ICSLicenseMaintV2.Utils;
using ICSLicenseMaintV2.ViewModels;
using System.IO;
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
        public ActionResult Upload(string licenseurl)
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
                    updatedLicense = rfsLicense.GetNewLicense(requestString, out responseString, out errorMessage);
                    
                    if (updatedLicense)
                    {
                        this.AddAlert(AlertModel.Success("Demo license created. Make your changes to the license, save, then click the <b>Download License File</b> button below."));
                        var licenseId = new LicenseUtil().GetLicenseIdFromResult(responseString);
                        return RedirectToAction("Edit", "Licenses", new { id = licenseId });
                        //var cd = new System.Net.Mime.ContentDisposition { FileName = "weblicense.txt", Inline = false };
                        //Response.AppendHeader("Content-Disposition", cd.ToString());
                        //return File(Encoding.UTF8.GetBytes(responseString), "text");
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