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

        [HttpPost("add")]
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

        [HttpGet("{restaurantId}")]
        public ActionResult<Restaurant> GetRestaurantById(string restaurantId)
        {
            try
            {
                var id = ObjectId.Parse(restaurantId);
                var restaurant = _dbManager.GetRestaurantById(id);
                if (restaurant == null)
                {
                    return NotFound("Restaurant not found.");
                }
                return Ok(restaurant);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost("{restaurantId}/menu/add")]
        public ActionResult AddMenuToRestaurant(string restaurantId, [FromBody] Menu menu)
        {
            try
            {
                var id = ObjectId.Parse(restaurantId);
                _dbManager.AddMenuToRestaurant(id, menu);
                return Ok("Menu added to restaurant successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpDelete("{restaurantId}")]
        public IActionResult DeleteRestaurant(string restaurantId)
        {
            try
            {
                var id = ObjectId.Parse(restaurantId);
                _dbManager.DeleteRestaurant(id);
                return Ok("Restaurant deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut("{restaurantId}/update-info")]
        public IActionResult UpdateRestaurantInfo(string restaurantId, [FromBody] Restaurant updatedRestaurant)
        {
            try
            {
                var id = ObjectId.Parse(restaurantId);

                // Kald DBManager-metoden for at opdatere specifikke felter
                _dbManager.UpdateRestaurant(id, updatedRestaurant);

                return Ok("Restaurant information, including ContactInfo, updated successfully without modifying the menu.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost("{restaurantId}/menu/{menuId}/menu-item")]
        public IActionResult AddMenuItemToMenu(string restaurantId, string menuId, [FromBody] MenuItem menuItem)
        {
            try
            {
                // Kald metoden for at tilføje en menuitem til menuen
                _dbManager.AddMenuItemToRestaurantMenu(restaurantId, ObjectId.Parse(menuId), menuItem);

                return Ok(new { message = "Menu item successfully added to the menu." });
            }
            catch (Exception ex)
            {
                // Returner en bad request med fejlbeskeden
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
