using PManagerFrontend.Interfaces.Services;
using SharedModels.APIs.DirectAccess.Input;
using SharedModels.InputModels;
using System.Text;
using System.Text.Json;

namespace PManagerFrontend.Services
{
    public class DirectLinkService : IDirectLinkService
    {
        IHttpClientFactory _factory;
        IStateManager _state;
        public DirectLinkService(IHttpClientFactory factory, IStateManager state)
        {
            _factory = factory;
            _state = state;
        }

        public async Task<ResponseWrapper<string>> GetDirectToken(LinkInput input)
        {
            using (var client = _factory.CreateClient("api"))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _state.JwtBearer);
                try
                {
                    var json = JsonSerializer.Serialize(input);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("api/DirectAccess/link", content);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var model = JsonSerializer.Deserialize<ResponseWrapper<string>>(responseContent);
                        return model;
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
