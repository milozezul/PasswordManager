using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PManager.Models;
using PManager.Models.Configs;

namespace PManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return StatusCode(200);
        }
    }
}
