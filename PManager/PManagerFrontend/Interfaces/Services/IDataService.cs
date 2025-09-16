using SharedModels.Database;

namespace PManagerFrontend.Interfaces.Services
{
    public interface IDataService
    {
        Task<List<Category>> GetCategories();
        Task CreateNewCategory(string category);
        Task<List<Record>> GetRecordsByCategory(string category);
        Task CreateRecord(string name, string url, string category);
        Task CreateRecordWithPassword(string name, string url, string category, string lockpassword, string newpassword);
    }
}