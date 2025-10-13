using SharedModels.Interfaces;

namespace SharedModels.InputModels
{
    public class PasswordStatusInput
    {
        public int RecordId { get; set; }
        public int PasswordId { get; set; }
        public string Password { get; set; }
    }
}
