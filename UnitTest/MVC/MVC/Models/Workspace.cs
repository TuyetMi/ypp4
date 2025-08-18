using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Models
{
    public class Workspace
    {
        public int Id { get; set; }
        public string WorkspaceName { get; set; } = null!;
        public int CreatedBy { get; set; }
        public bool IsPersonal { get; set; } = false;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
