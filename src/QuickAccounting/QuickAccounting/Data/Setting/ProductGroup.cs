using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.Setting
{
    public class ProductGroup
    {
        [Key]
        public int GroupId { get; set; }
        [Required]
        public string GroupName { get; set; }
        public int GroupUnder { get; set; }
        public string Image { get; set; }
        public string Narration { get; set; }
        public int CompanyId { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
