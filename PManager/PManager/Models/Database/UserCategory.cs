using SharedModels.Database;

namespace PManager.Models.Database
{
    public class UserCategory
    {
        public int UserId { get; set; }
        public int CategoryId { get; set; }

        public User User { get; set; }
        public Category Category { get; set; }
    }
}
