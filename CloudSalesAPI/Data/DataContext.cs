global using Microsoft.EntityFrameworkCore;

namespace CloudSalesAPI.Data
{
    public class DataContext : DbContext, IDataContext
    {

        public DataContext(DbContextOptions<DataContext> options): base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PurchasedLicense>()
                .HasIndex(pp => new { pp.ProductID, pp.CustomerAccountID })
                .IsUnique();
        }

        //entities
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAccount> CustomerAccounts { get; set; }
        public DbSet<PurchasedLicense> PurchasedLicences { get; set; }

    }


}
