using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickAccounting.Data.Setting.Inventory
{
    public class InventorySetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Inventory Setting ID")]
        public int InventorySettingsId { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The selected company ID is invalid.")]
        [ForeignKey("Company")]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Stock valuation Method is required.")]
        [StringLength(20, ErrorMessage = "Stock Valuation Method cannot exceed 20 characters.")]
        [Display(Name = "Stock Valuation Method")]
        public string StockValuationMethod { get; set; }

        [Required(ErrorMessage = "Allow Negative Stock is required.")]
        [Display(Name = "Allow Negative Stock")]
        public bool AllowNegativeStock { get; set; }

        [Required(ErrorMessage = "Enable Expiry Tracking is required.")]
        [Display(Name = "Enable Expiry Tracking")]
        public bool EnableExpiryTracking { get; set; }

        [Required(ErrorMessage = "Expiry Notification Days is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Expiry Notification Days must be a positive number.")]
        [Display(Name = "Expiry Notification Days")]
        public int ExpiryNotificationDays { get; set; }

        [Required(ErrorMessage = "Enable Barcoding is required.")]
        [Display(Name = "Enable Barcoding")]
        public bool EnableBarcoding { get; set; }

        [Required(ErrorMessage = "Created By is required.")]
        [StringLength(50, ErrorMessage = "Created By cannot exceed 50 characters.")]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Required(ErrorMessage = "Created Date is required.")]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [StringLength(50, ErrorMessage = "Modified By cannot exceed 50 characters.")]
        [Display(Name = "Modified By")]
        public string? ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Required(ErrorMessage = "Active status is required.")]
        [Display(Name = "Active")]
        public bool Active { get; set; } = true;
    }
}
