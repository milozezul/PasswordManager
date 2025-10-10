using PManagerFrontend.Models.Components;
using SharedModels.Database;
using SharedModels.DataService;
using SharedModels.InputModels;

namespace PManagerFrontend.Interfaces.Services
{
    public interface IDataService
    {
        Task<Category?> CreateNewCategory(string category);
        Task<Record?> CreateRecord(string name, string url, string username, int category);
        Task<RecordPasswordsModel> GetPasswordsByRecordId(int id, string lockpassword);
        Task<Password?> AddPassword(PasswordAddInputModel input);
        Task<bool> DiactivatePassword(int recordId, int passwordId, string password);
        Task<bool> ActivatePassword(int recordId, int passwordId, string password);
        Task<List<GroupFolderModel>> GetAllRecords();
        Task<bool> EditCategoryName(int categoryId, string name);
        Task<bool> EditCategoryDescription(int categoryId, string description);
        Task<bool> EditRecordName(int recordId, string name);
        Task<bool> AddNoteToPassword(NoteInputModel input);
        Task<DecryptedPassword?> GetPasswordsByPasswordId(PasswordGetOutputModel input);
        Task<bool> ReencryptPassword(PasswordReencryptInputModel input);
        Task<bool> DeletePasswordNote(NoteDeleteInput input);
    }
}