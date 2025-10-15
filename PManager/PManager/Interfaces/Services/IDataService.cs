using SharedModels.APIs.Data.Inputs;
using SharedModels.APIs.Data.Outputs;
using SharedModels.APIs.Explorer.Outputs;
using SharedModels.Database;

namespace PManager.Interfaces.Services
{
    public interface IDataService
    {
        Task<Password?> AddPassword(PasswordAddInput model);
        Task<Category?> CreateCategory(CategoryInput model);
        Task<Record?> CreateRecord(CreateRecordInput model);
        Task DeactivatePassword(PasswordStatus model);
        Task ActivatePassword(PasswordStatus model);
        void Dispose();
        Task<RecordPasswordsOutput?> GetPasswordsByRecordId(RecordPasswordsInput model);
        public int GetUserId();
        Task<Record?> GetRecordById(int id);
        int GetRecordId();
        int GetPasswordId();
        string GetFallback();
        Task<List<CategoryRecords>> GetAllRecords();
        Task<bool> EditCategoryName(EditData model);
        Task<bool> EditCategoryDescription(EditData model);
        Task<bool> EditRecordName(EditData model);
        Task<bool> AddNoteToPassword(NoteDataCreateInput model);
        Task<DecryptedPasswordOutput?> GetPasswordByPasswordId(PasswordDataInput model);
        Task<bool> ReencryptPassword(PasswordReencryptInput model);
        Task<bool> DeletePasswordNote(NoteDataDeleteInput model);
    }
}