namespace CloudSalesAPI.Data
{
    public interface IDataContext
    {
        DbSet<Customer> Customers { get; }
        DbSet<CustomerAccount> CustomerAccounts { get; }
        DbSet<PurchasedLicense> PurchasedLicences { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);


    }

}
