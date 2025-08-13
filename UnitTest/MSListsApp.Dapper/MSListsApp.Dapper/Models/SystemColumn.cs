
namespace MSListsApp.Dapper.Models
{
    public class SystemColumn
    {
        public int Id { get; set; }
        public int SystemDataTypeId { get; set; }
        public string ColumnName { get; set; } = string.Empty;
        public bool? IsVisible { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool CanRename { get; set; } = false;
    }
}
