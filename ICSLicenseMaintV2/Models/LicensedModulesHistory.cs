namespace ICSLicenseMaintV2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LicensedModulesHistory")]
    public partial class LicensedModulesHistory
    {
        [Key]
        public int RecId { get; set; }

        public int LicenseID { get; set; }

        [Required]
        [StringLength(100)]
        public string ModuleID { get; set; }

        [Required]
        [StringLength(10)]
        public string ProductID { get; set; }

        public int UserCount { get; set; }

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
