
namespace MSListsApp.Dapper.Models
{
    public class ListRowComment
    {
        public int Id { get; set; }
        public int? ListRowId { get; set; }
        public string? Content { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
