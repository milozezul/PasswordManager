using SharedModels.Interfaces;

namespace SharedModels.APIs.Data.Inputs
{
    public class NoteDataCreateInput: IPostApiRoute
    {
        public const string Api = "records/password/note";

        static string IPostApiRoute.Api => Api;

        public string Text { get; set; }
        public int PasswordId { get; set; }
        public int RecordId { get; set; }
    }
}
