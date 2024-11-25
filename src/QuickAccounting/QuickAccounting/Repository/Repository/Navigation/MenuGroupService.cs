using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Setting.Navigation;
using QuickAccounting.Repository.Interface.Navigation;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace QuickAccounting.Repository.Repository.Navigation
{
    public class MenuGroupService : IMenuGroup
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthenticationStateProvider _authState;

        #region Constructor
        // Constructor to initialize the service with the database context and auth
        public MenuGroupService(ApplicationDbContext context, AuthenticationStateProvider authState)
        {
            try
            {
                _context = context ?? throw new ArgumentNullException(nameof(context), "Database context cannot be null.");
                _authState = authState;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing Navigation Menu Service: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Fetch Mathods
        // Fetches a list of all menu groups.
        public async Task<List<MenuGroup>> GetAllAsync()
        {
            try
            {
                var result = await (from mg in _context.MenuGroup
                                    orderby mg.MenuGroupId ascending
                                    select mg
                                    ).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching menu groups: {ex.Message}");
                throw;
            }
        }

        // Fetches a list of active menu groups.
        public async Task<List<MenuGroup>> GetActiveAsync()
        {
            try
            {
                var result = await (from mg in _context.MenuGroup
                                    where mg.Active == true
                                    orderby mg.MenuGroupId ascending
                                    select mg
                                    ).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving active menu groups: {ex.Message}");
                throw;
            }
        }

        // Fetches a object of menu group for a specified menu group id.
        public async Task<MenuGroup> GetByIdAsync(int menuGroupId)
        {
            try
            {
                var result = await (from mg in _context.MenuGroup
                                    where mg.MenuGroupId == menuGroupId
                                    select mg
                                    ).FirstOrDefaultAsync();

                if (result == null)
                    throw new KeyNotFoundException($"Menu group with ID {menuGroupId} was not found.");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving menu group with ID {menuGroupId}: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Process Mathods
        // Creates a new menu group or updates an existing one based on MenuGroupId.
        public async Task<MenuGroup> UpsertAsync(MenuGroup menuGroup)
        {
            try
            {
                // Null check
                if (menuGroup == null)
                    throw new ArgumentNullException(nameof(menuGroup), "Menu group cannot be null.");

                // Fetch authentication state
                var authState = await _authState.GetAuthenticationStateAsync();
                string userName = authState.User.FindFirst(ClaimTypes.Name)?.Value;

                // Trim and standardize inputs
                menuGroup.MenuGroupName = menuGroup.MenuGroupName.Trim().ToUpper();
                menuGroup.IconName = menuGroup.IconName.Trim().ToLower();
                menuGroup.CreatedBy = userName;
                menuGroup.CreatedDate = menuGroup.CreatedDate == default ? DateTime.Now : menuGroup.CreatedDate;
                menuGroup.Active = menuGroup.Active;

                // Validate the MenuGroup object
                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(menuGroup);
                if (!Validator.TryValidateObject(menuGroup, context, validationResults, true))
                    throw new ValidationException($"{string.Join("; ", validationResults.Select(v => v.ErrorMessage))}");

                // Check if the menu group already exists
                var existingMenuGroup = await _context.MenuGroup.FirstOrDefaultAsync(mg => mg.MenuGroupId == menuGroup.MenuGroupId);

                if (existingMenuGroup != null)
                {
                    // Update existing record
                    existingMenuGroup.MenuGroupName = menuGroup.MenuGroupName;
                    existingMenuGroup.IconName = menuGroup.IconName;
                    existingMenuGroup.Description = menuGroup.Description;
                    existingMenuGroup.ModifiedBy = userName;
                    existingMenuGroup.ModifiedDate = DateTime.Now;
                    existingMenuGroup.Active = menuGroup.Active;

                    _context.MenuGroup.Update(existingMenuGroup);
                }
                else
                {
                    // Add new record
                    _context.MenuGroup.Add(menuGroup);
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                return menuGroup;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error upserting the menu group: {ex.Message}");
                throw;
            }
        }

        // Toggles the active status of a menu group by its ID.
        public async Task<MenuGroup> ToggleActiveAsync(int menuGroupId)
        {
            try
            {
                // Fetch the menu group by ID
                var menuGroup = await _context.MenuGroup.FirstOrDefaultAsync(mg => mg.MenuGroupId == menuGroupId);

                // Null check
                if (menuGroup == null)
                    throw new KeyNotFoundException($"Menu group with ID {menuGroupId} was not found.");

                // Fetch authentication state
                var authState = await _authState.GetAuthenticationStateAsync();
                string userName = authState.User.FindFirst(ClaimTypes.Name)?.Value;

                // Toggle the active status
                menuGroup.Active = !menuGroup.Active;
                menuGroup.ModifiedBy = userName;
                menuGroup.ModifiedDate = DateTime.Now;

                // Update the menu group in the database
                _context.MenuGroup.Update(menuGroup);
                await _context.SaveChangesAsync();

                return menuGroup;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error toggling Active status for menu group with ID {menuGroupId}: {ex.Message}");
                throw;
            }
        }

        #endregion
    }
}
