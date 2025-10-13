using SharedModels.Database;
using SharedModels.DataService;
using SharedModels.InputModels;

namespace PManager.Interfaces.Services
{
    public interface IDataService
    {
        Task<Password?> AddPassword(PasswordAddInput model);
        Task<Category?> CreateCategory(CategoryInput model);
        Task<Record?> CreateRecord(CreateRecordInput model);
        Task DeactivatePassword(PasswordStatusInput model);
        Task ActivatePassword(PasswordStatusInput model);
        void Dispose();
        Task<RecordPasswordsModel?> GetPasswordsByRecordId(RecordPasswordsInput model);
        public int GetUserId();
        Task<Record?> GetRecordById(int id);
        int GetRecordId();
        int GetPasswordId();
        string GetFallback();
        Task<List<CategoryRecords>> GetAllRecords();
        Task<bool> EditCategoryName(EditInput model);
        Task<bool> EditCategoryDescription(EditInput model);
        Task<bool> EditRecordName(EditInput model);
        Task<bool> AddNoteToPassword(NoteInputModel model);
        Task<DecryptedPassword?> GetPasswordByPasswordId(PasswordLocationInput model);
        Task<bool> ReencryptPassword(PasswordReencryptInput model);
        Task<bool> DeletePasswordNote(NoteDeleteInput model);
    }
}