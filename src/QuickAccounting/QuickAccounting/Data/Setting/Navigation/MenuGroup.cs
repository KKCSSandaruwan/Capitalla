using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickAccounting.Data.Setting.Navigation
{
    public class MenuGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MenuGroupId { get; set; }

        [Required(ErrorMessage = "Menu group name is required.")]
        [StringLength(100, ErrorMessage = "Menu group name cannot exceed 100 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Menu group name can only contain letters.")]
        public string MenuGroupName { get; set; }

        [StringLength(50, ErrorMessage = "Icon name cannot exceed 50 characters.")]
        public string? IconName { get; set; }

        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        public string? Description { get; set; }

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
    }
}
