using SharedModels.Database;
using SharedModels.DataService;

namespace PManagerFrontend.Models.Components
{
    public class RecordFolderModel
    {
        public Record Record { get; set; }
        public List<DecryptedPassword> Passwords { get; set; }
        public bool IsExpand { get; set; }
    }
}
