using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.HrPayroll
{
    public class MonthlySalaryDetails
    {
        [Key]
        public int MonthlySalaryDetailsId { get; set; }
        public int EmployeeId { get; set; }
        public int SalaryPackageId { get; set; }
        public int MonthlySalaryId { get; set; }
    }
}
