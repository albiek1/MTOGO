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

        [HttpPost("courier/add")]
        public ActionResult<Courier> addNewCourier([FromBody] Courier courier)
        {
            _dbManager.AddCourier(courier);
            Courier result = _dbManager.GetCourierByEmail(courier.Email);
            return Ok(result);
        }

        [HttpGet("customer/{email}")]
        public ActionResult<Customer> getCustomer(string email)
        {
            try
            {
                Courier courier = _dbManager.GetCourierByEmail(email);
                return Ok(courier);
            }
            catch (InvalidDataException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
