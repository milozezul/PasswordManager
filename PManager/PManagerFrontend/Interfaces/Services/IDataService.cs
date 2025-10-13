using PManagerFrontend.Models.Components;
using SharedModels.Database;
using SharedModels.DataService;
using SharedModels.InputModels;

namespace PManagerFrontend.Interfaces.Services
{
    public interface IDataService
    {
        Task<Category?> CreateNewCategory(string category);
        Task<Record?> CreateRecord(CreateRecordInput input);
        Task<RecordPasswordsModel> GetPasswordsByRecordId(RecordPasswordsInput input);
        Task<Password?> AddPassword(PasswordAddInput input);
        Task<bool> DiactivatePassword(PasswordStatusInput input);
        Task<bool> ActivatePassword(PasswordStatusInput input);
        Task<List<GroupFolderModel>> GetAllRecords();
        Task<bool> EditCategoryName(EditInput input);
        Task<bool> EditCategoryDescription(EditInput input);
        Task<bool> EditRecordName(EditInput input);
        Task<bool> AddNoteToPassword(NoteInputModel input);
        Task<DecryptedPassword?> GetPasswordsByPasswordId(PasswordLocationInput input);
        Task<bool> ReencryptPassword(PasswordReencryptInput input);
        Task<bool> DeletePasswordNote(NoteDeleteInput input);
    }
}