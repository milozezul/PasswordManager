using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PManager.Interfaces.Services;
using SharedModels.APIs.Data.Inputs;
using SharedModels.APIs.Data.Outputs;
using SharedModels.Database;

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

        [HttpPost(CreateRecordInput.Api)]
        public async Task<ActionResult<Record>> CreateRecord(CreateRecordInput model)
        {
            try
            {
                var record = await _dataService.CreateRecord(model);

                if (record == null) return StatusCode(409);

                return record;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        [HttpPost(PasswordAddInput.Api)]
        public async Task<ActionResult<Password>> AddPasswordToRecord(PasswordAddInput model)
        {
            try
            {
                var createdPassword = await _dataService.AddPassword(model);

                if (createdPassword == null) return StatusCode(409);

                return createdPassword;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        [HttpPost(NoteDataCreateInput.Api)]
        public async Task<IActionResult> AddNoteToPassword(NoteDataCreateInput model)
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

        [HttpPost(RecordPasswordsInput.Api)]
        public async Task<ActionResult<RecordPasswordsOutput>> GetRecordPasswords(RecordPasswordsInput model)
        {
            try
            {
                var passwords = await _dataService.GetPasswordsByRecordId(model);

                if (passwords == null) return StatusCode(404);

                return passwords;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        [HttpPost(PasswordDataInput.Api)]
        public async Task<ActionResult<DecryptedPasswordOutput>> GetPasswordByPasswordId(PasswordDataInput model)
        {
            try
            {
                var password = await _dataService.GetPasswordByPasswordId(model);

                if (password == null) return StatusCode(404);

                return password;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        [HttpPost(EditRecordNameInput.Api)]
        public async Task<IActionResult> EditRecordName(EditRecordNameInput model)
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

        [HttpPost(CategoryInput.Api)]
        public async Task<ActionResult<Category>> CreateCategory(CategoryInput model)
        {
            try
            {
                var createdCategory = await _dataService.CreateCategory(model);

                if (createdCategory == null) return StatusCode(409);

                return createdCategory;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        [HttpPost(EditCategoryNameInput.Api)]
        public async Task<IActionResult> EditCategoryName(EditCategoryNameInput model)
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

        [HttpPost(EditCategoryDescriptionInput.Api)]
        public async Task<IActionResult> EditCategoryDescrption(EditCategoryDescriptionInput model)
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

        [HttpPost(PasswordDiactivateInput.Api)]
        public async Task<IActionResult> DiactivatePassword(PasswordDiactivateInput model)
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

        [HttpPost(PasswordActivateInput.Api)]
        public async Task<IActionResult> ActivatePassword(PasswordActivateInput model)
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

        [HttpPost(PasswordReencryptInput.Api)]
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

        [HttpPost(NoteDataDeleteInput.Api)]
        public async Task<IActionResult> DeletePassword(NoteDataDeleteInput model)
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
