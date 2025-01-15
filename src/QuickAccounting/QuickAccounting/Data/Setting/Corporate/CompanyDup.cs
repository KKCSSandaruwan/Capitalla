using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickAccounting.Data.Setting.Corporate
{
    public class CompanyDup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Company ID")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(150, ErrorMessage = "Company name cannot exceed 150 characters.")]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Registration number is required.")]
        [StringLength(55, ErrorMessage = "Registration number cannot exceed 55 characters.")]
        [Display(Name = "Registration Number")]
        public string RegistrationNo { get; set; }

        [Required(ErrorMessage = "VAT number is required.")]
        [StringLength(50, ErrorMessage = "VAT number cannot exceed 50 characters.")]
        [Display(Name = "VAT Number")]
        public string VATNo { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(100, ErrorMessage = "Address cannot exceed 100 characters.")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
        [Display(Name = "City")]
        public string? City { get; set; }

        [StringLength(100, ErrorMessage = "State cannot exceed 100 characters.")]
        [Display(Name = "State")]
        public string? State { get; set; }

        [Required(ErrorMessage = "Postal code is required.")]
        [StringLength(20, ErrorMessage = "Postal code cannot exceed 20 characters.")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [StringLength(100, ErrorMessage = "Country cannot exceed 100 characters.")]
        [Display(Name = "Country")]
        public string? Country { get; set; }

        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90.")]
        [Display(Name = "Latitude")]
        public decimal? Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180.")]
        [Display(Name = "Longitude")]
        public decimal? Longitude { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 digits.")]
        [RegularExpression(@"^(?:\+?[1-9]\d{1,14}|\d{7,15})$", ErrorMessage = "Invalid phone number format.")]
        [Display(Name = "Phone Number")]
        public string PhoneNo { get; set; }

        [StringLength(15, ErrorMessage = "Mobile number cannot exceed 15 digits.")]
        [RegularExpression(@"^(?:\+?[1-9]\d{1,14}|\d{7,15})$", ErrorMessage = "Invalid mobile number format.")]
        [Display(Name = "Mobile Number")]
        public string? MobileNo { get; set; }

        [StringLength(15, ErrorMessage = "Fax number cannot exceed 15 digits.")]
        [RegularExpression(@"^(?:\+?[1-9]\d{1,14}|\d{7,15})$", ErrorMessage = "Invalid fax number format.")]
        [Display(Name = "Fax Number")]
        public string? FaxNo { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [StringLength(255, ErrorMessage = "Website URL cannot exceed 255 characters.")]
        [Display(Name = "Website")]
        public string? Website { get; set; }

        [StringLength(255, ErrorMessage = "Logo path cannot exceed 255 characters.")]
        [Display(Name = "Logo")]
        public string? LogoPath { get; set; }

        [Display(Name = "Mother Company")]
        public bool IsMotherCompany { get; set; } = false;

        [Required(ErrorMessage = "Created by is required.")]
        [StringLength(50, ErrorMessage = "Created by name cannot exceed 50 characters.")]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Required(ErrorMessage = "Created date is required.")]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [StringLength(50, ErrorMessage = "Modified by name cannot exceed 50 characters.")]
        [Display(Name = "Modified By")]
        public string? ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Required(ErrorMessage = "Active status is required.")]
        [Display(Name = "Active")]
        public bool Active { get; set; } = true;


        // Computed properties
        [NotMapped]
        [Display(Name = "Full Address")]
        public string FullAddress =>
            $"{Address}, {(string.IsNullOrEmpty(City) ? string.Empty : City + ", ")}" +
            $"{(string.IsNullOrEmpty(State) ? string.Empty : State + ", ")}" +
            $"{PostalCode}" +
            $"{(string.IsNullOrEmpty(Country) ? string.Empty : ", " + Country)}.";
    }
}
