using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.HrPayrollModel
{
    public class DesignationView
	{
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public decimal LeaveDays { get; set; }
        public decimal AdvanceAmount { get; set; }
        public string Narration { get; set; }
    }
}
