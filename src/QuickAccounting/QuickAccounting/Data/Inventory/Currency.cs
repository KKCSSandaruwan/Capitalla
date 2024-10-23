using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.Inventory
{
    public class Currency
    {
        [Key]
        public int CurrencyId { get; set; }
        [Required]
        public string CurrencySymbol { get; set; }
        [Required]
        public string CurrencyName { get; set; }
        public decimal NoOfDecimalPlaces { get; set; }
        public bool IsDefault { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
