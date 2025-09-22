namespace SharedModels.Database
{
    public class Password
    {
        public bool IsActive { get; set; }
        public int Id { get; set; }
        public byte[] Value { get; set; }

    }
}
