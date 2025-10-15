using PManagerFrontend.Models.Components;
using SharedModels.APIs.Data.Inputs;
using SharedModels.APIs.Data.Outputs;
using SharedModels.Database;

namespace PManagerFrontend.Interfaces.Services
{
    public interface IDataService
    {
        Task<Category?> CreateNewCategory(CategoryInput input);
        Task<Record?> CreateRecord(CreateRecordInput input);
        Task<RecordPasswordsOutput> GetPasswordsByRecordId(RecordPasswordsInput input);
        Task<Password?> AddPassword(PasswordAddInput input);
        Task<bool> DiactivatePassword(PasswordDiactivateInput input);
        Task<bool> ActivatePassword(PasswordActivateInput input);
        Task<List<GroupFolderModel>> GetAllRecords();
        Task<bool> EditCategoryName(EditCategoryNameInput input);
        Task<bool> EditCategoryDescription(EditCategoryDescriptionInput input);
        Task<bool> EditRecordName(EditRecordNameInput input);
        Task<bool> AddNoteToPassword(NoteDataCreateInput input);
        Task<DecryptedPasswordOutput?> GetPasswordsByPasswordId(PasswordDataInput input);
        Task<bool> ReencryptPassword(PasswordReencryptInput input);
        Task<bool> DeletePasswordNote(NoteDataDeleteInput input);
    }
}