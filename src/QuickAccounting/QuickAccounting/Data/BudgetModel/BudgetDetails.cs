using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.BudgetModel
{
    public class BudgetDetails
    {
        [Required]
        public int BudgetDetailsId { get; set; }
        public int BudgetMasterId { get; set; }
        public int LedgerId { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
    }
}
