using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.Setting
{
    public class Tax
    {
        [Key]
        public int TaxId { get; set; }
        [Required]
        public string TaxName { get; set; }
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Please enter a valid numeric or decimal number.")]
        public Decimal Rate { get; set; }
        public bool IsActive { get; set; }
        public int CompanyId { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
