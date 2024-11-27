using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.Setting.Navigation
{
    public class NavigationMenu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NavigationMenuId { get; set; }

        [Required(ErrorMessage = "Menu group ID is required.")]
        [ForeignKey("MenuGroup")]
        public int MenuGroupId { get; set; }

        [Required(ErrorMessage = "Main menu ID is required.")]
        [ForeignKey("MainMenu")]
        public int MainMenuId { get; set; }

        [Required(ErrorMessage = "Sub menu ID is required.")]
        [ForeignKey("SubMenu")]
        public int SubMenuId { get; set; }

        [Required(ErrorMessage = "Created by is required.")]
        [StringLength(50, ErrorMessage = "Created by cannot exceed 50 characters.")]
        public string CreatedBy { get; set; }

        [Required(ErrorMessage = "Creation date is required.")]
        public DateTime CreatedDate { get; set; }

        [StringLength(50, ErrorMessage = "Modified by cannot exceed 50 characters.")]
        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [Required(ErrorMessage = "Active status is required.")]
        public bool Active { get; set; } = true;
    }
}
