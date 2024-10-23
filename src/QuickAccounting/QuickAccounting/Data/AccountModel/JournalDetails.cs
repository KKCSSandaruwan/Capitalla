using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.AccountModel
{
    public class JournalDetails
    {
        [Key]
        public int JournalDetailsId { get; set; }
        public int JournalMasterId { get; set; }
        public int LedgerId { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public string Narration { get; set; }
    }
}
