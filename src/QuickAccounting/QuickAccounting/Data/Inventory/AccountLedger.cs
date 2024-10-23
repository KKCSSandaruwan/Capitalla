using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.Inventory
{
    public class AccountLedger
    {
        [Key]
        public int LedgerId { get; set; }
        public int AccountGroupId { get; set; }
        [Required]
        public string LedgerName { get; set; }
        public string LedgerCode { get; set; }
        public int CompanyId { get; set; }
        public Decimal OpeningBalance { get; set; }
        public bool IsDefault { get; set; }
        public string CrOrDr { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ShippingAddress { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string TaxNo { get; set; }
        public int CreditPeriod { get; set; }
        public decimal CreditLimit { get; set; }
        public string Type { get; set; }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? Active { get; set; }
    }
}
