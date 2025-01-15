using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickAccounting.Data.Setting.Currency
{
    public class Currency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Currency ID")]
        public int CurrencyId { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The selected company ID is invalid.")]
        [ForeignKey("Company")]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "ISO code is required.")]
        [StringLength(3, ErrorMessage = "ISO code cannot exceed 3 characters.")]
        [Display(Name = "ISO Code")]
        public string ISOCode { get; set; }

        [Required(ErrorMessage = "Symbol is required.")]
        [StringLength(10, ErrorMessage = "Symbol cannot exceed 10 characters.")]
        [RegularExpression(@"^[^\s]{1,10}$", ErrorMessage = "Symbol cannot contain spaces and must be up to 10 characters.")]
        [Display(Name = "Symbol")]
        public string Symbol { get; set; }

        [Required(ErrorMessage = "Currency name is required.")]
        [StringLength(50, ErrorMessage = "Currency name cannot exceed 50 characters.")]
        [Display(Name = "Currency Name")]
        public string CurrencyName { get; set; }

        [Required(ErrorMessage = "Display format is required.")]
        [StringLength(50, ErrorMessage = "Display format cannot exceed 50 characters.")]
        [Display(Name = "Display Format")]
        public string DisplayFormat { get; set; }

        [Required(ErrorMessage = "Created by is required.")]
        [StringLength(50, ErrorMessage = "Created by cannot exceed 50 characters.")]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Required(ErrorMessage = "Created date is required.")]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [StringLength(50, ErrorMessage = "Modified by cannot exceed 50 characters.")]
        [Display(Name = "Modified By")]
        public string? ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Required(ErrorMessage = "Active status is required.")]
        [Display(Name = "Active")]
        public bool Active { get; set; } = true;
    }
}
