using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.HrPayroll
{
    public class TaskPriority
    {
        [Key]
        public int TaskpriorityId { get; set; }
        [Required(ErrorMessage ="Type taskpriority")]
        public string Name { get; set; }
        [Required(ErrorMessage ="choose color")]
        public string Color { get; set; }
        public string Description { get; set; }
    }
}
