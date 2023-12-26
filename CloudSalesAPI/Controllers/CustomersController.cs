using CloudSalesAPI.Models;
using CloudSalesAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CloudSalesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IAdministrationService _administration;

        public CustomersController(IAdministrationService administration)
        {
            _administration = administration;
        }

        [HttpGet("GetCustomer")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _administration.GetCustomerById(id);
            if (customer == null) {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPost("AddCustomer")]
        public async Task<ActionResult<int>> AddCustomer(string customerName)
        {
            if (string.IsNullOrEmpty(customerName)) {
                return BadRequest("customer name can't be empty");
            }
            var customerId = await _administration.CreateCustomer(customerName);
            return Ok(customerId);
        }
    }
}
