using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Models
{
    public enum MemberStatus
    {
        Owner = 1,
        Contributor = 2,
        Vỉewer = 3
    }
    public class WorkspaceMember
    {
        public int Id { get; set; }
        public int WorkspaceId { get; set; }
        public int AccountId { get; set; }
        public DateTime? JoinedAt { get; set; }
        public MemberStatus MemberStatus { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
