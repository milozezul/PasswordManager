namespace SharedModels.Database
{
    public class RecordPasswords
    {
        public int PasswordId { get; set; }
        public int RecordId { get; set; }

        public Password Password { get; set; }
        public Record Record { get; set; }

    }
}
