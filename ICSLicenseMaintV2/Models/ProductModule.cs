namespace ICSLicenseMaintV2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductModule
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(100)]
        public string ModuleID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string ProductID { get; set; }

        [StringLength(50)]
        public string ModuleName { get; set; }

        public short? CrypKeyOptCode { get; set; }

        public bool? IncludeInDemo { get; set; }

        public virtual Product Product { get; set; }
    }
}
