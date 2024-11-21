using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTOGO_API_Service.Data;

namespace MTOGO_Api_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : Controller
    {
        DBManager _dbManager = new DBManager();

        [HttpPost("menuitem/add")]
        public ActionResult<MenuItem> addNewMenuItem([FromBody] MenuItem menuItem)
        {
            _dbManager.addMenuItem(menuItem);
            return Ok(menuItem);
        }
    }
}
