namespace ICSLicenseMaintV2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Permission
    {
        [Key]
        [StringLength(50)]
        public string Userid { get; set; }

        public bool AllowPermLicense { get; set; }
    }
}
