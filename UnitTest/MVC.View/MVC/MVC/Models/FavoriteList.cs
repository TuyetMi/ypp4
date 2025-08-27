
namespace MVC.Models
{
    public class FavoriteList
    {
        public int Id { get; set; }

        public int ListId { get; set; }       // FK to List
        public int AccountId { get; set; }    // FK to Account

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
