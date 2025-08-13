
namespace MSListsApp.Dapper.Models
{
    public class ListViewSettingValue
    {
        public int Id { get; set; }
        public int? ListViewId { get; set; }
        public int? ViewTypeSettingKeyId { get; set; }
        public string? KeyValue { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
