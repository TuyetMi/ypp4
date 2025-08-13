
namespace MSListsApp.Dapper.Models
{
    public class FavoriteList
    {
        public int Id { get; set; }
        public int? ListId { get; set; }
        public int? AccountId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
