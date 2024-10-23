namespace QuickAccounting.Data.Authentication
{
    public class UserAccount
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
    }
}
