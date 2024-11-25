using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MTOGO_API_Service.Data;

namespace MTOGO_Api_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        DBManager _dbManager = new DBManager();

        [HttpPost("add")]
        public ActionResult AddOrder([FromBody] Order order)
        {
            try
            {
                _dbManager.AddOrder(order);
                return Ok("Order added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("restaurant/{restaurantId}")]
        public ActionResult<List<Order>> GetOrdersByRestaurant(string restaurantId)
        {
            try
            {
                var orders = _dbManager.GetOrdersByRestaurant(ObjectId.Parse(restaurantId));
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
