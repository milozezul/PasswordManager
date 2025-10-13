namespace SharedModels.InputModels
{
    public class PasswordReencryptInput
    {
        public string OldKey { get; set; }
        public string NewKey { get; set; }
        public int RecordId { get; set; }
        public int PasswordId { get; set; }
    }
}
