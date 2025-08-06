using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsListsApp.Models
{
    public class List
    {
        public int Id { get; set; }
        public int ListTypeId { get; set; }
        public int? ListTemplateId { get; set; }
        public int? WorkspaceID { get; set; }
        public string ListName { get; set; } = null!;
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string ListStatus { get; set; } = "Active";


    }
}
