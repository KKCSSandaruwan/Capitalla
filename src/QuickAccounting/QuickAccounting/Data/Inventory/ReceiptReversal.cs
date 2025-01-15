using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.Inventory
{
    public class ReceiptReversal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReceiptReversalId { get; set; }

        [Required(ErrorMessage = "Receipt Details ID is required.")]
        public int ReceiptDetailsId { get; set; }

        [Required(ErrorMessage = "Serial Number is required.")]
        [StringLength(50, ErrorMessage = "Serial Number cannot exceed 50 characters.")]
        public string SerialNo { get; set; }

        [Required(ErrorMessage = "Voucher Number is required.")]
        [StringLength(50, ErrorMessage = "Voucher Number cannot exceed 50 characters.")]
        public string VoucherNo { get; set; }

        [Required(ErrorMessage = "Reversal Amount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Reversal Amount must be a positive value.")]
        public decimal ReversalAmount { get; set; }

        [StringLength(255, ErrorMessage = "Narration cannot exceed 255 characters.")]
        public string Narration { get; set; }

        [Required(ErrorMessage = "Added By is required.")]
        [StringLength(255, ErrorMessage = "Added By cannot exceed 255 characters.")]
        public string AddedBy { get; set; }

        [Required(ErrorMessage = "Added Date is required.")]
        public DateTime AddedDate { get; set; }
    }
}
