using CloudSalesAPI.Data;
using CloudSalesAPI.Models;
using CloudSalesAPI.Provider;
using Microsoft.Extensions.Configuration;

namespace CloudSalesAPI.Services
{
    public class ProvisioningService : IProvisioningService
    {

        private readonly ICloudComputingProvider _provider;
        private readonly IDataContext _context;
        private readonly ValidatorManager _productValidator;


        private readonly IConfiguration _configuration;
        public ProvisioningService(ICloudComputingProvider provider, IDataContext context, IConfiguration configuration)
        {
            _provider = provider;
            _context = context;
            _configuration = configuration;
            _productValidator = new ValidatorManager(_provider.GetProducts());
        }

        public async Task<bool> ChangeProductLicenseQuantity(int accountId, int productId, int newQuantity)
        {
            var purchasedProduct = _context.PurchasedLicences
                .FirstOrDefault(pp => pp.CustomerAccountID == accountId && pp.ProductID == productId && pp.Status == LicenseStatus.Active);

            if (purchasedProduct != null && _productValidator.IsValidQuantity(productId, newQuantity))
            {
                // Check with the cloud provider if there are enough available licenses
                var availableProducts = _provider.GetProducts();
                var cloudProduct = availableProducts.FirstOrDefault(p => p.Id == productId);

                if (cloudProduct != null && cloudProduct.AvailableQuantity >= newQuantity)
                {
                    purchasedProduct.Quantity = newQuantity;
                    await _context.SaveChangesAsync(default);
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<Product> GetAvailableProducts()
        {
            return _provider.GetProducts();
        }

        public async Task<List<PurchasedLicense>> GetPurchasedProductLicenses(int accountId)
        {
            var purchasedProducts = await _context.PurchasedLicences.
                            Where(c => c.CustomerAccountID == accountId)
                               .ToListAsync();
            return purchasedProducts;
        }

        public async Task<bool> PurchaseProduct(int accountId, int productId, int quantity)
        {
            var success = false;

            // validate quantity and product id
            if (_productValidator.IsValidQuantity(productId, quantity))
            {
                // provision on ccp side
                success = _provider.ProvisionProduct(productId, quantity);
                if (success)
                {
                    var productName = _provider.GetProducts().First(x => x.Id == productId).Name;

                    DateTime validToDate;
                    var billingMonthsString = _configuration["BillingPeriodMonths"];
                    if (int.TryParse(billingMonthsString, out int billingMonths))
                    {
                        validToDate = DateTime.Now.AddMonths(billingMonths);
                    }
                    else
                    {
                        // Handle parsing error, the configuration value is not a valid integer
                        // take default value 1 month
                        validToDate = DateTime.Now.AddMonths(1);
                    }

                    await _context.PurchasedLicences.AddAsync(new PurchasedLicense
                    {
                        CustomerAccountID = accountId,
                        ProductID = productId,
                        ProductName = productName,
                        Quantity = quantity,
                        Status = LicenseStatus.Active,
                        ValidToDate = validToDate
                    });
                    await _context.SaveChangesAsync(default);

                }
            }
            return success;
        }


        public async Task<bool> CancelProductLicense(int accountId, int productId)
        {
            var purchasedProduct = _context.PurchasedLicences
                .FirstOrDefault(pp => pp.CustomerAccountID == accountId && pp.ProductID == productId && pp.Status == LicenseStatus.Active);

            if (purchasedProduct != null)
            {
                purchasedProduct.Status = LicenseStatus.Canceled;
                await _context.SaveChangesAsync(default);
                return true;
            }

            return false;
        }

        public async Task<bool> ExtendProductLicense(int accountId, int externalProductId)
        {
            var purchasedProduct = _context.PurchasedLicences
                .FirstOrDefault(pp => pp.CustomerAccountID == accountId && pp.ProductID == externalProductId && pp.Status == LicenseStatus.Active);

            if (purchasedProduct != null)
            {
                var billingMonthsString = _configuration["BillingPeriodMonths"];
                if (int.TryParse(billingMonthsString, out int billingMonths))
                {
                    purchasedProduct.ValidToDate = purchasedProduct.ValidToDate.AddMonths(billingMonths);
                    await _context.SaveChangesAsync(default);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
    }
}
