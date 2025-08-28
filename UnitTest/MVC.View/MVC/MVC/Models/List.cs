namespace MVC.Models
{
    public enum ListStatus
    {
        Active = 1,
        Archived = 2,
        Deleted = 3
    }
    public class List
    {
        public int Id { get; set; }
        public int ListTypeId { get; set; }       
        public int? ListTemplateId { get; set; }    
        public int? WorkspaceID { get; set; }    
        public string ListName { get; set; } = string.Empty;
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public int CreatedBy { get; set; }  
        public DateTime? CreatedAt { get; set; }
        public ListStatus ListStatus { get; set; }
    }
}
