namespace MSListsApp.Dapper.Models
{
    public class ListTemplate
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? HeaderImage { get; set; }
        public string? TemplateDescription { get; set; }
        public string? Icon { get; set; }
        public string Color { get; set; } = "#28A745";
        public string? Sumary { get; set; }
        public string? Feature { get; set; }
        public int ListTypeId { get; set; }
        public int ProviderId { get; set; }
    }
}
