namespace PManagerFrontend.Interfaces.Services
{
    public interface IStateManager
    {
        string JwtBearer { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
    }
}