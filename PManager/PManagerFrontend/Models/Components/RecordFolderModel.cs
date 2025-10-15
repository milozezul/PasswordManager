using SharedModels.APIs.Data.Outputs;
using SharedModels.Database;

namespace PManagerFrontend.Models.Components
{
    public class RecordFolderModel
    {
        public Record Record { get; set; }
        public List<DecryptedPasswordOutput> Passwords { get; set; }
        public bool IsExpand { get; set; }
        public string? Key { get; set; }
    }
}
