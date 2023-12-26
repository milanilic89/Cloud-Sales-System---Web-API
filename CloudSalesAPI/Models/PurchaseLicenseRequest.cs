namespace CloudSalesAPI.Models
{
    public class PurchaseLicenseRequest
    {
        public int ProductID { get; set; }
        public int AccountID { get; set; }
        public int Quantity { get; set; }
    }
}
