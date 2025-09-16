using SharedModels.Database;

namespace PManager.Models.DataService
{
    public class RecordPasswordsModel
    {
        public Record Record { get; set; }
        public List<DecryptedPassword> Passwords { get; set; }
    }
}
