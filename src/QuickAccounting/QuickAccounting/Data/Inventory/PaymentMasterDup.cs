using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickAccounting.Data.Inventory
{
    public class PaymentMasterDup
    {
        [Key]
        public int PaymentMasterId { get; set; }
        public int LedgerId { get; set; }
        public int AccountId { get; set; }
        public string PaymentType { get; set; }
        public string SettlmentType { get; set; }
        public string InvoiceType { get; set; }
        public int FinancialYearId { get; set; }
        public int CompanyId { get; set; }
        public string UserId { get; set; }
        public string Narration { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifyDate { get; set; }


        // Navigation property for linking with PaymentDetail
        public virtual ICollection<PaymentDetailsDup>? PurchaseDetails { get; set; } = new List<PaymentDetailsDup>();
    }
}
