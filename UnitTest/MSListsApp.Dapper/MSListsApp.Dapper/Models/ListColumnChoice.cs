
namespace MSListsApp.Dapper.Models
{
    public class ListColumnChoice
    {
        public int Id { get; set; }
        public int? ListDynamicColumnId { get; set; }
        public string? ChoiceValue { get; set; }
        public string? ChoiceColor { get; set; }
        public int DisplayOrder { get; set; } = 0;
    }
}
