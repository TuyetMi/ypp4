
namespace MVC.Models
{
    public enum PermissionCode
    {
        Owner,
        Contributor,
        Reader
    }
    public class ListPermission
    {
        public int Id { get; set; }
        public string PermissionName { get; set; } = string.Empty;
        public PermissionCode PermissionCode { get; set; }  // dùng enum
        public string? PermissionDescription { get; set; }
        public string? Icon { get; set; }
    }
}
