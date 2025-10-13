using SharedModels.Interfaces;

namespace SharedModels.InputModels
{
    public class NoteInputModel: IApiRoute
    {
        public const string Api = "records/password/note";

        static string IApiRoute.Api => Api;

        public string Text { get; set; }
        public int PasswordId { get; set; }
        public int RecordId { get; set; }
    }
}
