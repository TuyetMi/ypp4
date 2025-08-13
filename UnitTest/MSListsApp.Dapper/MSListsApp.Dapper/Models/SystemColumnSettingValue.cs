
namespace MSListsApp.Dapper.Models
{
    public class SystemColumnSettingValue
    {
        public int Id { get; set; }
        public int SystemColumnId { get; set; }
        public int DataTypeSettingKeyId { get; set; }
        public string? KeyValue { get; set; }
    }
}
