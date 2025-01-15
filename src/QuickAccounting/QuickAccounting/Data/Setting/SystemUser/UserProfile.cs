using QuickAccounting.Data.Setting.Corporate;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickAccounting.Data.Setting.SystemUser
{
    public class UserProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "User ID")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "User Role is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The selected user role ID is invalid.")]
        [ForeignKey("UserRole")]
        [Display(Name = "User Role")]
        public int UserRoleId { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The selected company ID is invalid.")]
        [ForeignKey("Company")]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "User name is required.")]
        [StringLength(50, ErrorMessage = "User name cannot exceed 50 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "User name can only contain letters, numbers, and underscores (_).")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(255, ErrorMessage = "Password cannot exceed 255 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must be at least 8 characters long, and contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        [Display(Name = "Password")]
        public string PlainPassword { get; set; }

        [Display(Name = "Encrypted Password")]
        public string EncryptedPassword { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [StringLength(150, ErrorMessage = "Email cannot exceed 150 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [Display(Name = "Email Address")]
        public string? Email { get; set; }

        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        [RegularExpression(@"^(?:\+94|0)?(?:7[0-9]{8}|[1-9][0-9]{6})$", ErrorMessage = "Invalid Sri Lanka phone number format.")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [StringLength(255, ErrorMessage = "Avatar path cannot exceed 255 characters.")]
        [Display(Name = "Avatar")]
        public string? AvatarPath { get; set; }

        [Required(ErrorMessage = "Please confirm the verified status.")]
        [Display(Name = "Verified")]
        public bool Verified { get; set; }

        [Required(ErrorMessage = "Please indicate if the user is blocked.")]
        [Display(Name = "Blocked")]
        public bool Blocked { get; set; }

        [Display(Name = "Last Login Date")]
        public DateTime? LastLoginDate { get; set; }

        [StringLength(50, ErrorMessage = "Created by name cannot exceed 50 characters.")]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; } = "System";

        [Required(ErrorMessage = "Created date is required.")]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [StringLength(50, ErrorMessage = "Modified by name cannot exceed 50 characters.")]
        [Display(Name = "Modified By")]
        public string? ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Required(ErrorMessage = "Active status is required.")]
        [Display(Name = "Active")]
        public bool Active { get; set; } = true;


        // Navigation properties
        public virtual UserRole? UserRole { get; set; }

        public virtual Company? Company { get; set; }
    }
}
