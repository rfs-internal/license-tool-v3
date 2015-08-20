namespace ICSLicenseMaintV2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LicensedModule
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LicenseID { get; set; }

        [Key]
        [Column(Order = 1)]
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

        public virtual License License { get; set; }
    }
}
