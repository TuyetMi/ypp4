using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVC.Models;

namespace MVC.Dtos.AccountDtos
{
    public class AccountLoginDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string AccountPassword { get; set; } = string.Empty;
        public AccountStatus Status { get; set; }
    }
}
