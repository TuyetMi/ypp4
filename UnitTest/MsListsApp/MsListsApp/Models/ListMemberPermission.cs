using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsListsApp.Models
{
    public class ListMemberPermission
    {
        public int Id { get; set; }

        public int ListId { get; set; }

        public int AccountId { get; set; }

        public int HighestPermissionId { get; set; }

        public string HighestPermissionCode { get; set; } = null!;

        public int GrantedByAccountId { get; set; }

        public string? Note { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        //// Navigation properties
        //public List List { get; set; } = null!;

        //public Account Account { get; set; } = null!;

        //public Account GrantedByAccount { get; set; } = null!;

        //public ListPermission HighestPermission { get; set; } = null!;
    }
}
