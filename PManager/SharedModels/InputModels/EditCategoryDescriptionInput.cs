using SharedModels.Interfaces;

namespace SharedModels.InputModels
{
    public class EditCategoryDescriptionInput: EditInput, IApiRoute
    {
        public const string Api = "categories/edit/description";
        static string IApiRoute.Api => Api;
    }
}
