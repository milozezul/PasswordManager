using PManagerFrontend.Interfaces.Services;
using SharedModels.Database;
using System.Text.Json;

namespace PManagerFrontend.Services
{
    public class DataService : IDataService
    {
        IHttpClientFactory _factory;
        public DataService(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<List<Category>> GetCategories()
        {
            using (var client = _factory.CreateClient("api"))
            {
                try
                {
                    var response = await client.GetAsync("api/Data/categories");

                    if (response.IsSuccessStatusCode)
                    {
                        var str = await response.Content.ReadAsStringAsync();

                        var model = JsonSerializer.Deserialize<List<Category>>(str);

                        return model;
                    }
                    return new List<Category>();
                }
                catch (Exception ex)
                {
                    return new List<Category>();
                }
            }
        }
    }
}
