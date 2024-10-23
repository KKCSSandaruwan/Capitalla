using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickAccounting.Data.Inventory
{
    public partial class PurchaseOrder
    {
        public int PurchaseOrderId { get; set; }
        public string SerialNo { get; set; } = null!;
        public string VoucherNo { get; set; } = null!;
        public int WarehouseId { get; set; }
        public int VoucherTypeId { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public int LedgerId { get; set; }
        public string? Reference { get; set; }
        public string Narration { get; set; } = null!;
        public int PurchaseOrderMasterId { get; set; }
        public int TaxId { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TaxRate { get; set; }
        public decimal DisPer { get; set; }
        public decimal BillDiscount { get; set; }
        public decimal ShippingAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal NetAmounts { get; set; }
        public decimal PayAmount { get; set; }
        public decimal BalanceDue { get; set; }
        public string Status { get; set; } = null!;
        public decimal PreviousDue { get; set; }
        public string PaymentStatus { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public int FinancialYearId { get; set; }
        public int CompanyId { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        [NotMapped]
        public List<PurchaseOrderDetail> listOrder { get; set; } = new List<PurchaseOrderDetail>();
        [NotMapped]
        public List<DeleteItem> listDelete { get; set; } = new List<DeleteItem>();
		public bool? Active { get; set; }
	}
}
