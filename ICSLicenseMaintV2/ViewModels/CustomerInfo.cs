using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICSLicenseMaintV2.ViewModels
{
    public class CustomerInfo
    {
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }

        public int SiteCount { get; set; }
        public int LicenseCount { get; set; }
    }
}