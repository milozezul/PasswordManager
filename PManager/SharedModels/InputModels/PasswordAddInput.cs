namespace SharedModels.InputModels
{
    public class PasswordAddInput
    {
        public int RecordId { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string NewPassword { get; set; }
        public string Password { get; set; }
    }
}
