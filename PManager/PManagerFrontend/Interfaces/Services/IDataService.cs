using PManagerFrontend.Models.Components;
using SharedModels.Database;
using SharedModels.DataService;

namespace PManagerFrontend.Interfaces.Services
{
    public interface IDataService
    {
        Task<Category?> CreateNewCategory(string category);
        Task<Record?> CreateRecord(string name, string url, string username, int category);
        Task<RecordPasswordsModel> GetPasswordsByRecordId(int id, string lockpassword);
        Task<Password?> AddPassword(string lockpassword, string newpassword, int recordId);
        Task<bool> DiactivatePassword(int recordId, int passwordId, string password);
        Task<bool> ActivatePassword(int recordId, int passwordId, string password);
        Task<List<GroupFolderModel>> GetAllRecords();
        Task<bool> EditCategoryName(int categoryId, string name);
        Task<bool> EditCategoryDescription(int categoryId, string description);
        Task<bool> EditRecordName(int recordId, string name);
    }
}