using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.ViewModel
{
    public class TaxRatesView
    {
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public int TaxNameId { get; set; }
        public string TaxName { get; set; }
        public decimal Rate { get; set; }
        public bool Active { get; set; }
        public bool IsActive { get; set; }
    }
}
