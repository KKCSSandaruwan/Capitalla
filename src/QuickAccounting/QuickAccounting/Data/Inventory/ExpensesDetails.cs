using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.Inventory
{
    public class ExpensesDetails
    {
        [Key]
        public int ExpensesDetailsId { get; set; }
        public int ExpensiveMasterId { get; set; }
        public int LedgerId { get; set; }
        public decimal Amount { get; set; }
        public string Narration { get; set; }
    }
}
