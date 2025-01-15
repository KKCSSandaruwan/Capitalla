using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using QuickAccounting.Data.Setting.Navigation;

namespace QuickAccounting.Data.Setting.SystemUser
{
    public class UserPrivilege
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserPrivilegeId { get; set; }

        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "User role is required.")]
        [ForeignKey("UserRole")]
        public int UserRoleId { get; set; }

        [Required(ErrorMessage = "Navigation menu ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Navigation menu ID must be a positive integer.")]
        [ForeignKey("NavigationMenu")]
        public int NavigationMenuId { get; set; }

        public bool CanView { get; set; } = false;

        public bool CanAdd { get; set; } = false;

        public bool CanEdit { get; set; } = false;

        public bool CanDelete { get; set; } = false;

        [Required(ErrorMessage = "Created by is required.")]
        [StringLength(50, ErrorMessage = "Created by name cannot exceed 50 characters.")]
        public string CreatedBy { get; set; }

        [Required(ErrorMessage = "Created date is required.")]
        public DateTime CreatedDate { get; set; }

        [StringLength(50, ErrorMessage = "Modified by name cannot exceed 50 characters.")]
        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [Required(ErrorMessage = "Active status is required.")]
        public bool Active { get; set; }


        // Navigation properties
        public virtual UserRole? UserRole { get; set; }

        public virtual NavigationMenu? NavigationMenu { get; set; }
    }
}
