using PManagerFrontend.Interfaces.Services;
using PManagerFrontend.Models.Components;
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

        public async Task<Category?> CreateNewCategory(string category)
        {
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

        public async Task<Record?> CreateRecord(string name, string url, string username, int category)
        {
            using (var client = _factory.CreateClient("api"))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _state.JwtBearer);
                try
                {
                    var response = await client.PostAsync($"api/Data/records/create/{category}?name={name}&url={url}&username={username}", null);
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

        public async Task<RecordPasswordsModel> GetPasswordsByRecordId(int id, string lockpassword)
        {
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

        public async Task<bool> DiactivatePassword(int recordId, int passwordId, string password)
        {
            using (var client = _factory.CreateClient("api"))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _state.JwtBearer);
                try
                {
                    PasswordInputModel input = new PasswordInputModel()
                    {
                        Password = password
                    };
                    string json = JsonSerializer.Serialize(input);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync($"api/Data/password/diactivate?recordId={recordId}&passwordId={passwordId}", content);
                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public async Task<bool> ActivatePassword(int recordId, int passwordId, string password)
        {
            using (var client = _factory.CreateClient("api"))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _state.JwtBearer);
                try
                {
                    PasswordInputModel input = new PasswordInputModel()
                    {
                        Password = password
                    };
                    string json = JsonSerializer.Serialize(input);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync($"api/Data/password/activate?recordId={recordId}&passwordId={passwordId}", content);
                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public async Task<List<GroupFolderModel>> GetAllRecords()
        {
            using (var client = _factory.CreateClient("api"))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _state.JwtBearer);
                try
                {
                    var response = await client.GetAsync("api/Explorer/records");
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var model = JsonSerializer.Deserialize<List<CategoryRecords>>(responseContent);
                        var result = new List<GroupFolderModel>();
                        foreach (var group in model ?? Enumerable.Empty<CategoryRecords>())
                        {
                            var records = new List<RecordFolderModel>();
                            foreach (var record in group.Records ?? Enumerable.Empty<Record>())
                            {
                                records.Add(new RecordFolderModel()
                                {
                                    Record = record,
                                    IsExpand = false,
                                    Passwords = null
                                });
                            }
                            result.Add(new GroupFolderModel()
                            {
                                Category = group.Category,
                                IsExpand = false,
                                Records = records
                            });
                        }
                        return result;
                    }
                    return new List<GroupFolderModel>();
                }
                catch (Exception ex)
                {
                    return new List<GroupFolderModel>();
                }
            }
        }
    }
}
