using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.Authentication
{
    public class UserMaster
    {
        [Key]
       public int UserId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a role.")]
        public int RoleId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required, StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; }
        public string Image { get; set; }
        public bool Active { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
