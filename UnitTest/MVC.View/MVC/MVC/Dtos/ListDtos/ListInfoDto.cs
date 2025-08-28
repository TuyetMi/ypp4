
namespace MVC.Dtos.ListDtos
{
    public class ListInfoDto
    {
        public int Id { get; set; }
        public string ListName { get; set; } = string.Empty;
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public DateTime? LastAccessedAt { get; set; }
        public bool IsFavorited { get; set; }
    }
}
