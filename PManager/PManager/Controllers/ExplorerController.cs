using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PManager.Interfaces.Services;
using SharedModels.APIs.Explorer.Outputs;

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

        [HttpGet(CategoryRecords.Api)]
        public async Task<ActionResult<List<CategoryRecords>>> GetAllRecords()
        {
            try
            {
                return await _data.GetAllRecords();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }
    }
}
