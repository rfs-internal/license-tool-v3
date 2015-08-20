namespace ICSLicenseMaintV2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LicensesHistory")]
    public partial class LicensesHistory
    {
        [Key]
        public int RecID { get; set; }

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
        public string MachineID { get; set; }

        [Required]
        [StringLength(200)]
        public string InstallPath { get; set; }

        [Required]
        [StringLength(256)]
        public string MachineName { get; set; }

        public int TotalUserCount { get; set; }

        public bool TimeOut { get; set; }

        public int DaysRemaining { get; set; }

        public DateTime DateIssued { get; set; }

        public DateTime LastRequestedUpdate { get; set; }

        [Required]
        [StringLength(250)]
        public string ChangeUserID { get; set; }

        [Required]
        [StringLength(1)]
        public string ChangeType { get; set; }

        public DateTime ChangeTime { get; set; }
    }
}
