namespace MSListsApp.Dapper.DTOs
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string Avatar { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? DateBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string AccountStatus { get; set; } = string.Empty;
    }
}
