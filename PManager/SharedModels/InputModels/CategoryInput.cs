using SharedModels.Interfaces;

namespace SharedModels.InputModels
{
    public class CategoryInput: IApiRoute
    {
        public const string Api = "categories";
        static string IApiRoute.Api => Api;

        public string Category { get; set; }
    }
}
