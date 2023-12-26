namespace CloudSalesAPI.Provider
{
    public class ValidatorManager
    {
        private readonly IEnumerable<Product> _availableProducts;

        public ValidatorManager(IEnumerable<Product> availableProducts)
        {
            _availableProducts = availableProducts;
        }

        public bool IsValidQuantity(int productId, int requestedQuantity)
        {
            // Find the product with the given productId in the available products list
            Product product = _availableProducts.FirstOrDefault(p => p.Id == productId);

            // If the product with the given productId is found and the available quantity is greater than or equal to the requested quantity
            return product != null && product.AvailableQuantity >= requestedQuantity;
        }
    }

}
