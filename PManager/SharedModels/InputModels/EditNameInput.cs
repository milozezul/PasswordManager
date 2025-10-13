using SharedModels.Interfaces;

namespace SharedModels.InputModels
{
    public class EditNameInput: EditInput, IApiRoute
    {
        public const string Api = "records/edit/name";
        static string IApiRoute.Api => Api;
    }
}
