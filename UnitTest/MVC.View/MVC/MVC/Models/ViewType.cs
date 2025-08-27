
namespace MVC.Models
{
    public class ViewType
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? HeaderImage { get; set; }
        public string? Icon { get; set; }
        public string? ViewTypeDescription { get; set; }
    }
}
