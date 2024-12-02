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
        public IActionResult AddOrder([FromBody] Order order)
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

        [HttpGet("{orderId}")]
        public ActionResult<Order> GetOrderById(string orderId)
        {
            try
            {
                var id = ObjectId.Parse(orderId);
                var order = _dbManager.GetOrderById(id);
                if (order == null)
                {
                    return NotFound("Order not found.");
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpDelete("{orderId}")]
        public IActionResult DeleteOrder(string orderId)
        {
            try
            {
                var id = ObjectId.Parse(orderId);
                _dbManager.DeleteOrder(id);
                return Ok("Order deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut("{orderId}/update-info")]
        public IActionResult UpdateOrderInfo(string orderId, [FromBody] Order updatedOrder)
        {
            try
            {
                var id = ObjectId.Parse(orderId);

                // Kald DBManager-metoden for at opdatere ordren
                _dbManager.UpdateOrderInfo(id, updatedOrder);

                return Ok("Order information updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
