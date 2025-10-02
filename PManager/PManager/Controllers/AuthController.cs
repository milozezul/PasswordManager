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

                if (bearer.IsSuccess)
                {
                    return StatusCode(200, bearer);
                }
                else
                {
                    return StatusCode(404, bearer);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new LoginResponse()
                {
                    IsSuccess = false,
                    Token = "",
                    Message = $"Server Error: {ex.Message}"
                });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterInput input)
        {
            try
            {
                var user = await _auth.CreateUser(input);

                if (user != null)
                {
                    return StatusCode(201, new ResponseWrapper<bool>()
                    {
                        Value = true,
                        Message = ""
                    });
                }
                else
                {
                    return StatusCode(409, new ResponseWrapper<bool>() {
                        Value = false,
                        Message = "Failed to register user, try different username."
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
