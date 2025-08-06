using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsListsApp.Models
{
    public class ListPermission
    {
        public int Id { get; set; }

        public string PermissionName { get; set; } = null!;

        public string PermissionCode { get; set; } = null!;  // e.g. "Owner", "Contributor", "Reader"

        public string? PermissionDescription { get; set; }

        public string? Icon { get; set; }

        public ICollection<ListMemberPermission> ListMemberPermissions { get; set; } = new List<ListMemberPermission>();
    }
}
