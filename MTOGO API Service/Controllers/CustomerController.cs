using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTOGO_API_Service.Data;

namespace MTOGO_Api_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        DBManager _dbManager = new DBManager();

        [HttpPost("customer/add")]
        public ActionResult<Customer> addNewCustomer([FromBody] Customer customer)
        {
            _dbManager.addCustomer(customer);
            return Ok(customer);
        }
    }
}
