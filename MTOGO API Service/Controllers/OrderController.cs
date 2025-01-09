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
        private readonly IDBManager _dbManager;

        public OrderController(IDBManager dbManager)
        {
            _dbManager = dbManager;
        }

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

        [HttpPost("add-items/{orderId}")]
        public IActionResult AddMenuItemsToOrder(string orderId, [FromBody] List<MenuItem> menuItems)
        {
            try
            {
                // Valider og konverter orderId fra string til ObjectId
                if (!ObjectId.TryParse(orderId, out var orderObjectId))
                {
                    return BadRequest("Invalid orderId format.");
                }

                // Valider og konverter MenuItemId i hvert menuItem
                foreach (var menuItem in menuItems)
                {
                    // Valider og konverter MenuItemId (hvis det er en string, konverter til ObjectId)
                    if (menuItem.MenuItemId.ToString() is string menuItemIdString)
                    {
                        // Forsøg at konvertere MenuItemId fra string til ObjectId
                        if (!ObjectId.TryParse(menuItemIdString, out var menuItemObjectId))
                        {
                            return BadRequest($"Invalid MenuItemId format for item {menuItem.MenuItemName}");
                        }
                        // Opdater menuItemId med den konverterede ObjectId
                        menuItem.MenuItemId = menuItemObjectId;
                    }
                }

                // Kald DBManager-metoden for at opdatere ordren
                _dbManager.AddMenuItemsToOrder(orderObjectId, menuItems);

                return Ok("Menu items added to order successfully.");
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
