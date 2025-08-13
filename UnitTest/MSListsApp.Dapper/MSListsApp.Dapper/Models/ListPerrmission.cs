using System;
using System.Collections.Generic;


namespace MSListsApp.Dapper.Models
{
    public class ListPermission
    {
        public int Id { get; set; }
        public string PermissionName { get; set; } = string.Empty;
        public string PermissionCode { get; set; } = string.Empty;
        public string? PermissionDescription { get; set; }
        public string? Icon { get; set; }
    }
}
