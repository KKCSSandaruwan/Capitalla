using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.HrPayroll
{
    public class TaskCategory
    {
        [Key]
        public int TaskcategoryId { get; set; }
        [Required(ErrorMessage ="Type taskcategory")]
        public string Name { get; set; }
        [Required(ErrorMessage ="choose color")]
        public string Color { get; set; }
        public string Description { get; set; }
    }
}
