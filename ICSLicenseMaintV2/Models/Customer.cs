namespace ICSLicenseMaintV2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            CustomerSites = new HashSet<CustomerSite>();
        }

        [DisplayName("ID")]
        [StringLength(10)]
        [Required]
        public string CustomerID { get; set; }

        [StringLength(50)]
        [DisplayName("Customer Name")]
        [Required]
        public string CustomerName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerSite> CustomerSites { get; set; }
    }
}
