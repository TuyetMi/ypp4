
namespace MSListsApp.Dapper.Models
{
    public class ListRow
    {
        public int Id { get; set; }
        public int ListId { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public DateTime? ModifiedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string ListRowStatus { get; set; } = "Active";
    }
}
