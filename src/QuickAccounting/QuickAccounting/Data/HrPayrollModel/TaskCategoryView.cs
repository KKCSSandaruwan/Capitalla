using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.HrPayroll
{
    public class TaskCategoryView
    {
        public int TaskcategoryId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
    }
}
