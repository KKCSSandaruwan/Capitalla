using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.HrPayroll
{
    public class PayHead
    {
        [Key]
        public int PayHeadId { get; set; }
        [Required]
        public string PayHeadName { get; set; }
        public string Type { get; set; }
        public string Narration { get; set; }
    }
}
