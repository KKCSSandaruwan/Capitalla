using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.ViewModel.SystemUser
{
    public class UserProfileView
    {
        [Display(Name = "User ID")]
        public int UserId { get; set; }

        [Display(Name = "User Role ID")]
        public int UserRoleId { get; set; }

        [Display(Name = "User Role Name")]
        public string? UserRoleName { get; set; }

        [Display(Name = "Company ID")]
        public int CompanyId { get; set; }

        [Display(Name = "Company Name")]
        public string? CompanyName { get; set; }

        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Email Address")]
        public string? Email { get; set; }

        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Avatar Path")]
        public string? AvatarPath { get; set; }

        [Display(Name = "Verified")]
        public bool Verified { get; set; }

        [Display(Name = "Blocked")]
        public bool Blocked { get; set; }

        [Display(Name = "Last Login Date")]
        public DateTime? LastLoginDate { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modified By")]
        public string? ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; } = true;


        // Computed properties
        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
