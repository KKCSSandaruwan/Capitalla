using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickAccounting.Data.Inventory
{
    [Table("ReceiptMasterDup")]
    public class ReceiptMasterDup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReceiptMasterId { get; set; }

        [Required]
        public int LedgerId { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Required]
        [StringLength(255)]
        public string TransactionType { get; set; }

        [Required]
        [StringLength(255)]
        public string InvoiceType { get; set; }

        [Required]
        [StringLength(255)]
        public string ProcessType { get; set; }

        [Required]
        public int FinancialYearId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        [StringLength(255)]
        public string UserId { get; set; }

        [StringLength(255)]
        public string Narration { get; set; }

        public DateTime? AddedDate { get; set; }

        public DateTime? ModifyDate { get; set; }
    }
}
