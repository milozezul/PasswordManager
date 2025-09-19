using SharedModels.Database;
using SharedModels.DataService;

namespace PManager.Interfaces.Services
{
    public interface IDataService
    {
        Task<Password?> AddPassword(int recordId, string newPassword, string password);
        Task<Category?> CreateCategory(string name);
        Task<Record?> CreateRecord(string category, string name, string url);
        Task<RecordPasswordsModel?> CreateRecordWithPassword(string category, string name, string url, string newPassword, string password);
        Task DeactivatePassword(int recordId, int passwordId, string password);
        void Dispose();
        Task<List<Category>> GetCategories();
        Task<RecordPasswordsModel?> GetPasswordsByRecordId(int recordId, string password);
        Task<List<Record>> GetRecords(string category);
        public int GetUserId();
        Task<Record?> GetRecordById(int id);
        int GetRecordId();
        int GetPasswordId();
        string GetFallback();
    }
}