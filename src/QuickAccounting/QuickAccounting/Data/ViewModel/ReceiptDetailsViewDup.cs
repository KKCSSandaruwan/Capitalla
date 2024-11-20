namespace QuickAccounting.Data.ViewModel
{
    public class ReceiptDetailsViewDup
    {
        public int ReceiptDetailsId { get; set; }
        public int ReceiptMasterId { get; set; }
        public int SalesMasterId { get; set; }
        public int LedgerId { get; set; }
        public string LedgerName { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string VoucherNo { get; set; }
        public string TransactionType { get; set; }
        public string TransactionStatus { get; set; }
        public string InvoiceType { get; set; }
        public string ProcessType { get; set; }
        public decimal ReceiveableAmount { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal DueAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string Narration { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
