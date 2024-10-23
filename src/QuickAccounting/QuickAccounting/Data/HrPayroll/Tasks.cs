using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.HrPayroll
{
    public class Tasks
    {
        [Key]
        public int TaskId { get; set; }
        [Required]
        public string TaskName { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a taskcategory.")]
        public int TaskcategoryId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a taskpriority.")]
        public int TaskpriorityId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a employee.")]
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
