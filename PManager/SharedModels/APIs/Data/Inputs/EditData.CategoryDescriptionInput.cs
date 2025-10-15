using SharedModels.Interfaces;

namespace SharedModels.APIs.Data.Inputs
{
    public class EditCategoryDescriptionInput: EditData, IPostApiRoute
    {
        public const string Api = "categories/edit/description";
        static string IPostApiRoute.Api => Api;
    }
}
