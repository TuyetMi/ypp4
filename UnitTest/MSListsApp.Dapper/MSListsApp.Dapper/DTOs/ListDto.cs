namespace MSListsApp.Dapper.DTOs
{
    public class ListDto
    {
        public int Id { get; set; } // 0 khi tạo mới
        public int ListTypeId { get; set; }
        public int? ListTemplateId { get; set; }
        public int? WorkspaceId { get; set; }
        public string ListName { get; set; } = string.Empty;
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string ListStatus { get; set; } = "Active";
    }
    public class ListDetailDto
    {
        public int Id { get; set; }
        public string ListName { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public string? WorkspaceName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string ListStatus { get; set; } = "Active";
    }
    public class ListSummaryDto
    {
        public string ListName { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public string? WorkspaceName { get; set; }
        public bool IsFavorited { get; set; }
    }
}
