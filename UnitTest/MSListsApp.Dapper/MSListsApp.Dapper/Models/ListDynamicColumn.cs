
namespace MSListsApp.Dapper.Models
{
    public class ListDynamicColumn
    {
        public int Id { get; set; }
        public int ListId { get; set; }
        public int SystemDataTypeId { get; set; }
        public int? SystemColumnId { get; set; }
        public string ColumnName { get; set; } = string.Empty;
        public string? ColumnDescription { get; set; }
        public bool IsSystemColumn { get; set; } = false;
        public bool IsVisible { get; set; } = true;
        public int CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
