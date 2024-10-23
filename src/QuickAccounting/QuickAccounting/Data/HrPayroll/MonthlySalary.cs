using QuickAccounting.Data.Inventory;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickAccounting.Data.HrPayroll
{
    public class MonthlySalary
    {
        [Key]
        public int MonthlySalaryId { get; set; }
        public DateTime SalaryMonth { get; set; }
        public string YearMonth { get; set; }
        public string Narration { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        [NotMapped]
        public List<MonthlySalaryDetails> listOrder { get; set; } = new List<MonthlySalaryDetails>();
    }
}
