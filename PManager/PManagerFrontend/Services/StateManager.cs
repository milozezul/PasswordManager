using PManagerFrontend.Interfaces.Services;

namespace PManagerFrontend.Services
{
    public class StateManager : IStateManager
    {
        string _jwtBearer { get; set; }
        public string JwtBearer
        {
            get
            {
                return _jwtBearer;
            }
            set
            {
                _jwtBearer = value;
            }
        }
    }
}
