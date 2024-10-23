using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.ViewModel
{
    public class WarehouseView
    {
        public int WarehouseId { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public int CompanyId { get; set; }
    }
}
