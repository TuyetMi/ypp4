
namespace MSListsApp.Dapper.Models
{
    public class List
    {
        public int Id { get; set; }
        public int ListTypeId { get; set; }
        public int? ListTemplateId { get; set; }
        public int? WorkspaceId { get; set; }
        public string ListName { get; set; } 
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string ListStatus { get; set; } = "Active";
    }
}
