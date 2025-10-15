using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PManager.Interfaces.Services;
using PManager.Models.Database;
using SharedModels.APIs.Auth.Input;
using SharedModels.APIs.Auth.Output;
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

        [HttpPost(LoginInput.Api)]
        public async Task<ActionResult<LoginResponse>> Login(LoginInput input)
        {
            try
            {
                var bearer = await _auth.Login(input);

                if (bearer.IsSuccess)
                {
                    return bearer;
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

        [HttpPost(RegisterInput.Api)]
        public async Task<ActionResult<ResponseWrapper<bool>>> Register(RegisterInput input)
        {
            try
            {
                var user = await _auth.CreateUser(input);

                if (user != null)
                {
                    return new ResponseWrapper<bool>()
                    {
                        Value = true,
                        Message = ""
                    };
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
