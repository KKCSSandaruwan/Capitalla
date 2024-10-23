using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickAccounting.Data.BudgetModel
{
    public class BudgetDetailsView
	{
        public int BudgetDetailsId { get; set; }
        public int BudgetMasterId { get; set; }
        public int LedgerId { get; set; }
		public string LedgerName { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        [NotMapped]
        public int Id { get; set; }

	}
}
