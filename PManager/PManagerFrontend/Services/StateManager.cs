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

        string _firstName { get; set; }
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
            }
        }
        string _lastName { get; set; }
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
            }
        }
        string _email { get; set; }
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }
    }
}
