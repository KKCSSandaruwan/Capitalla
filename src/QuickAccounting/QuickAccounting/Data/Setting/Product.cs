using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickAccounting.Data.Setting
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string ProductCode { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
        public int GroupId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a brand.")]
        public int BrandId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a unit.")]
        public int UnitId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a tax.")]
        public int TaxId { get; set; }
        public decimal PurchaseRate { get; set; }
        public decimal SalesRate { get; set; }
        public decimal Mrp { get; set; }
        public string Narration { get; set; }
        public int QtyAlert { get; set; }
        public bool IsActive { get; set; }
        public string Barcode { get; set; }
        public string Image { get; set; }
        public int CompanyId { get; set; }
        public int OpeningStock { get; set; }
        public DateTime ExiparyDate { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        [NotMapped]
        public List<ProductView> Variants { get; set; } = new List<ProductView>();
    }
}
