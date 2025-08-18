
namespace MVC.Models
{
    public enum AccountStatus
    {
        Active = 1,
        Inactive = 2,
        Suspended = 3
    }
    public class Account
    {
        public int Id { get; set; }

        public string Avatar { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime? DateBirth { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Company { get; set; } = string.Empty;

        public AccountStatus Status { get; set; }

        public string AccountPassword { get; set; } = string.Empty;
    }
}
