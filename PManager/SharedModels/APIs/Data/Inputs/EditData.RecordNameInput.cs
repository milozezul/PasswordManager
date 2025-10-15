using SharedModels.Interfaces;

namespace SharedModels.APIs.Data.Inputs
{
    public class EditRecordNameInput: EditData, IPostApiRoute
    {
        public const string Api = "records/edit/name";
        static string IPostApiRoute.Api => Api;
    }
}
