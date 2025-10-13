using SharedModels.Interfaces;

namespace SharedModels.InputModels
{
    public class EditCategoryNameInput: EditInput, IApiRoute
    {
        public const string Api = "categories/edit/name";
        static string IApiRoute.Api => Api;
    }
}
