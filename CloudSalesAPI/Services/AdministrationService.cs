using CloudSalesAPI.Data;

namespace CloudSalesAPI.Services
{
    public class AdministrationService : IAdministrationService
    {
        private readonly IDataContext _context;

        public AdministrationService(IDataContext context)
        {
            _context = context;
        }

        public async Task CreateAccount(int customerId, string accountName)
        {
            // Check if the customer with the specified customerId exists
            _ = await _context.Customers.FindAsync(customerId) ?? throw new ArgumentException("Customer with the specified ID does not exist.");

            _context.CustomerAccounts.Add(new CustomerAccount
            {
                CustomerID = customerId,
                AccountName = accountName
            });
            await _context.SaveChangesAsync(default);
        }

        public async Task<int> CreateCustomer(string customerName)
        {
            var customer = _context.Customers.Add(new Customer
            {
                CustomerName = customerName
            });
            await _context.SaveChangesAsync(default);
            return customer.Entity.CustomerID;
        }

        public async Task DeleteAccount(int accountId)
        {
            var account = await _context.CustomerAccounts.FindAsync(accountId);
            if (account != null)
            {
                _context.CustomerAccounts.Remove(account);
                await _context.SaveChangesAsync(default);
            }
        }

        public async Task<CustomerAccount> GetAccountById(int accountId)
        {
            var account = await _context.CustomerAccounts.FindAsync(accountId);
            if (account is not null)
            {
                account.PurchasedLicense = await _context.PurchasedLicences.Where(p => p.CustomerAccountID == accountId).ToListAsync();
            }
            return account;
        }

        public async Task<List<CustomerAccount>> GetAccounts(int customerId)
        {
            var accounts = await _context.CustomerAccounts
                               .Where(c => c.CustomerID == customerId)
                               .Include(c => c.PurchasedLicense)
                               .ToListAsync();

            return accounts;
        }

        public async Task<Customer> GetCustomerById(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer is null)
            {
                return null;
            }

            return customer;

        }

        public async Task UpdateAccount(int accountId, string newAccountName)
        {
            var account = await _context.CustomerAccounts.FindAsync(accountId);
            if (account != null)
            {
                account.AccountName = newAccountName;
                await _context.SaveChangesAsync(default);
            }
        }
    }
}
