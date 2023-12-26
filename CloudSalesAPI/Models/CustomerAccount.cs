namespace CloudSalesAPI.Models
{
    public class CustomerAccount
    {
        public int CustomerAccountID { get; set; }
        public int CustomerID { get; set; } 
        public string AccountName { get; set; } 
        public List<PurchasedLicense> PurchasedLicense { get; set; } 
    }
}
