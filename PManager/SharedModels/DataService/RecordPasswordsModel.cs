using SharedModels.Database;

namespace SharedModels.DataService
{
    public class RecordPasswordsModel
    {
        public Record Record { get; set; }
        public List<DecryptedPassword> Passwords { get; set; }
    }
}
