using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PManager.Interfaces.Services;
using SharedModels.InputModels;

namespace PManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        IAuthService _auth;
        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginInput input)
        {
            try
            {
                var bearer = await _auth.Login(input);
                if (!string.IsNullOrEmpty(bearer))
                {
                    return StatusCode(200, new ResponseWrapper<string>()
                    {
                        Value = bearer,
                        Message = ""
                    });
                }
                else
                {
                    return StatusCode(404, new ResponseWrapper<string>()
                    {
                        Value = "",
                        Message = "User credentials didnt match"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(LoginInput input)
        {
            try
            {
                var user = await _auth.CreateUser(input);
                if (user != null)
                {
                    return StatusCode(201, new ResponseWrapper<string>()
                    {
                        Value = user.Id.ToString(),
                        Message = ""
                    });
                }
                else
                {
                    return StatusCode(409, new ResponseWrapper<string>() {
                        Value = "",
                        Message = "Failed to register user, change username"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }
    }
}
