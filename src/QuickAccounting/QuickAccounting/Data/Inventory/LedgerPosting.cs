using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.Inventory
{
    public class LedgerPosting
    {
        [Key]
        public int LedgerPostingId { get; set; }
        public DateTime Date { get; set; }
        public string NepaliDate { get; set; }
        public int VoucherTypeId { get; set; }
        public string VoucherNo { get; set; }
        public int LedgerId { get; set; }

        /// <summary>
        /// (හර)
        /// Amount entered as a debit in double-entry accounting. 
        /// Used to increase asset or expense accounts and decrease liabilities or revenue.
        /// </summary>
        public decimal Debit { get; set; }

        /// <summary>
        /// (බැ ර)
        /// Amount entered as a credit in double-entry accounting. 
        /// Used to decrease asset or expense accounts and increase liabilities or revenue.
        /// </summary>
        public decimal Credit { get; set; }
        public int DetailsId { get; set; }
        public int YearId { get; set; }
        public string InvoiceNo { get; set; }
        public string ChequeNo { get; set; }
        public string ChequeDate { get; set; }
        public int CompanyId { get; set; }
        public string ReferenceN { get; set; }
        public string LongReference { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? Active { get; set; }
    }
}
