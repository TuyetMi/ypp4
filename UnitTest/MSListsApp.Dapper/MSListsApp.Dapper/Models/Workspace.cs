using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSListsApp.Dapper.Models
{
    public class Workspace
    {
        public int Id { get; set; }
        public string WorkspaceName { get; set; }
        public int CreatedBy { get; set; }
        public bool IsPersonal { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
