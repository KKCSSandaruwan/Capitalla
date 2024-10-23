using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.Setting
{
    public class Brand
    {
        [Key]
        public int BrandId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Image { get; set; }
        public int CompanyId { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
