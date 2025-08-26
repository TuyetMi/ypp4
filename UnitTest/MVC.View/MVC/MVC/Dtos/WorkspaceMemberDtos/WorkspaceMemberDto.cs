using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVC.Models;

namespace MVC.Dtos.WorkspaceMemberDtos
{
    public class WorkspaceMemberDto
    {
        public int Id { get; set; }
        public int WorkspaceId { get; set; }
        public int AccountId { get; set; }
        public MemberStatus MemberStatus { get; set; } // enum trùng với model
        public DateTime? JoinedAt { get; set; }
    }
}
