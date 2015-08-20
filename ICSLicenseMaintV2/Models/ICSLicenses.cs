namespace ICSLicenseMaintV2
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ICSLicenses : DbContext
    {
        public ICSLicenses()
            : base("name=ICSLicenses")
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerSite> CustomerSites { get; set; }
        public virtual DbSet<LicensedModule> LicensedModules { get; set; }
        public virtual DbSet<LicensedModulesHistory> LicensedModulesHistories { get; set; }
        public virtual DbSet<License> Licenses { get; set; }
        public virtual DbSet<LicensesHistory> LicensesHistories { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<ProductModule> ProductModules { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerSite>()
                .Property(e => e.SiteDescription)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerSite>()
                .HasMany(e => e.Licenses)
                .WithRequired(e => e.CustomerSite)
                .HasForeignKey(e => new { e.CustomerID, e.SiteID })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LicensedModulesHistory>()
                .Property(e => e.ChangeType)
                .IsFixedLength();

            modelBuilder.Entity<LicensesHistory>()
                .Property(e => e.MachineName)
                .IsFixedLength();

            modelBuilder.Entity<LicensesHistory>()
                .Property(e => e.ChangeType)
                .IsFixedLength();

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Licenses)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductModules)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

        }

        public System.Data.Entity.DbSet<ICSLicenseMaintV2.ViewModels.NewCustomerAndSiteModel> NewCustomerAndSiteModels { get; set; }
    }
}
