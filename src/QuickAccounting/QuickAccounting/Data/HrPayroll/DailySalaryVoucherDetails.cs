using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickAccounting.Data.HrPayroll
{
    public class DailySalaryVoucherDetails
    {
        [Key]
        public int DailySalaryVoucherDetailsId { get; set; }
        public int DailySalaryVoucherMasterId { get; set; }
        public int EmployeeId { get; set; }
        public decimal Wage { get; set; }
        public string Status { get; set; }
        [NotMapped]
        public string YearMonthDay { get; set; }
    }
}
