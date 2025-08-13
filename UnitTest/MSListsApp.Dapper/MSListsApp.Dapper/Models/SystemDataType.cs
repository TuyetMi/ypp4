using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSListsApp.Dapper.Models
{
    public class SystemDataType
    {
        public int Id { get; set; }
        public string? Icon { get; set; }
        public string? DataTypeDescription { get; set; }
        public string? CoverImg { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string DataTypeValue { get; set; } = string.Empty;
    }
}
