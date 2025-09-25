using SharedModels.Database;
using System.Text.Json.Serialization;

namespace PManagerFrontend.Models.Components
{
    public class GroupFolder
    {
        public Category Category { get; set; }
        public List<RecordFolder> Records { get; set; }
        public bool IsExpand { get; set; }
    }
}
