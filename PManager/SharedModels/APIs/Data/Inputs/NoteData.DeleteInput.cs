using SharedModels.Interfaces;

namespace SharedModels.APIs.Data.Inputs
{
    public class NoteDataDeleteInput: IPostApiRoute
    {
        public const string Api = "password/delete";
        static string IPostApiRoute.Api => Api;

        public int RecordId { get; set; }
        public int PasswordId { get; set; }
        public int NoteId { get; set; }
    }
}
