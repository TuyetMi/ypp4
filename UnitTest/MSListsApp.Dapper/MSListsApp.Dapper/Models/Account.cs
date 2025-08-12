
namespace MSListsApp.Dapper.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Avatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateBirth { get; set; } // Cho phép null nếu cột này nullable
        public string Email { get; set; }
        public string Company { get; set; }
        public string AccountStatus { get; set; }
        public string AccountPassword { get; set; }
    }
}
