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

        [HttpPost("records/create")]
        public async Task<IActionResult> CreateRecord(CreateRecordInput model)
        {
            try
            {
                var record = await _dataService.CreateRecord(model);

                if (record == null) return StatusCode(409);

                return StatusCode(201, record);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        [HttpPost("records/password")]
        public async Task<IActionResult> AddPasswordToRecord(PasswordAddInput model)
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

        [HttpPost("records")]
        public async Task<IActionResult> GetRecordPasswords(RecordPasswordsInput model)
        {
            try
            {
                var passwords = await _dataService.GetPasswordsByRecordId(model);

                if (passwords == null) return StatusCode(404);

                return StatusCode(200, passwords);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        [HttpPost("record/password/get")]
        public async Task<IActionResult> GetPasswordByPasswordId(PasswordLocationInput model)
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

        [HttpPost("records/edit/name")]
        public async Task<IActionResult> EditRecordName(EditInput model)
        {
            try
            {
                var result = await _dataService.EditRecordName(model);
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

        [HttpPost("categories/edit/name")]
        public async Task<IActionResult> EditCategoryName(EditInput model)
        {
            try
            {
                var isSuccess = await _dataService.EditCategoryName(model);
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

        [HttpPost("categories/edit/description")]
        public async Task<IActionResult> EditCategoryDescrption(EditInput model)
        {
            try
            {
                var isSuccess = await _dataService.EditCategoryDescription(model);
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
        public async Task<IActionResult> DiactivatePassword(PasswordStatusInput model)
        {
            try
            {
                await _dataService.DeactivatePassword(model);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error {ex.Message}");
            }
        }

        [HttpPost("password/activate")]
        public async Task<IActionResult> ActivatePassword(PasswordStatusInput model)
        {
            try
            {
                await _dataService.ActivatePassword(model);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error {ex.Message}");
            }
        }

        [HttpPost("password/reencrypt")]
        public async Task<IActionResult> ReencryptPassword(PasswordReencryptInput model)
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
