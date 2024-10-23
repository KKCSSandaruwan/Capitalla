using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.Selectors
{
    public class ComboAccountLedger
    {
        [Key]
        public int LedgerId { get; set; }
        public string LedgerCode { get; set; }
        public string LedgerName { get; set; }

    }
}
