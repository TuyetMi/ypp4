namespace MSListsApp.Dapper.DTOs
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string Avatar { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime? DateBirth { get; set; }
        public string Email { get; set; } = null!;
        public string Company { get; set; } = null!;
        public string AccountStatus { get; set; } = null!;
        public string AccountPassword { get; set; } = null!;
    }

    public class AccountSummaryDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string AccountStatus { get; set; }
    }

}