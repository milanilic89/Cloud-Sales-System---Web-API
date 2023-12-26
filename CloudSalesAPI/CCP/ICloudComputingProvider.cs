namespace CloudSalesAPI.Provider
{
    public interface ICloudComputingProvider
    {
        IEnumerable<Product> GetProducts();

        bool ProvisionProduct(int productId, int productQty);
    }
}
