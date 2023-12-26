using CloudSalesAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CloudSalesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAdministrationService _administration;

        public AccountsController(IAdministrationService administration)
        {
            _administration = administration;
        }

        // GET: api/<AccountController>
        [HttpGet("GetAllAccountsByCustomerID")]
        public async Task<ActionResult<IEnumerable<CustomerAccount>>> GetAllAccounts(int customerId)
        {
            var accounts = await _administration.GetAccounts(customerId);
            return Ok(accounts);
        }

        // POST api/<AccountController>
        [HttpPost("AddAccountToCustomer")]
        public async Task<ActionResult> Post(int customerId, string accountName)
        {
            try
            {
                await _administration.CreateAccount(customerId, accountName);
                return Ok("Account created successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                // Return a generic error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        } 
    }
}
