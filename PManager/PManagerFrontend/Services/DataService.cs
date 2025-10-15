using PManagerFrontend.Interfaces.Services;
using PManagerFrontend.Models.Components;
using SharedModels.APIs.Data.Inputs;
using SharedModels.APIs.Data.Outputs;
using SharedModels.APIs.Explorer.Outputs;
using SharedModels.Database;
using SharedModels.Interfaces;
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

        public async Task<Category?> CreateNewCategory(CategoryInput input)
        {
            return await PostAsync<CategoryInput, Category?>(input);
        }

        public async Task<Record?> CreateRecord(CreateRecordInput input)
        {
            return await PostAsync<CreateRecordInput, Record?>(input);
        }

        public async Task<RecordPasswordsOutput> GetPasswordsByRecordId(RecordPasswordsInput input)
        {
            return await PostAsync<RecordPasswordsInput, RecordPasswordsOutput>(input);
        }

        public async Task<DecryptedPasswordOutput?> GetPasswordsByPasswordId(PasswordDataInput input)
        {
            return await PostAsync<PasswordDataInput, DecryptedPasswordOutput?>(input);
        }

        public async Task<Password?> AddPassword(PasswordAddInput input)
        {
            return await PostAsync<PasswordAddInput, Password?>(input);
        }

        public async Task<bool> AddNoteToPassword(NoteDataCreateInput input)
        {
            return await PostAsync(input);
        }

        public async Task<bool> DiactivatePassword(PasswordDiactivateInput input)
        {
            return await PostAsync(input);
        }

        public async Task<bool> ActivatePassword(PasswordActivateInput input)
        {
            return await PostAsync(input);
        }

        public async Task<List<GroupFolderModel>> GetAllRecords()
        {
            var model = await GetListAsync<CategoryRecords>();

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

        public async Task<bool> EditRecordName(EditRecordNameInput input)
        {
            return await PostAsync(input);
        }

        public async Task<bool> EditCategoryName(EditCategoryNameInput input)
        {
            return await PostAsync(input);
        }

        public async Task<bool> EditCategoryDescription(EditCategoryDescriptionInput input)
        {
            return await PostAsync(input);
        }

        public async Task<bool> ReencryptPassword(PasswordReencryptInput input)
        {
            return await PostAsync(input);
        }

        public async Task<bool> DeletePasswordNote(NoteDataDeleteInput input)
        {
            return await PostAsync(input);
        }

        public async Task<List<T>?> GetListAsync<T>() where T: IGetApiRoute
        {
            using (var client = _factory.CreateClient("api"))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _state.JwtBearer);
                try
                {
                    var response = await client.GetAsync("api/Explorer/" + T.Api);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonSerializer.Deserialize<List<T>>(responseContent);
                    }
                    return default;
                }
                catch (Exception ex)
                {
                    return default;
                }
            }
        }
        public async Task<bool> PostAsync<T>(T input) where T: IPostApiRoute
        {
            using (var client = _factory.CreateClient("api"))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _state.JwtBearer);
                try
                {
                    string json = JsonSerializer.Serialize(input);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("api/Data/" + T.Api, content);
                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public async Task<U?> PostAsync<T,U>(T input) where T: IPostApiRoute
        {
            using (var client = _factory.CreateClient("api"))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _state.JwtBearer);
                try
                {
                    var json = JsonSerializer.Serialize(input);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("api/Data/" + T.Api, content);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonSerializer.Deserialize<U>(responseContent);
                    }
                    return default;
                }
                catch (Exception ex)
                {
                    return default;
                }
            }
        }
    }
}
