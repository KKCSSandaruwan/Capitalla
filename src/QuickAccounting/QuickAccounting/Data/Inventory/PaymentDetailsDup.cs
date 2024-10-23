using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickAccounting.Data.Inventory
{
    public class PaymentDetailsDup
    {
        [Key]
        public int PaymentDetailId { get; set; }
        [ForeignKey("PaymentMasterDup")]
        public int PaymentMasterId { get; set; }
        public int PurchaseMasterId { get; set; } 
        public string? SerialNo { get; set; }
        public string? VoucherNo { get; set; } 
        public decimal TotalAmount { get; set; } 
        public decimal PaidAmount { get; set; } 
        public decimal DueAmount { get; set; } 
        public string? PaymentStatus { get; set; } 
        public DateTime? AddedDate { get; set; } 
        public DateTime? ModifyDate { get; set; }


        // Navigation property for linking with PaymentMasterDup
        public PaymentMasterDup PaymentMasterDup { get; set; }
    }
}
