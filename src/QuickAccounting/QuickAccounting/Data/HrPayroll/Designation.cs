using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.HrPayroll
{
    public class Designation
    {
        [Key]
        public int DesignationId { get; set; }
        [Required]
        public string DesignationName { get; set; }
        public decimal LeaveDays { get; set; }
        public decimal AdvanceAmount { get; set; }
        public string Narration { get; set; }
    }
}
