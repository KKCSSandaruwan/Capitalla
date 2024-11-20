using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickAccounting.Data.Inventory
{
    [Table("ReceiptDetailsDup")]
    public class ReceiptDetailsDup
    {
        [Key]
        public int ReceiptDetailsId { get; set; }

        public int ReceiptMasterId { get; set; }

        public int SalesMasterId { get; set; }

        public string SerialNo { get; set; }

        public string VoucherNo { get; set; }

        public string TransactionStatus { get; set; }

        public decimal ReceiveableAmount { get; set; }

        public decimal ReceivedAmount { get; set; }

        public decimal DueAmount { get; set; }

        public string PaymentStatus { get; set; }

        public string AddedBy { get; set; }

        public DateTime AddedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}