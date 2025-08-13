

namespace MSListsApp.Dapper.Models
{
    public class KeySetting
    {
        public int Id { get; set; }
        public string? Icon { get; set; }
        public string KeyName { get; set; } = string.Empty;
        public string ValueType { get; set; } = string.Empty;
        public bool IsDefaultValue { get; set; } = false;
        public string? ValueOfDefault { get; set; }
        public bool IsShareLinkSetting { get; set; } = false;
    }
}
