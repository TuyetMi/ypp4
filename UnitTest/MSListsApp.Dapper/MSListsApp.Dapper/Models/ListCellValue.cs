
namespace MSListsApp.Dapper.Models
{
    public class ListCellValue
    {
        public int Id { get; set; }
        public int ListRowId { get; set; }
        public int ListColumnId { get; set; }
        public string? CellValue { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
