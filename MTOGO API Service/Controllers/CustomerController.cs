using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MTOGO_API_Service.Data;

namespace MTOGO_Api_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        DBManager _dbManager = new DBManager();

        [HttpPost("add")]
        public ActionResult<Customer> AddNewCustomer([FromBody] Customer customer)
        {
            try
            {
                _dbManager.AddCustomer(customer);
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("{customerId}")]
        public ActionResult<Customer> GetCustomerById(string customerId)
        {
            try
            {
                var id = ObjectId.Parse(customerId);
                var customer = _dbManager.GetCustomerById(id);
                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpDelete("{customerId}")]
        public IActionResult DeleteCustomer(string customerId)
        {
            try
            {
                var id = ObjectId.Parse(customerId);
                _dbManager.DeleteCustomer(id);
                return Ok("Customer deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut("{customerId}")]
        public IActionResult UpdateCustomer(string customerId, [FromBody] Customer updatedCustomer)
        {
            try
            {
                var id = ObjectId.Parse(customerId);
                _dbManager.UpdateCustomer(id, updatedCustomer);
                return Ok("Customer updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
