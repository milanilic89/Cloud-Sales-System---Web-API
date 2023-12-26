namespace CloudSalesAPI.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public List<CustomerAccount> LinkedAccounts { get; set; } = new List<CustomerAccount>();
    }
}
