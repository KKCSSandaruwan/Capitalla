using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.Setting.Corporate
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Company Name is required.")]
        [StringLength(255, ErrorMessage = "Company Name cannot exceed 255 characters.")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Registration Number is required.")]
        [StringLength(50, ErrorMessage = "Registration Number cannot exceed 50 characters.")]
        public string RegistrationNo { get; set; }

        [Required(ErrorMessage = "VAT Number is required.")]
        [StringLength(50, ErrorMessage = "VAT Number cannot exceed 50 characters.")]
        public string VATNo { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(100, ErrorMessage = "Address cannot exceed 100 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
        public string City { get; set; }

        [StringLength(100, ErrorMessage = "State cannot exceed 100 characters.")]
        public string? State { get; set; }

        [Required(ErrorMessage = "Postal Code is required.")]
        [StringLength(20, ErrorMessage = "Postal Code cannot exceed 20 characters.")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(100, ErrorMessage = "Country cannot exceed 100 characters.")]
        public string Country { get; set; }

        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90.")]
        public decimal? Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180.")]
        public decimal? Longitude { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [StringLength(20, ErrorMessage = "Phone Number cannot exceed 20 characters.")]
        public string PhoneNo { get; set; }

        [StringLength(20, ErrorMessage = "Mobile Number cannot exceed 20 characters.")]
        public string? MobileNo { get; set; }

        [StringLength(20, ErrorMessage = "Fax Number cannot exceed 20 characters.")]
        public string? FaxNo { get; set; }

        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [StringLength(255, ErrorMessage = "Website URL cannot exceed 255 characters.")]
        public string? Website { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Currency ID must be a positive number.")]
        public int? CurrencyId { get; set; }

        // Non-nullable foreign keys and required properties
        [Required]
        public int FinancialYearId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Number of decimals must be a positive integer.")]
        public int NoofDecimal { get; set; }

        // Optional fields (nullable in SQL)
        public string? Logo { get; set; }

        // Audit fields
        [Required]
        [StringLength(255, ErrorMessage = "Added By cannot exceed 255 characters.")]
        public string AddedBy { get; set; }

        [Required]
        public DateTime AddedDate { get; set; }

        [StringLength(255, ErrorMessage = "Modified By cannot exceed 255 characters.")]
        public string? ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }
    }
}
