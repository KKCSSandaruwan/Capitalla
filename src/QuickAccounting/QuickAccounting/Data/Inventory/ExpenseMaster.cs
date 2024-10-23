using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickAccounting.Data.Inventory
{
    public class ExpenseMaster
    {
        [Key]
        public int ExpensiveMasterId { get; set; }
        public int LedgerId { get; set; }
        public int WarehouseId { get; set; }
        public DateTime Date { get; set; }
        public int VoucherTypeId { get; set; }
        public string SerialNo { get; set; }
        public string VoucherNo { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string Narration { get; set; }
        public int FinancialYearId { get; set; }
        public int CompanyId { get; set; }
        public string UserId { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        [NotMapped]
        public List<ExpensesDetails> listOrder { get; set; } = new List<ExpensesDetails>();
        [NotMapped]
        public List<DeleteItem> listDelete { get; set; } = new List<DeleteItem>();
    }
}
