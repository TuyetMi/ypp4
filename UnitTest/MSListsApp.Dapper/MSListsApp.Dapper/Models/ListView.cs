
namespace MSListsApp.Dapper.Models
{
    public class ListView
    {
        public int Id { get; set; }
        public int ListId { get; set; }
        public int CreatedBy { get; set; }
        public int ViewTypeId { get; set; }
        public string? ViewName { get; set; }
        public bool IsSystem { get; set; } = false;
        public DateTime? CreatedAt { get; set; }
    }
}
