using QuickAccounting.Data.Inventory;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickAccounting.Data.HrPayroll
{
    public class DailyAttendanceMaster
    {
        [Key]
        public int DailyAttendanceMasterId { get; set; }
        public DateTime Date { get; set; }
        public string Narration { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
		[NotMapped]
		public List<DailyAttendanceDetails> listOrder { get; set; } = new List<DailyAttendanceDetails>();
	}
}
