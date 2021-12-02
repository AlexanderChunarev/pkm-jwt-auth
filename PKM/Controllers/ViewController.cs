using Microsoft.AspNetCore.Mvc;

namespace PKM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ViewController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Authorized");
        }
    }
}