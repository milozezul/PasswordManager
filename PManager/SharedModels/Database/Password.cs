namespace SharedModels.Database
{
    public class Password
    {
        public bool IsActive { get; set; }
        public int Id { get; set; }
        public byte[] Value { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public List<Note> Notes { get; set; } = new List<Note>();

    }
}
