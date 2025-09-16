namespace SharedModels.Database
{
    public class Record
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Category Category { get; set; }
    }
}
