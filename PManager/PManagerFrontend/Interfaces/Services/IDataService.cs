using PManagerFrontend.Models.Components;
using SharedModels.Database;
using SharedModels.DataService;
using SharedModels.InputModels;

namespace PManagerFrontend.Interfaces.Services
{
    public interface IDataService
    {
        Task<Category?> CreateNewCategory(CategoryInput input);
        Task<Record?> CreateRecord(CreateRecordInput input);
        Task<RecordPasswordsModel> GetPasswordsByRecordId(RecordPasswordsInput input);
        Task<Password?> AddPassword(PasswordAddInput input);
        Task<bool> DiactivatePassword(PasswordDiactivateInput input);
        Task<bool> ActivatePassword(PasswordActivateInput input);
        Task<List<GroupFolderModel>> GetAllRecords();
        Task<bool> EditCategoryName(EditCategoryNameInput input);
        Task<bool> EditCategoryDescription(EditCategoryDescriptionInput input);
        Task<bool> EditRecordName(EditNameInput input);
        Task<bool> AddNoteToPassword(NoteInputModel input);
        Task<DecryptedPassword?> GetPasswordsByPasswordId(PasswordLocationInput input);
        Task<bool> ReencryptPassword(PasswordReencryptInput input);
        Task<bool> DeletePasswordNote(NoteDeleteInput input);
    }
}