using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickAccounting.Data.Inventory
{
    [Table("ReceiptDetailsDup")]
    public class ReceiptDetailsDup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReceiptDetailsId { get; set; }

        [Required]
        [ForeignKey("ReceiptMasterDup")]
        public int ReceiptMasterId { get; set; }

        [Required]
        public int SalesMasterId { get; set; }

        [StringLength(255)]
        public string SerialNo { get; set; }

        [StringLength(255)]
        public string VoucherNo { get; set; }

        [Required]
        [StringLength(255)]
        public string TransactionStatus { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ReceivedAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal DueAmount { get; set; }

        [Required]
        [StringLength(255)]
        public string PaymentStatus { get; set; }

        public DateTime? AddedDate { get; set; }

        public DateTime? ModifyDate { get; set; }
    }
}