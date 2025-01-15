using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Setting.SystemUser;
using QuickAccounting.Repository.Interface.SystemUser;
using QuickAccounting.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace QuickAccounting.Repository.Repository.SystemUser
{
    public class UserRoleService : IUserRole
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthenticationStateProvider _authState;

        #region Constructor
        // Constructor to initialize the service with the database context and auth
        public UserRoleService(ApplicationDbContext context, AuthenticationStateProvider authState)
        {
            try
            {
                _context = context ?? throw new ArgumentNullException(nameof(context), "Database context cannot be null.");
                _authState = authState;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing UserRole Service: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Fetch Methods
        // Fetches a list of all user roles, ordered by user role name in ascending order.
        public async Task<List<UserRole>> GetAllAsync()
        {
            try
            {
                var result = await (from ur in _context.UserRole
                                    orderby ur.UserRoleName ascending
                                    select ur
                                    ).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user roles: {ex.Message}");
                throw;
            }
        }

        // Fetches a list of active user roles, ordered by user role name in ascending order.
        public async Task<List<UserRole>> GetActiveAsync()
        {
            try
            {
                var result = await (from ur in _context.UserRole
                                    where ur.Active == true
                                    orderby ur.UserRoleName ascending
                                    select ur
                                    ).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving active user roles: {ex.Message}");
                throw;
            }
        }

        // Fetches a single user role by its ID.
        public async Task<UserRole> GetByIdAsync(int userRoleId)
        {
            try
            {
                var result = await (from ur in _context.UserRole
                                    where ur.UserRoleId == userRoleId
                                    select ur
                                    ).FirstOrDefaultAsync();

                if (result == null)
                    throw new KeyNotFoundException($"User role with ID {userRoleId} was not found.");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user role with ID {userRoleId}: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Process Methods
        // Creates or updates a user role based on user role id.
        public async Task<UserRole> UpsertAsync(UserRole userRole)
        {
            try
            {
                // Null check
                if (userRole == null)
                    throw new ArgumentNullException(nameof(userRole), "User role cannot be null.");

                // Fetch authentication state
                var authState = await _authState.GetAuthenticationStateAsync();
                string userName = authState.User.FindFirst(ClaimTypes.Name)?.Value;

                // Trim and standardize inputs
                userRole.UserRoleName = userRole.UserRoleName.Contains(" ") ? StringFormatter.ToPascalCase(userRole.UserRoleName.Trim()) : userRole.UserRoleName.Trim();
                userRole.Description = userRole.Description?.Trim();
                userRole.CreatedBy = userName;
                userRole.CreatedDate = userRole.CreatedDate == default ? DateTime.Now : userRole.CreatedDate;
                userRole.Active = userRole.Active;

                // Validate the UserRole object
                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(userRole);
                if (!Validator.TryValidateObject(userRole, context, validationResults, true))
                    throw new ValidationException($"{string.Join("; ", validationResults.Select(v => v.ErrorMessage))}");

                // Check if the user role name is already placed
                var existingUserRoleName = await _context.UserRole.FirstOrDefaultAsync(ur => ur.UserRoleName == userRole.UserRoleName && ur.UserRoleId != userRole.UserRoleId);
                if (existingUserRoleName != null)
                    throw new InvalidOperationException($"A user role with the name '{userRole.UserRoleName}' already exists. Please choose a different role name.");

                // Check if the user role already exists
                var existingUserRole = await _context.UserRole.FirstOrDefaultAsync(ur => ur.UserRoleId == userRole.UserRoleId);
                if (existingUserRole != null)
                {
                    // Update existing record
                    existingUserRole.UserRoleName = userRole.UserRoleName;
                    existingUserRole.Description = userRole.Description;
                    existingUserRole.ModifiedBy = userName;
                    existingUserRole.ModifiedDate = DateTime.Now;
                    existingUserRole.Active = userRole.Active;

                    _context.UserRole.Update(existingUserRole);
                }
                else
                {
                    // Add new record
                    _context.UserRole.Add(userRole);
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                return userRole;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error upserting the user role: {ex.Message}");
                throw;
            }
        }

        // Toggles the active status of a user role by its ID.
        public async Task<UserRole> ToggleActiveAsync(int userRoleId)
        {
            try
            {
                // Fetch the user role by ID
                var userRole = await _context.UserRole.FirstOrDefaultAsync(ur => ur.UserRoleId == userRoleId);

                // Null check
                if (userRole == null)
                    throw new KeyNotFoundException($"User role with ID {userRoleId} was not found.");

                // Fetch authentication state
                var authState = await _authState.GetAuthenticationStateAsync();
                string userName = authState.User.FindFirst(ClaimTypes.Name)?.Value;

                // Toggle the active status
                userRole.Active = !userRole.Active;
                userRole.ModifiedBy = userName;
                userRole.ModifiedDate = DateTime.Now;

                // Update the user role in the database
                _context.UserRole.Update(userRole);
                await _context.SaveChangesAsync();

                return userRole;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error toggling Active status for user role with ID {userRoleId}: {ex.Message}");
                throw;
            }
        }

        #endregion
    }
}
