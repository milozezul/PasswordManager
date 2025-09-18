using PManagerFrontend.Interfaces.Services;
using SharedModels.InputModels;
using System.Text;
using System.Text.Json;

namespace PManagerFrontend.Services
{
    public class AuthService : IAuthService
    {
        IHttpClientFactory _factory;
        public AuthService(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<ResponseWrapper<bool>> Register(LoginInput input)
        {
            using (var client = _factory.CreateClient("api"))
            {
                try
                {
                    var json = JsonSerializer.Serialize(input);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("api/Auth/register", content);
                    var jsonRes = await response.Content.ReadAsStringAsync();
                    var model = JsonSerializer.Deserialize<ResponseWrapper<string>>(jsonRes);
                    if (!string.IsNullOrEmpty(model.Value))
                    {
                        return new ResponseWrapper<bool>()
                        {
                            Value = true,
                            Message = model.Message
                        };
                    }
                    else
                    {
                        return new ResponseWrapper<bool>()
                        {
                            Value = false,
                            Message = model.Message
                        };
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<ResponseWrapper<string>> Login(LoginInput input)
        {
            using (var client = _factory.CreateClient("api"))
            {
                try
                {
                    var json = JsonSerializer.Serialize(input);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("api/Auth/login", content);
                    var jsonRes = await response.Content.ReadAsStringAsync();
                    var model = JsonSerializer.Deserialize<ResponseWrapper<string>>(jsonRes);
                    return model;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
