using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.HrPayroll
{
    public class SalaryVoucherDetails
    {
        [Key]
        public int SalaryVoucherDetailsId { get; set; }
        public int SalaryVoucherMasterId { get; set; }
        public int EmployeeId { get; set; }
        public decimal Bonus { get; set; }
        public decimal Deduction { get; set; }
        public decimal Advance { get; set; }
        public decimal Lop { get; set; }
        public decimal Salary { get; set; }
        public string Status { get; set; }
    }
}
