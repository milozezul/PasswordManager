using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PManager.Interfaces.Services;
using SharedModels.InputModels;

namespace PManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    //wrap in response wrappers
    public class DataController : ControllerBase
    {
        IDataService _dataService;
        public DataController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("records/{category}")]
        public async Task<IActionResult> GetRecords(string category)
        {
            try
            {
                var records = await _dataService.GetRecords(category);
                return StatusCode(200, records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sever Error: {ex.Message}");
            }
        }

        [HttpPost("records/{category}")]
        public async Task<IActionResult> CreateRecord(string category, [FromQuery] string name, [FromQuery] string url)
        {
            try
            {
                var record = await _dataService.CreateRecord(category, name, url);

                if (record == null) return StatusCode(409);

                return StatusCode(201, record);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        [HttpPost("records/password/{recordId:int}")]
        public async Task<IActionResult> AddPasswordToRecord(int recordId, PasswordParametersModel model)
        {
            try
            {
                var createdPassword = await _dataService.AddPassword(recordId, model.NewPassword, model.Password);

                if (createdPassword == null) return StatusCode(409);

                return StatusCode(201, createdPassword);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        [HttpPost("records/{category}/password")]
        public async Task<IActionResult> CreateRecordWithPassword(string category, [FromQuery] string name, [FromQuery] string url, PasswordParametersModel model)
        {
            try
            {
                var record = await _dataService.CreateRecordWithPassword(category, name, url, model.NewPassword, model.Password);

                if (record == null) return StatusCode(409);

                return StatusCode(201, record);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        [HttpPost("records/{recordId:int}")]
        public async Task<IActionResult> GetRecordPasswords(int recordId, PasswordInputModel model)
        {
            try
            {
                var passwords = await _dataService.GetPasswordsByRecordId(recordId, model.Password);

                if (passwords == null) return StatusCode(404);

                return StatusCode(200, passwords);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        [HttpPost("categories/{category}")]
        public async Task<IActionResult> CreateCategory(string category)
        {
            try
            {
                var createdCategory = await _dataService.CreateCategory(category);

                if (createdCategory == null) return StatusCode(409);

                return StatusCode(201, createdCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }
        
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _dataService.GetCategories();

                if (categories == null) return StatusCode(404);

                return StatusCode(200, categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        [HttpPost("password/diactivate")]
        public async Task<IActionResult> DiactivatePassword([FromQuery] int recordId, [FromQuery] int passwordId, PasswordInputModel input)
        {
            try
            {
                await _dataService.DeactivatePassword(recordId, passwordId, input.Password);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error {ex.Message}");
            }
        }

        [HttpPost("password/activate")]
        public async Task<IActionResult> ActivatePassword([FromQuery] int recordId, [FromQuery] int passwordId, PasswordInputModel input)
        {
            try
            {
                await _dataService.ActivatePassword(recordId, passwordId, input.Password);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error {ex.Message}");
            }
        }
    }
}
