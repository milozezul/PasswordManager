using SharedModels.Interfaces;

namespace SharedModels.APIs.Data.Inputs
{
    public class PasswordStatus
    {
        public int RecordId { get; set; }
        public int PasswordId { get; set; }
        public string Password { get; set; }
    }
}
