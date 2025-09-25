using SharedModels.Database;

namespace PManagerFrontend.Models.Components
{
    public class GroupFolderModel
    {
        public Category Category { get; set; }
        public List<RecordFolderModel> Records { get; set; }
        public bool IsExpand { get; set; }
    }
}
