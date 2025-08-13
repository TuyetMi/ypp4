
namespace MSListsApp.Dapper.Models
{
    public class ListViewColumn
    {
        public int Id { get; set; }
        public int ListViewId { get; set; }
        public int ListDynamicColumnId { get; set; }
        public int DisplayOrder { get; set; }
        public int? IsVisible { get; set; }
    }
}
