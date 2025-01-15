namespace QuickAccounting.Data.Setting
{
    public class ProductView
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public decimal Qty { get; set; }
        public decimal Rate { get; set; }
        public string ProductName { get; set; }
        public int UnitId { get; set; }
        public decimal PurchaseRate { get; set; }
        public decimal SalesRate { get; set; }
        public decimal Mrp { get; set; }
        public string Narration { get; set; }
        public bool IsActive { get; set; }
        public string Barcode { get; set; }
        public string Image { get; set; }
        public string UnitName { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public int TaxId { get; set; }
        public decimal VatAmount { get; set; }
        public decimal SCCSLAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal ShippingAmount { get; set; }
        public decimal TaxRate { get; set; }
        public string TaxName { get; set; }
        public string TaxCode { get; set; }
        public string BrandName { get; set; }
        public string BatchNo { get; set; }
        public int BatchId { get; set; }
        public int CurrentStock { get; set; }
        //PurchaseInvoice
        public int PurchaseDetailsId { get; set; }
		public int PurchaseOrderDetailsId { get; set; }
		public int ReceiptDetailsId { get; set; }
        public int OrderDetailsId { get; set; }
        //PurchaseReturn
        public int PurchaseReturnDetailsId { get; set; }

        //SalesInvoice
        public int SalesDetailsId { get; set; }
        public int DeliveryNoteDetailsId { get; set; }
        public int QuotationDetailsId { get; set; }

        //SalesReturn
        public int SalesReturnMasterId { get; set; }
        public int SalesReturnDetailsId { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public decimal SalesReturnTotalAmount { get; set; }
        public decimal SalesReturnGrandTotal { get; set; }

        //StockPosting
        public int StockPostingId { get; set; }
    }
}
