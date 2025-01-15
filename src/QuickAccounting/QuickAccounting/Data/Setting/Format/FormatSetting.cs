using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickAccounting.Data.Setting.Format
{
    public class FormatSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Format Settings ID")]
        public int FormatSettingsId { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The selected company ID is invalid.")]
        [ForeignKey("Company")]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Short Date Format is required.")]
        [StringLength(25, ErrorMessage = "Short Date Format cannot exceed 25 characters.")]
        [Display(Name = "Short Date Format")]
        public string ShortDateFormat { get; set; }

        [Required(ErrorMessage = "Long Date Format is required.")]
        [StringLength(50, ErrorMessage = "Long Date Format cannot exceed 50 characters.")]
        [Display(Name = "Long Date Format")]
        public string LongDateFormat { get; set; }

        [Required(ErrorMessage = "Short DateTime Format is required.")]
        [StringLength(25, ErrorMessage = "Short DateTime Format cannot exceed 25 characters.")]
        [Display(Name = "Short DateTime Format")]
        public string ShortDateTimeFormat { get; set; }

        [Required(ErrorMessage = "Long DateTime Format is required.")]
        [StringLength(50, ErrorMessage = "Long DateTime Format cannot exceed 50 characters.")]
        [Display(Name = "Long DateTime Format")]
        public string LongDateTimeFormat { get; set; }

        [Required(ErrorMessage = "Currency Format is required.")]
        [StringLength(20, ErrorMessage = "Currency Format cannot exceed 20 characters.")]
        [Display(Name = "Currency Format")]
        public string CurrencyFormat { get; set; }

        [Required(ErrorMessage = "Number Format is required.")]
        [StringLength(20, ErrorMessage = "Number Format cannot exceed 20 characters.")]
        [Display(Name = "Number Format")]
        public string NumberFormat { get; set; }

        [Required(ErrorMessage = "Percentage Format is required.")]
        [StringLength(20, ErrorMessage = "Percentage Format cannot exceed 20 characters.")]
        [Display(Name = "Percentage Format")]
        public string PercentageFormat { get; set; }

        [Required(ErrorMessage = "Created By is required.")]
        [StringLength(50, ErrorMessage = "Created By cannot exceed 50 characters.")]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Required(ErrorMessage = "Created Date is required.")]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [StringLength(50, ErrorMessage = "Modified By cannot exceed 50 characters.")]
        [Display(Name = "Modified By")]
        public string? ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Required(ErrorMessage = "Active status is required.")]
        [Display(Name = "Active")]
        public bool Active { get; set; } = true;
    }
}
