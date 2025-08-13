
namespace MSListsApp.Dapper.Models
{
    public class RecentList
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int ListId { get; set; }
        public DateTime LastAccessedAt { get; set; }
    }
}
