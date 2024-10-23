using QuickAccounting.Data.AccountModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickAccounting.Data.BudgetModel
{
    public class BudgetMaster
    {
        [Key]
        public int BudgetMasterId { get; set; }
        [Required]
        public string BudgetName { get; set; }
        public string Type { get; set; }
        public decimal TotalDr { get; set; }
        public decimal TotalCr { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Narration { get; set; }
		[NotMapped]
		public List<BudgetDetails> listOrder { get; set; } = new List<BudgetDetails>();
		[NotMapped]
		public List<DeleteItem> listDelete { get; set; } = new List<DeleteItem>();
	}
}
