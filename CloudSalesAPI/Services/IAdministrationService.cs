namespace CloudSalesAPI.Services
{
    public interface IAdministrationService
    {
        // Create a new customer
        Task<int> CreateCustomer(string customerName);

        // Get customer details by customer ID
        Task<Customer> GetCustomerById(int customerId);

        // Get all accounts for specific customer
        Task<List<CustomerAccount>> GetAccounts(int customerId);

        // Create a new account
        Task CreateAccount(int customerId, string accountName);

        // Get account details by account ID
        Task<CustomerAccount> GetAccountById(int accountId);

        // Update account details (e.g., customer name)
        Task UpdateAccount(int accountId, string newAccountName);

        // Delete an account by account ID
        Task DeleteAccount(int accountId);
    }

}
