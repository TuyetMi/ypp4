
namespace MVC.Models
{
    public class RecentList
    {
        public int Id { get; set; }

        public int AccountId { get; set; }           // FK to Account
        public int ListId { get; set; }              // FK to List

        public DateTime LastAccessedAt { get; set; }
    }
}
