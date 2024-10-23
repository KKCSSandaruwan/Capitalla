using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.HrPayroll
{
    public class DailyAttendanceDetails
    {
        [Key]
        public int DailyAttendanceDetailsId { get; set; }
        public int DailyAttendanceMasterId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Employee.")]
        public int EmployeeId { get; set; }
        public string Status { get; set; }
        public string Narration { get; set; }
    }
}
