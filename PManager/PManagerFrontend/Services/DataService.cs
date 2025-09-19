using PManagerFrontend.Interfaces.Services;
using SharedModels.Database;
using SharedModels.DataService;
using SharedModels.InputModels;
using System.Text;
using System.Text.Json;

namespace PManagerFrontend.Services
{
    public class DataService : IDataService
    {
        IHttpClientFactory _factory;
        IStateManager _state;
        public DataService(IHttpClientFactory factory, IStateManager state)
        {
            _factory = factory;
            _state = state;
        }

        public async Task<List<Category>> GetCategories()
        {
            //replace emty returns with nulls and theows to next catch
            using (var client = _factory.CreateClient("api"))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _state.JwtBearer);
                try
                {
                    var response = await client.GetAsync("api/Data/categories");
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {                        
                        var model = JsonSerializer.Deserialize<List<Category>>(responseContent);
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

        public async Task<Category?> CreateNewCategory(string category)
        {
            //replace emty returns with nulls and theows to next catch
            using (var client = _factory.CreateClient("api"))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _state.JwtBearer);
                try
                {
                    var response = await client.PostAsync("api/Data/categories/" + category, null);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var model = JsonSerializer.Deserialize<Category>(responseContent);
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

        public async Task<List<Record>> GetRecordsByCategory(string category)
        {
            //replace emty returns with nulls and theows to next catch
            using (var client = _factory.CreateClient("api"))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _state.JwtBearer);
                try
                {
                    var response = await client.GetAsync("api/Data/records/" + category);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {                        
                        var model = JsonSerializer.Deserialize<List<Record>>(responseContent);
                        return model;
                    }
                    return new List<Record>();
                }
                catch (Exception ex)
                {
                    return new List<Record>();
                }
            }
        }

        public async Task<Record?> CreateRecord(string name, string url, string category)
        {
            //replace emty returns with nulls and theows to next catch
            using (var client = _factory.CreateClient("api"))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _state.JwtBearer);
                try
                {
                    var response = await client.PostAsync($"api/Data/records/{category}?name={name}&url={url}", null);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var model = JsonSerializer.Deserialize<Record>(responseContent);
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

        public async Task<RecordPasswordsModel?> CreateRecordWithPassword(string name, string url, string category, string lockpassword, string newpassword)
        {
            //replace emty returns with nulls and theows to next catch
            using (var client = _factory.CreateClient("api"))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _state.JwtBearer);
                try
                {
                    PasswordParametersModel input = new PasswordParametersModel()
                    {
                        Password = lockpassword,
                        NewPassword = newpassword
                    };
                    var json = JsonSerializer.Serialize(input);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync($"api/Data/records/{category}/password?name={name}&url={url}", content);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var model = JsonSerializer.Deserialize<RecordPasswordsModel>(responseContent);
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

        public async Task<RecordPasswordsModel> GetPasswordsByRecordId(int id, string lockpassword)
        {
            //replace emty returns with nulls and theows to next catch
            using (var client = _factory.CreateClient("api"))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _state.JwtBearer);
                try
                {
                    var input = new PasswordInputModel()
                    {
                        Password = lockpassword
                    };
                    var json = JsonSerializer.Serialize(input);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("api/Data/records/" + id, content);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {                        
                        var model = JsonSerializer.Deserialize<RecordPasswordsModel>(responseContent);
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

        public async Task<Password?> AddPassword(string lockpassword, string newpassword, int recordId)
        {
            //replace emty returns with nulls and theows to next catch
            using (var client = _factory.CreateClient("api"))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _state.JwtBearer);
                try
                {
                    PasswordParametersModel input = new PasswordParametersModel()
                    {
                        Password = lockpassword,
                        NewPassword = newpassword
                    };
                    var json = JsonSerializer.Serialize(input);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync($"api/Data/records/password/{recordId}", content);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var model = JsonSerializer.Deserialize<Password>(responseContent);
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
