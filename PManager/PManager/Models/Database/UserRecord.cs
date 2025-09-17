using SharedModels.Database;

namespace PManager.Models.Database
{
    public class UserRecord
    {
        public int UserId { get; set; }
        public int RecordId { get; set; }

        public User User { get; set; }
        public Record Record { get; set; }
    }
}
