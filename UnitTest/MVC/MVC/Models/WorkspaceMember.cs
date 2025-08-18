using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Models
{
    public class WorkspaceMember
    {
        public int Id { get; set; }
        public int WorkspaceId { get; set; }
        public int AccountId { get; set; }
        public DateTime? JoinedAt { get; set; }
        public string MemberStatus { get; set; } = "Active";
        public DateTime? UpdatedAt { get; set; }
    }
}
