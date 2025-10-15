using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PManager.Interfaces.Services;
using SharedModels.APIs.DirectAccess.Input;
using SharedModels.InputModels;

namespace PManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class DirectAccessController : ControllerBase
    {
        IDirectAccessService _direct;
        public DirectAccessController(IDirectAccessService direct)
        {
            _direct = direct;
        }

        [Authorize(Roles = "User")]
        [HttpPost("link")]
        public async Task<IActionResult> GenerateDirectLink(LinkInput input)
        {
            try
            {
                var result = _direct.GetDirectLinkToken(input);

                if (result == null) return StatusCode(404);

                return StatusCode(200, new ResponseWrapper<string>()
                {
                    Value = result,
                    Message = ""
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Link")]
        [HttpPost("enter")]
        public async Task<IActionResult> GetPasswordDirectly(PasswordInput input)
        {
            try
            {
                var password = await _direct.GetDirectPassword(input.Password);

                if (password == null) return StatusCode(404);

                return StatusCode(200, password);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }
    }
}
