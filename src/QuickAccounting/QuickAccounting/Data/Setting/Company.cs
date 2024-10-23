using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.Setting
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        [Required]
        public string Email { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a currency.")]
        public int CurrencyId { get; set; }
        public int FinancialYearId { get; set; }
        public int NoofDecimal { get; set; }
        public string Website { get; set; }
        public int WarehouseId { get; set; }
        public string Logo { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
