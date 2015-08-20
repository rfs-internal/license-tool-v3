using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ICSLicenseMaintV2.ViewModels
{
    public class LicenseModulesEditModel
    {
        [Key]
        public int LicenseID { get; set; }

        public string[] Modules { get; set; }
    }
}