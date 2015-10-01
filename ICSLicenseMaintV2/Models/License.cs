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
        [DisplayName("Machine ID")]
        public string MachineID { get; set; }

        [Required]
        [StringLength(200)]
        [DisplayName("Install Path")]
        public string InstallPath { get; set; }

        [Required]
        [StringLength(200)]
        [DisplayName("Machine Name")]
        public string MachineName { get; set; }

        [DisplayName("User Count")]
        public int TotalUserCount { get; set; }

        [DisplayName("Time Out")]
        public bool TimeOut { get; set; }

        [DisplayName("Days Remaining")]
        public int DaysRemaining { get; set; }

        [DisplayName("Issued")]
        public DateTime DateIssued { get; set; }

        [DisplayName("Last Requested Update")]
        public DateTime LastRequestedUpdate { get; set; }

        // BEGIN ADD FIELDS FROM VIEW
        [DisplayName("Last Changed By")]
        public string ChangeUserID { get; set; }

        [DisplayName("Last Changed Type")]
        public char ChangeType { get; set; }

        [DisplayName("Last Changed")]
        public DateTime ChangeTime { get; set; }
        // END ADD FIELDS FROM VIEW


        public virtual CustomerSite CustomerSite { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LicensedModule> LicensedModules { get; set; }

        public virtual Product Product { get; set; }
    }
}
