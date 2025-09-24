using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PManager.Interfaces.Services;

namespace PManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class ExplorerController : ControllerBase
    {
        IDataService _data;
        public ExplorerController(IDataService data)
        {
            _data = data;
        }

        [HttpGet("records")]
        public async Task<IActionResult> GetAllRecords()
        {
            try
            {
                var categories = await _data.GetAllRecords();
                return StatusCode(200, categories);                                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }
    }
}
