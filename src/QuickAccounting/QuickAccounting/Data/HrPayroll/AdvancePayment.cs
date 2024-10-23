using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.HrPayroll
{
    public class AdvancePayment
    {
        [Key]
        public int AdvancePaymentId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Employee.")]
        public int EmployeeId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Account.")]
        public int LedgerId { get; set; }
        public string SerialNo { get; set; }
        public string YearMonth { get; set; }
		[Required]
        public string VoucherNo { get; set; }
        public string InvoiceNo { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime SalaryMonth { get; set; }
        public string Narration { get; set; }
        public int VoucherTypeId { get; set; }
		public int FinancialYearId { get; set; }
		public DateTime? AddedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
