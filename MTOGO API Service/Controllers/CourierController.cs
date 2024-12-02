using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MTOGO_API_Service.Data;

namespace MTOGO_Api_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourierController : Controller
    {
        DBManager _dbManager = new DBManager();
        private static List<Courier> couriers = new List<Courier>();

        [HttpPost("add")]
        public ActionResult<Courier> AddNewCourier([FromBody] Courier courier)
        {
            try
            {
                _dbManager.AddCourier(courier);
                return Ok(courier);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("{email}")]
        public ActionResult<Courier> GetCourierByEmail(string email)
        {
            try
            {
                var courier = _dbManager.GetCourierByEmail(email);
                return Ok(courier);
            }
            catch (InvalidDataException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpDelete("{courierId}")]
        public IActionResult DeleteCourier(string courierId)
        {
            try
            {
                var id = ObjectId.Parse(courierId);
                _dbManager.DeleteCourier(id);
                return Ok("Courier deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut("{courierId}/update-info")]
        public IActionResult UpdateCourierInfo(string courierId, [FromBody] Courier updatedCourier)
        {
            try
            {
                var id = ObjectId.Parse(courierId);

                // Fjern AssignedDeliveries for at sikre, at det ikke opdateres
                updatedCourier.AssignedDeliveries = null;

                // Kald DBManager-metoden for at opdatere kureren
                _dbManager.UpdateCourier(id, updatedCourier);

                return Ok("Courier information updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut("{courierId}/update-status")]
        public IActionResult UpdateCourierStatus(string courierId, [FromBody] Courier updatedCourierStatus)
        {
            try
            {
                var id = ObjectId.Parse(courierId);
                updatedCourierStatus.AssignedDeliveries = null;

                _dbManager.UpdateCourierStatus(id, updatedCourierStatus);
                return Ok("Courier's status updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
