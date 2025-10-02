using PManagerFrontend.Interfaces.Services;
using SharedModels.InputModels;
using System.Text;
using System.Text.Json;

namespace PManagerFrontend.Services
{
    public class AuthService : IAuthService
    {
        IHttpClientFactory _factory;
        IStateManager _state;
        public AuthService(IHttpClientFactory factory, IStateManager state)
        {
            _factory = factory;
            _state = state;
        }

        public async Task<ResponseWrapper<bool>> Register(RegisterInput input)
        {
            using (var client = _factory.CreateClient("api"))
            {
                try
                {
                    var json = JsonSerializer.Serialize(input);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("api/Auth/register", content);
                    var jsonRes = await response.Content.ReadAsStringAsync();
                    var model = JsonSerializer.Deserialize<ResponseWrapper<bool>>(jsonRes);
                    return model;                    
                }
                catch (Exception ex)
                {
                    return new ResponseWrapper<bool>()
                    {
                        Value = false,
                        Message = "Failed request."
                    };
                }
            }
        }

        public async Task<LoginResponse> Login(LoginInput input)
        {
            using (var client = _factory.CreateClient("api"))
            {
                try
                {
                    var json = JsonSerializer.Serialize(input);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("api/Auth/login", content);
                    var jsonRes = await response.Content.ReadAsStringAsync();
                    var model = JsonSerializer.Deserialize<LoginResponse>(jsonRes);
                    if (model.IsSuccess)
                    {
                        _state.JwtBearer = model.Token;
                        _state.FirstName = model.FirstName;
                        _state.LastName = model.LastName;
                        _state.Email = model.Email;
                    }
                    return model;
                }
                catch (Exception ex)
                {
                    return new LoginResponse()
                    {
                        IsSuccess = false,
                        Token = "",
                        Message = "Failed request."
                    };
                }
            }
        }
    }
}
