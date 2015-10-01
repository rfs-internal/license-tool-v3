namespace ICSLicenseMaintV2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class License
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public License()
        {
            LicensedModules = new HashSet<LicensedModule>();
        }

        [Display(ResourceType = typeof(LicenseMaintStrings), Name = "LicenseID")]
        public int LicenseID { get; set; }

        [Required]
        [StringLength(10)]
        public string CustomerID { get; set; }

        [Required]
        [StringLength(10)]
        public string ProductID { get; set; }

        [Required]
        [StringLength(10)]
        public string SiteID { get; set; }

        [Required]
        [StringLength(200)]
        [Display(ResourceType = typeof(LicenseMaintStrings), Name = "MachineID")]
        public string MachineID { get; set; }

        [Required]
        [StringLength(200)]
        [Display(ResourceType = typeof(LicenseMaintStrings), Name = "InstallPath")]
        public string InstallPath { get; set; }

        [Required]
        [StringLength(200)]
        [Display(ResourceType = typeof(LicenseMaintStrings), Name = "MachineName")]
        public string MachineName { get; set; }

        [Display(ResourceType = typeof(LicenseMaintStrings), Name = "UserCount")]
        public int TotalUserCount { get; set; }

        [Display(ResourceType = typeof(LicenseMaintStrings), Name = "TimeOut")]
        public bool TimeOut { get; set; }

        [Display(ResourceType = typeof(LicenseMaintStrings), Name = "DaysRemaining")]
        public int DaysRemaining { get; set; }

        [Display(ResourceType = typeof(LicenseMaintStrings), Name = "DateIssued")]
        public DateTime DateIssued { get; set; }

        [Display(ResourceType = typeof(LicenseMaintStrings), Name = "LastRequestedUpdate")]
        public DateTime LastRequestedUpdate { get; set; }

        // BEGIN ADD FIELDS FROM VIEW
        [Display(ResourceType = typeof(LicenseMaintStrings), Name = "ChangeUserID")]
        public string ChangeUserID { get; set; }

        [Display(ResourceType = typeof(LicenseMaintStrings), Name = "ChangeType")]
        public char ChangeType { get; set; }

        [Display(ResourceType = typeof(LicenseMaintStrings), Name = "ChangeTime")]
        public DateTime ChangeTime { get; set; }
        // END ADD FIELDS FROM VIEW


        public virtual CustomerSite CustomerSite { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LicensedModule> LicensedModules { get; set; }

        public virtual Product Product { get; set; }
    }
}
