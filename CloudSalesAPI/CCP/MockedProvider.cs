namespace CloudSalesAPI.Provider
{
    public class MockedProvider : ICloudComputingProvider
    {
        private readonly List<Product> _availableProducts; 

        public MockedProvider()
        {
            _availableProducts = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Microsoft Office",
                    AvailableQuantity = 10,
                    Price = 100
                },
                new Product
                {
                    Id = 2,
                    Name = "Visual Studio Code",
                    AvailableQuantity = 8,
                    Price = 200
                },
                new Product
                {
                    Id = 3,
                    Name = "Azure",
                    AvailableQuantity = 20,
                    Price = 300
                },
                    new Product
                {
                    Id = 4,
                    Name = "Software X",
                    AvailableQuantity = 10,
                    Price = 200
                },
                new Product
                {
                    Id = 5,
                    Name = "Software Y",
                    AvailableQuantity = 28,
                    Price = 199.88M
                }
            };
        }

        public IEnumerable<Product> GetProducts()
        {
            return _availableProducts;
        }

        public bool ProvisionProduct(int productId, int productQty)
        {
            // If the product with the given productId is found, mock response to true
            if (_availableProducts.FirstOrDefault(p => p.Id == productId) != null)
            {
                return true; // Provisioning successful
            }

            return false; // Product provisioning failed due to insufficient quantity or product not found
        }
    }
}
