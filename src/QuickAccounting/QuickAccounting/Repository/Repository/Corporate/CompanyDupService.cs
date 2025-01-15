using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Setting.Corporate;
using QuickAccounting.Repository.Interface.Corporate;
using QuickAccounting.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace QuickAccounting.Repository.Repository.Corporate
{
    public class CompanyDupService : ICompanyDup
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthenticationStateProvider _authState;

        #region Constructor
        // Constructor to initialize the service with the database context and auth
        public CompanyDupService(ApplicationDbContext context, AuthenticationStateProvider authState)
        {
            try
            {
                _context = context ?? throw new ArgumentNullException(nameof(context), "Database context cannot be null.");
                _authState = authState;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing CompanyDup Service: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Fetch Methods
        // Fetches a list of all companies, ordered by CompanyName in ascending order.
        public async Task<List<CompanyDup>> GetAllAsync()
        {
            try
            {
                var result = await (from c in _context.CompanyDup
                                    orderby c.CompanyName ascending
                                    select new CompanyDup
                                    {
                                        CompanyId = c.CompanyId,
                                        CompanyName = c.CompanyName,
                                        RegistrationNo = c.RegistrationNo,
                                        VATNo = c.VATNo,
                                        Address = c.Address,
                                        City = c.City,
                                        State = c.State,
                                        PostalCode = c.PostalCode,
                                        Country = c.Country,
                                        PhoneNo = c.PhoneNo,
                                        MobileNo = c.MobileNo,
                                        FaxNo = c.FaxNo,
                                        Email = c.Email,
                                        Website = c.Website,
                                        IsMotherCompany = c.IsMotherCompany,
                                        Active = c.Active
                                    }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching companies: {ex.Message}");
                throw;
            }
        }

        // Fetches a list of active companies, ordered by CompanyName in ascending order.
        public async Task<List<CompanyDup>> GetActiveAsync()
        {
            try
            {
                var result = await (from c in _context.CompanyDup
                                    where c.Active == true
                                    orderby c.CompanyName ascending
                                    select new CompanyDup
                                    {
                                        CompanyId = c.CompanyId,
                                        CompanyName = c.CompanyName,
                                        RegistrationNo = c.RegistrationNo,
                                        VATNo = c.VATNo,
                                        Address = c.Address,
                                        City = c.City,
                                        State = c.State,
                                        PostalCode = c.PostalCode,
                                        Country = c.Country,
                                        PhoneNo = c.PhoneNo,
                                        MobileNo = c.MobileNo,
                                        FaxNo = c.FaxNo,
                                        Email = c.Email,
                                        Website = c.Website,
                                        IsMotherCompany = c.IsMotherCompany,
                                        Active = c.Active
                                    }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching active companies: {ex.Message}");
                throw;
            }
        }

        // Fetches a object of company for a specified company id.
        public async Task<CompanyDup> GetByIdAsync(int companyId)
        {
            try
            {
                var result = await (from c in _context.CompanyDup
                                    where c.CompanyId == companyId
                                    select c
                                    ).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving company with ID {companyId}: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Process Methods
        // Creates a new company or updates an existing one based on CompanyID.
        public async Task<CompanyDup> UpsertAsync(CompanyDup company)
        {
            try
            {
                // Null check
                if (company == null)
                    throw new ArgumentNullException(nameof(company), "Company cannot be null.");

                // Fetch authentication state
                var authState = await _authState.GetAuthenticationStateAsync();
                string userName = authState.User.FindFirst(ClaimTypes.Name)?.Value;

                // Trim and standardize inputs
                company.CompanyName = StringFormatter.ToTitleCase(company.CompanyName?.Trim());
                company.RegistrationNo = StringFormatter.ToUpperCase(company.RegistrationNo?.Trim());
                company.VATNo = StringFormatter.ToUpperCase(company.VATNo?.Trim());
                company.Address = StringFormatter.ToTitleCase(company.Address?.Trim());
                company.City = StringFormatter.ToTitleCase(company.City?.Trim());
                company.State = StringFormatter.ToTitleCase(company.State?.Trim());
                company.PostalCode = StringFormatter.ToUpperCase(company.PostalCode?.Trim());
                company.Country = StringFormatter.ToTitleCase(company.Country?.Trim());
                company.PhoneNo = company.PhoneNo?.Trim();
                company.MobileNo = company.MobileNo?.Trim();
                company.FaxNo = company.FaxNo?.Trim();
                company.Email = StringFormatter.ToLowerCase(company.Email?.Trim());
                company.Website = StringFormatter.ToLowerCase(company.Website?.Trim());
                company.LogoPath = StringFormatter.ToLowerCase(company.LogoPath?.Trim());
                company.CreatedBy = userName;
                company.CreatedDate = company.CreatedDate == default ? DateTime.Now : company.CreatedDate;

                // Validate the UserProfile object
                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(company);
                if (!Validator.TryValidateObject(company, context, validationResults, true))
                    throw new ValidationException($"{string.Join("; ", validationResults.Select(v => v.ErrorMessage))}");

                // Check if a company with the same name already exists
                var existingCompanyName = await _context.CompanyDup.FirstOrDefaultAsync(c => c.CompanyName == company.CompanyName && c.CompanyId != company.CompanyId);
                if (existingCompanyName != null)
                    throw new ValidationException($"A company with the name '{company.CompanyName}' already exists. Please choose a different name.");

                // Check if the company already exists
                var existingCompany = await _context.CompanyDup.FirstOrDefaultAsync(c => c.CompanyId == company.CompanyId);
                if (existingCompany != null)
                {
                    // Update existing record
                    existingCompany.CompanyName = company.CompanyName;
                    existingCompany.RegistrationNo = company.RegistrationNo;
                    existingCompany.VATNo = company.VATNo;
                    existingCompany.Address = company.Address;
                    existingCompany.City = company.City;
                    existingCompany.State = company.State;
                    existingCompany.PostalCode = company.PostalCode;
                    existingCompany.Country = company.Country;
                    existingCompany.PhoneNo = company.PhoneNo;
                    existingCompany.MobileNo = company.MobileNo;
                    existingCompany.FaxNo = company.FaxNo;
                    existingCompany.Email = company.Email;
                    existingCompany.Website = company.Website;
                    existingCompany.LogoPath = company.LogoPath;
                    existingCompany.ModifiedBy = userName;
                    existingCompany.ModifiedDate = DateTime.Now;
                    existingCompany.IsMotherCompany = company.IsMotherCompany;
                    existingCompany.Active = company.Active;

                    _context.CompanyDup.Update(existingCompany);
                }
                else
                {
                    // Add new record
                    _context.CompanyDup.Add(company);
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Reset the "IsMotherCompany" flag for other companies if this company is set as a mother company
                if (company.IsMotherCompany)
                    await ResetMotherCompanyFlagAsync(company.CompanyId);

                return company;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error upserting the company: {ex.Message}");
                throw;
            }
        }

        // Toggles the active status of a company by its ID.
        public async Task<CompanyDup> ToggleActiveAsync(int companyId)
        {
            try
            {
                // Fetch the company by ID
                var company = await _context.CompanyDup.FirstOrDefaultAsync(c => c.CompanyId == companyId);

                // Null check
                if (company == null)
                    throw new KeyNotFoundException($"Company with ID {companyId} was not found.");

                // Fetch authentication state
                var authState = await _authState.GetAuthenticationStateAsync();
                string userName = authState.User.FindFirst(ClaimTypes.Name)?.Value;

                // Toggle the active status
                company.Active = !company.Active;
                company.ModifiedBy = userName;
                company.ModifiedDate = DateTime.Now;

                // Update the company in the database
                _context.CompanyDup.Update(company);
                await _context.SaveChangesAsync();

                return company;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error toggling Active status for company with ID {companyId}: {ex.Message}");
                throw;
            }
        }

        /// Sets the IsMotherCompany flag to false for all companies except the one with the specified companyId.
        /// This ensures that only one company retains the IsMotherCompany flag as true while others are updated to false.
        private async Task ResetMotherCompanyFlagAsync(int companyId)
        {
            try
            {
                // Fetch all companies except the one with the specified companyId
                var companiesToUpdate = await _context.CompanyDup.
                                                Where(c => c.CompanyId != companyId).
                                                ToListAsync();

                // Iterate through each company and set the IsMotherCompany flag to false
                foreach (var company in companiesToUpdate)
                    company.IsMotherCompany = false;

                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
