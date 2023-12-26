namespace CloudSalesAPI.Services
{
    public interface IProvisioningService
    {

        IEnumerable<Product> GetAvailableProducts();
        Task<bool> PurchaseProduct(int accountId, int productId, int quantity);
        Task<List<PurchasedLicense>> GetPurchasedProductLicenses(int accountId);
        Task<bool> ChangeProductLicenseQuantity(int accountId, int externalProductId, int newQuantity);
        Task<bool> CancelProductLicense(int accountId, int licenceProductId);
        Task<bool> ExtendProductLicense(int accountId, int licenceProductId);
    }
}
