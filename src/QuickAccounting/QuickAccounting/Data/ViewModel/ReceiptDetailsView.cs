namespace QuickAccounting.Data.ViewModel
{
    public class ReceiptDetailsView
    {
        public int ReceiptDetailsId { get; set; }
        public int ReceiptMasterId { get; set; }
        public int SalesMasterId { get; set; }
        public int LedgerId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ReceiveAmount { get; set; }
        public decimal DueAmount { get; set; }
        public int Id { get; set; }
        public string LedgerName { get; set; }
    }
}
