
namespace MSListsApp.Dapper.Models
{
    public class DynamicColumnSettingValue
    {
        public int Id { get; set; }
        public int? DynamicColumnId { get; set; }
        public int? DataTypeSettingKeyId { get; set; }
        public string? KeyValue { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
