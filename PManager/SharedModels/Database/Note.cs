namespace SharedModels.Database
{
    public class Note
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Text { get; set; }

        public int PasswordId { get; set; }
        public Password Password { get; set; }
    }
}
