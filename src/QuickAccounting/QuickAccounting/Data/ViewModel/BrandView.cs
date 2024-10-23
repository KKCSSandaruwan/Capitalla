using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.ViewModel
{
    public class BrandView
    {
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int CompanyId { get; set; }
    }
}
