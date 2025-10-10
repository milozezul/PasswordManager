using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PManager.Interfaces.Services;
using SharedModels.InputModels;

namespace PManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class DataController : ControllerBase
    {
        IDataService _dataService;
        public DataController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost("records/create/{category:int}")]
        public async Task<IActionResult> CreateRecord(int category, [FromQuery] string name, [FromQuery] string url, [FromQuery] string username)
        {
            try
            {
                var record = await _dataService.CreateRecord(category, name, url, username);

                if (record == null) return StatusCode(409);

                return StatusCode(201, record);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        [HttpPost("records/password")]
        public async Task<IActionResult> AddPasswordToRecord(PasswordAddInputModel model)
        {
            try
            {
                var createdPassword = await _dataService.AddPassword(model);

                if (createdPassword == null) return StatusCode(409);

                return StatusCode(201, createdPassword);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        [HttpPost("records/password/note")]
        public async Task<IActionResult> AddNoteToPassword(NoteInputModel model)
        {
            try
            {
                var result = await _dataService.AddNoteToPassword(model);

                if (result)
                {
                    return StatusCode(201);
                }
                else
                {
                    return StatusCode(406);
                }
                
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

        [HttpPost("record/password/get")]
        public async Task<IActionResult> GetPasswordByPasswordId(PasswordGetOutputModel model)
        {
            try
            {
                var password = await _dataService.GetPasswordByPasswordId(model);

                if (password == null) return StatusCode(404);

                return StatusCode(200, password);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        [HttpPost("records/{recordId:int}/edit/name")]
        public async Task<IActionResult> EditRecordName(int recordId, [FromQuery] string name)
        {
            try
            {
                var result = await _dataService.EditRecordName(recordId, name);
                if (result)
                {
                    return StatusCode(200);
                }
                else
                {
                    return StatusCode(406);
                }
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

        [HttpPost("categories/edit/name/{id:int}")]
        public async Task<IActionResult> EditCategoryName(int id, [FromQuery] string name)
        {
            try
            {
                var isSuccess = await _dataService.EditCategoryName(id, name);
                if (isSuccess)
                {
                    return StatusCode(200);
                }
                else
                {
                    return StatusCode(406);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        [HttpPost("categories/edit/description/{id:int}")]
        public async Task<IActionResult> EditCategoryDescrption(int id, [FromQuery] string description)
        {
            try
            {
                var isSuccess = await _dataService.EditCategoryDescription(id, description);
                if (isSuccess)
                {
                    return StatusCode(200);
                }
                else
                {
                    return StatusCode(406);
                }
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

        [HttpPost("password/reencrypt")]
        public async Task<IActionResult> ReencryptPassword(PasswordReencryptInputModel model)
        {
            try
            {
                var result = await _dataService.ReencryptPassword(model);

                if (result)
                {
                    return StatusCode(200);
                }
                else
                {
                    return StatusCode(406);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error {ex.Message}");
            }
        }

        [HttpPost("password/delete")]
        public async Task<IActionResult> DeletePassword(NoteDeleteInput model)
        {
            try
            {
                var result = await _dataService.DeletePasswordNote(model);

                if (result)
                {
                    return StatusCode(200);
                }
                else
                {
                    return StatusCode(406);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error {ex.Message}");
            }
        }
    }
}
