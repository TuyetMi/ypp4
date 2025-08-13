namespace MSListsApp.Dapper.DTOs
{
    // Dùng khi tạo mới account
    public class CreateAccountDto
    {
        public string Avatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateBirth { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string AccountStatus { get; set; }
    }
}
