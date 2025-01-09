using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickAccounting.Data.Setting.Currency
{
    public class CurrencyExchangeRate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Exchange Rate ID")]
        public int ExchangeRateId { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The selected company ID is invalid.")]
        [ForeignKey("Company")]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Base currency is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The selected base currency ID is invalid.")]
        [ForeignKey("Currency")]
        [Display(Name = "Base Currency")]
        public int BaseCurrencyId { get; set; }

        [Required(ErrorMessage = "Foreign currency is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The selected foreign currency ID is invalid.")]
        [ForeignKey("Currency")]
        [Display(Name = "Foreign Currency")]
        public int ForeignCurrencyId { get; set; }

        [Required(ErrorMessage = "Exchange rate is required.")]
        [Range(0.000001, double.MaxValue, ErrorMessage = "Exchange rate must be a positive value.")]
        [Display(Name = "Exchange Rate")]
        public decimal ExchangeRate { get; set; }

        [Required(ErrorMessage = "Effective date is required.")]
        [Display(Name = "Effective Date")]
        public DateTime EffectiveDate { get; set; }

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
