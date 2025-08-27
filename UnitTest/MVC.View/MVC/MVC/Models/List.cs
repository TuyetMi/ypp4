
namespace MVC.Models
{
    public class List
    {
        public int Id { get; set; }

        public int ListTypeId { get; set; }          // FK to ListType
        public int? ListTemplateId { get; set; }     // FK to ListTemplate, nullable
        public int? WorkspaceID { get; set; }        // FK to Workspace, nullable

        public string ListName { get; set; } = string.Empty;
        public string? Icon { get; set; }
        public string? Color { get; set; }

        public int CreatedBy { get; set; }           // FK to Account/User
        public DateTime? CreatedAt { get; set; }

        public string ListStatus { get; set; } = "Active"; // 'Active', 'Archived', etc.
    }
}
