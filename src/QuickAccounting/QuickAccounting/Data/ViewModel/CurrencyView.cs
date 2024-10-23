using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.ViewModel
{
    public class CurrencyView
    {
        public int CurrencyId { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; }
        public decimal NoOfDecimalPlaces { get; set; }
        public bool IsDefault { get; set; }
        public int CompanyId { get; set; }
    }
}
