using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ICSLicenseMaintV2.ViewModels
{
    public class NewSiteModel
    {
        [Key]
        [Required]
        public int LicenseID { get; set; }
        [Required]
        [StringLength(10)]
        [DisplayName("Site ID")]
        public string SiteID { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("Site Name")]
        public string SiteName { get; set; }
        [DisplayName("Site Description")]
        public string SiteDescription { get; set; }
    }
}