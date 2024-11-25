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

        [HttpPost("create")]
        public ActionResult CreateOrder([FromBody] Order order)
        {
            try
            {
                // Valider og konverter restaurantId fra string til ObjectId
                if (!ObjectId.TryParse(order.RestaurantId.ToString(), out ObjectId restaurantObjectId))
                {
                    return BadRequest("Invalid RestaurantId format.");
                }

                // Valider og konverter customerId fra string til ObjectId
                if (!ObjectId.TryParse(order.CustomerId.ToString(), out ObjectId customerObjectId))
                {
                    return BadRequest("Invalid CustomerId format.");
                }

                // Opdatér order med konverterede ObjectIds
                order.RestaurantId = restaurantObjectId;
                order.CustomerId = customerObjectId;

                // Fortsæt med oprettelsen af ordren
                _dbManager.CreateOrder(order);
                return Ok("Order created successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost("add")]
        //public ActionResult AddOrder([FromBody] Order order)
        //{
        //    try
        //    {
        //        _dbManager.AddOrder(order);
        //        return Ok("Order added successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Error: {ex.Message}");
        //    }
        //}

        //[HttpPost("create")]
        //public ActionResult CreateOrder([FromBody] CreateOrderRequest request)
        //{
        //    try
        //    {
        //        _dbManager.AddOrder(request.CustomerId, request.RestaurantId, request.OrderItems);
        //        return Ok("Order created successfully!");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

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
