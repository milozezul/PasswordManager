using SharedModels.Interfaces;

namespace SharedModels.InputModels
{
    public class NoteDeleteInput: IApiRoute
    {
        public const string Api = "password/delete";
        static string IApiRoute.Api => Api;

        public int RecordId { get; set; }
        public int PasswordId { get; set; }
        public int NoteId { get; set; }
    }
}
