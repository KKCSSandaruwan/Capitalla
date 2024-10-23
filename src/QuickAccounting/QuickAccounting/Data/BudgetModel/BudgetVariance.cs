using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickAccounting.Data.BudgetModel
{
    public class BudgetVariance
	{
        public int particular { get; set; }
        public decimal totalDr { get; set; }
        public decimal totalCr { get; set; }
        public decimal BudgetDr { get; set; }
        public decimal ActualDr { get; set; }
        public decimal VarianceDr { get; set; }
		public decimal VarianceCr { get; set; }
		public decimal SpentAmountDr { get; set; }
        public decimal BudgetCr { get; set; }
		public decimal ActualCr { get; set; }
	}
}
