using CloudSalesAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CloudSalesAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductLicencesController : ControllerBase
    {

        private readonly IProvisioningService _provisioningService;

        public ProductLicencesController(IProvisioningService provisioningService)
        {
            _provisioningService = provisioningService;
        }

        // GET: api/<ProductController>
        [HttpGet("GetAvailableServices")]
        public ActionResult<IEnumerable<Product>> GetAllAvailableServices()
        {
            var products = _provisioningService.GetAvailableProducts();
            return Ok(products);
        }

        // GET api/<ProductController>/provisioning/5
        [HttpGet("GetLicencesPerAccount")]
        public async Task<ActionResult<List<Product>>> Get(int accountId)
        {
            var products = await _provisioningService.GetPurchasedProductLicenses(accountId);
            return Ok(products);
        }

        // order software license through CCP
        // POST api/products/purchase
        [HttpPost("purchaseLicense")]
        public async Task<IActionResult> Post([FromBody] PurchaseLicenseRequest request)
        {
            try
            {
                var success = await _provisioningService.PurchaseProduct(request.AccountID, request.ProductID, request.Quantity);

                if (success)
                {
                    return Ok("Product license purchased successfully.");
                }
                else
                {
                    return BadRequest("Insufficient quantity or product not available.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // PUT api/products/change-quantity
        [HttpPut("change-quantity")]
        public async Task<IActionResult> Put(int externalProductId, int accountId, int quantity)
        {
            try
            {
                await _provisioningService.ChangeProductLicenseQuantity(accountId, externalProductId, quantity);
                return Ok("Licence quantity has been changed.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("CancelServiceLicense")]
        public IActionResult CancelServicetLicense(int licenceProductId, int accountId)
        {
            try
            {
                _provisioningService.CancelProductLicense(accountId, licenceProductId);
                return Ok("License canceled successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("ExtendServicelicense")]
        public async Task<IActionResult> ExtendServiceLicense(int licenceProductId, int accountId)
        {
            try
            {
                await _provisioningService.ExtendProductLicense(accountId, licenceProductId);
                return Ok("License extended successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");

            }
        }
    }
}
