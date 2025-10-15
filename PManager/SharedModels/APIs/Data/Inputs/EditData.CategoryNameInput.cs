using SharedModels.Interfaces;

namespace SharedModels.APIs.Data.Inputs
{
    public class EditCategoryNameInput: EditData, IPostApiRoute
    {
        public const string Api = "categories/edit/name";
        static string IPostApiRoute.Api => Api;
    }
}
