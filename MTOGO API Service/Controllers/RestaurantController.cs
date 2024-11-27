using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MTOGO_API_Service.Data;

namespace MTOGO_Api_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantController : Controller
    {
        DBManager _dbManager = new DBManager();

        [HttpPost("{restaurantId}/menu/add")]
        public ActionResult AddMenuToRestaurant(string restaurantId, [FromBody] Menu menu)
        {
            try
            {
                _dbManager.AddMenuToRestaurant(int.Parse(restaurantId), menu);
                return Ok("Menu added to restaurant successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost("{restaurantId}/menu/menuitem/add")]
        public ActionResult AddMenuItemToRestaurantMenu(string restaurantId, string menuId, [FromBody] MenuItem menuItem)
        {
            try
            {
                if (string.IsNullOrEmpty(menuId))
                {
                    return BadRequest("menuId cannot be null or empty.");
                }

                // Konverter menuId fra string til ObjectId
                var menuObjectId = ObjectId.Parse(menuId);

                // Kald DBManager-metoden
                _dbManager.AddMenuItemToRestaurantMenu(restaurantId, menuObjectId, menuItem);

                return Ok("Menu item added to restaurant's menu successfully.");
            }
            catch (FormatException)
            {
                return BadRequest("Invalid menuId format.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("search/by-name")]
        public ActionResult<Restaurant> GetRestaurantByName([FromQuery] string name)
        {
            try
            {
                var restaurant = _dbManager.GetRestaurantByName(name);
                if (restaurant == null)
                {
                    return NotFound("Restaurant not found");
                }
                return Ok(restaurant);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost("restaurant/add")]
        public ActionResult<Restaurant> AddRestaurant([FromBody] Restaurant restaurant)
        {
            try
            {
                _dbManager.AddRestaurant(restaurant);
                return Ok(restaurant);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        //[HttpGet("restaurant/{restaurantId}")]
        //public ActionResult<Restaurant> GetRestaurantById(string ownerId)
        //{
        //    try
        //    {
        //        var restaurant = _dbManager.GetRestaurantById(int.Parse(ownerId));
        //        if (restaurant == null)
        //            return NotFound("Restaurant not found");

        //        return Ok(restaurant);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Error: {ex.Message}");
        //    }
        //}

        [HttpGet("allrestaurants")]
        public ActionResult<List<Restaurant>> GetAllRestaurants()
        {
            try
            {
                return Ok(_dbManager.GetAllRestaurants());
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

    }
}
