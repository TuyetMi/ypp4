namespace MSListsApp.Dapper.DTOs
{
    public class AccountDto
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

    // Dùng khi cập nhật account
    public class UpdateAccountDto
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
