namespace ICSLicenseMaintV2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CustomerSite
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustomerSite()
        {
            Licenses = new HashSet<License>();
        }

        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        [DisplayName("Customer ID")]
        public string CustomerID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        [DisplayName("Site ID")]
        [Required]
        public string SiteID { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Site Name")]
        public string SiteName { get; set; }

        [DisplayName("Site Description")]
        public string SiteDescription { get; set; }

        public virtual Customer Customer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<License> Licenses { get; set; }
    }
}
