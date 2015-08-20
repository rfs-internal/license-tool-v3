using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICSLicenseMaintV2.ViewModels
{
    public class CustomerSiteInfo
    {
        public string SiteID { get; set; }
        public string SiteName { get; set; }
        public int LicenseCount { get; set; }
        public int ExpiredLicenseCount { get; set; }
    }
}