namespace CloudSalesAPI.Models
{
    public class PurchasedLicense { 
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; } 
        public int Quantity { get; set; }
        public LicenseStatus Status { get; set; }
        public DateTime ValidToDate { get; set; }
        public int CustomerAccountID { get; set; }
}

    public enum LicenseStatus
    {
        Active,
        Canceled
    }
}
