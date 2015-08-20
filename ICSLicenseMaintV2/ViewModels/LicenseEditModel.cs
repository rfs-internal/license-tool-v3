using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICSLicenseMaintV2.ViewModels
{
    public class LicenseEditModel
    {
        public int LicenseID { get; set; }
        public string CustomerID { get; set; }
        public string SiteID { get; set; }
        public int AddDays { get; set; }
        public int UserCount {get;set;}

        public bool IsPermanent { get; set; }
    }
}