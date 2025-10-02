using SharedModels.Database;
using SharedModels.DataService;

namespace PManager.Interfaces.Services
{
    public interface IDataService
    {
        Task<Password?> AddPassword(int recordId, string newPassword, string password);
        Task<Category?> CreateCategory(string name);
        Task<Record?> CreateRecord(int categoryId, string name, string url, string username);
        Task DeactivatePassword(int recordId, int passwordId, string password);
        Task ActivatePassword(int recordId, int passwordId, string password);
        void Dispose();
        Task<RecordPasswordsModel?> GetPasswordsByRecordId(int recordId, string password);
        public int GetUserId();
        Task<Record?> GetRecordById(int id);
        int GetRecordId();
        int GetPasswordId();
        string GetFallback();
        Task<List<CategoryRecords>> GetAllRecords();
        Task<bool> EditCategoryName(int categoryId, string newName);
        Task<bool> EditCategoryDescription(int categoryId, string description);
    }
}