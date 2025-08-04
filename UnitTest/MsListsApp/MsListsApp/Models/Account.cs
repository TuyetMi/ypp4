using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsListsApp.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string? Avatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateBirth { get; set; }
        public string Email { get; set; }
        public string? Company { get; set; }
        public string AccountStatus { get; set; } = "Active";
        public string AccountPassword { get; set; }
    }
}
