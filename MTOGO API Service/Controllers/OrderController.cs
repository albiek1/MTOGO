using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTOGO_API_Service.Data;

namespace MTOGO_Api_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        DBManager _dbManager = new DBManager();

        [HttpPost("order/add")]
        public ActionResult<Order> addNewOrder([FromBody] Order order)
        {
            _dbManager.addOrder(order);
            return Ok(order);
        }
    }
}
