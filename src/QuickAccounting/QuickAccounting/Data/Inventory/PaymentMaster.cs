using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickAccounting.Data.Inventory
{
    public class PaymentMaster
    {
        [Key]
        public int PaymentMasterId { get; set; }
        public int PurchaseMasterId { get; set; }
        public string VoucherNo { get; set; }
        public DateTime Date { get; set; }
        public int LedgerId { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal Amount { get; set; }
        public string SerialNo { get; set; }
        public string Narration { get; set; }
        public int VoucherTypeId { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public string PaymentType { get; set; }
        public int FinancialYearId { get; set; }
        public int CompanyId { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        [NotMapped]
        public List<PaymentDetails> listOrder { get; set; } = new List<PaymentDetails>();
        [NotMapped]
        public List<DeleteItem> listDelete { get; set; } = new List<DeleteItem>();
        [NotMapped]
        public decimal PreviousDue { get; set; }
        [NotMapped]
        public int CashBankid { get; set; }
        
    }
}
