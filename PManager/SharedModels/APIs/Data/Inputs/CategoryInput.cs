using SharedModels.Interfaces;

namespace SharedModels.APIs.Data.Inputs
{
    public class CategoryInput: IPostApiRoute
    {
        public const string Api = "categories";
        static string IPostApiRoute.Api => Api;

        public string Category { get; set; }
    }
}
