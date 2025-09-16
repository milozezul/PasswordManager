using SharedModels.Database;

namespace PManagerFrontend.Interfaces.Services
{
    public interface IDataService
    {
        Task<List<Category>> GetCategories();
    }
}