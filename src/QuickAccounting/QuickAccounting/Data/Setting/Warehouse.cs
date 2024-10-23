using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.Setting
{
    public class Warehouse
    {
        [Key]
        public int WarehouseId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public int CompanyId { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
