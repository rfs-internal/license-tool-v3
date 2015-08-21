namespace ICSLicenseMaintV2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductModule
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(100)]
        [DisplayName("Module ID")]
        public string ModuleID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        [DisplayName("Product ID")]
        public string ProductID { get; set; }

        [StringLength(50)]
        [DisplayName("Module Name")]
        public string ModuleName { get; set; }

        [DisplayName("CrypKey Opt Code")]
        public short? CrypKeyOptCode { get; set; }

        [DisplayName("Include In Demo")]
        public bool? IncludeInDemo { get; set; }

        public virtual Product Product { get; set; }
    }
}
