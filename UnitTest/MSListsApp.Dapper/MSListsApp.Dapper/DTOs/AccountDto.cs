using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSListsApp.Dapper.DTOs
{
    public class AccountCreateDto
    {
        public string Avatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateBirth { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string AccountStatus { get; set; }
        public string AccountPassword { get; set; }
    }

    public class AccountUpdateDto
    {
        public int Id { get; set; }
        public string Avatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateBirth { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string AccountStatus { get; set; }
        public string AccountPassword { get; set; }
    }

    public class AccountReadDto
    {
        public int Id { get; set; }
        public string Avatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateBirth { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string AccountStatus { get; set; }
    }
}
