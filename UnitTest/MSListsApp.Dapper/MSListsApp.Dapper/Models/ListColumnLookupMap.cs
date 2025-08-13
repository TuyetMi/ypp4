
namespace MSListsApp.Dapper.Models
{
    public class ListColumnLookupMap
    {
        public int Id { get; set; }
        public int? TargetColumnId { get; set; }
        public int? SourceListId { get; set; }
        public int? SourceColumnId { get; set; }
        public bool IsAddtionColumn { get; set; } = false;
        public int? LookupColumnId { get; set; }
    }
}
