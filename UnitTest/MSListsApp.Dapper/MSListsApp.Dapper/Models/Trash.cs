
namespace MSListsApp.Dapper.Models
{
    public class Trash
    {
        public int Id { get; set; }
        public string? EntityType { get; set; }
        public int? EntityId { get; set; }
        public int? UserDeleteId { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
