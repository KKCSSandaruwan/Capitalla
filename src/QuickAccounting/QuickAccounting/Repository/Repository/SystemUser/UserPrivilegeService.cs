using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Setting.SystemUser;
using QuickAccounting.Repository.Interface.SystemUser;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace QuickAccounting.Repository.Repository.SystemUser
{
    public class UserPrivilegeService : IUserPrivilege
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthenticationStateProvider _authState;

        #region Constructor
        // Constructor to initialize the service with the database context and auth
        public UserPrivilegeService(ApplicationDbContext context, AuthenticationStateProvider authState)
        {
            try
            {
                _context = context ?? throw new ArgumentNullException(nameof(context), "Database context cannot be null.");
                _authState = authState;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing User Privilege Service: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Fetch Methods
        // Fetches a list of all user privileges.
        public async Task<List<UserPrivilege>> GetAllAsync()
        {
            try
            {
                var result = await (from up in _context.UserPrivilege
                                    orderby up.UserRoleId ascending
                                    select up
                                    ).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user privileges: {ex.Message}");
                throw;
            }
        }

        // Fetches a list of active user privileges.
        public async Task<List<UserPrivilege>> GetActiveAsync()
        {
            try
            {
                var result = await (from up in _context.UserPrivilege
                                    where up.Active == true
                                    orderby up.UserRoleId ascending
                                    select up
                                    ).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching active user privileges: {ex.Message}");
                throw;
            }
        }

        // Fetches a object of user privilege for a specified user privilege id.
        public async Task<UserPrivilege> GetByIdAsync(int userPrivilegeId)
        {
            try
            {
                var result = await (from up in _context.UserPrivilege
                                    where up.UserPrivilegeId == userPrivilegeId
                                    select up
                                    ).FirstOrDefaultAsync();

                if (result == null)
                    throw new KeyNotFoundException($"User privilege with ID {userPrivilegeId} was not found.");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user privilege with ID {userPrivilegeId}: {ex.Message}");
                throw;
            }
        }

        // Fetches a list of user privileges along with associated navigation menu data for a specified user role, with optional filters for menu group, main menu, and sub-menu.
        public async Task<List<UserPrivilegeView>> GetRoleMappedNavigationAsync(int userRoleId = 0, int menuGroupId = 0, int mainMenuId = 0, int subMenuId = 0)
        {
            try
            {
                var result = await (from nm in _context.NavigationMenu
                                    join mg in _context.MenuGroup
                                         on nm.MenuGroupId equals mg.MenuGroupId
                                    join mm in _context.MainMenu
                                         on nm.MainMenuId equals mm.MainMenuId
                                    join sm in _context.SubMenu
                                         on nm.SubMenuId equals sm.SubMenuId into smJoin
                                    from sm in smJoin.DefaultIfEmpty()
                                    join up in _context.UserPrivilege
                                         on new { nm.NavigationMenuId, UserRoleId = userRoleId }
                                         equals new { up.NavigationMenuId, up.UserRoleId } into upJoin
                                    from up in upJoin.DefaultIfEmpty()
                                    where (menuGroupId == 0 || nm.MenuGroupId == menuGroupId) &&
                                          (mainMenuId == 0 || nm.MainMenuId == mainMenuId) &&
                                          (subMenuId == 0 || nm.SubMenuId == subMenuId)
                                    select new UserPrivilegeView
                                    {
                                        UserPrivilegeId = up != null ? up.UserPrivilegeId : 0,
                                        UserRoleId = userRoleId,
                                        NavigationMenuId = nm.NavigationMenuId,
                                        MenuGroupName = mg.MenuGroupName,
                                        MainMenuName = mm.MainMenuName,
                                        SubMenuName = sm.SubMenuName,
                                        CanView = up != null && up.CanView,
                                        CanAdd = up != null && up.CanAdd,
                                        CanEdit = up != null && up.CanEdit,
                                        CanDelete = up != null && up.CanDelete,
                                        CreatedBy = up.CreatedBy,
                                        CreatedDate = up.CreatedDate,
                                        ModifiedBy = up.ModifiedBy,
                                        ModifiedDate = up.ModifiedDate,
                                        Active = up != null && up.Active,
                                    }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching mapped navigation privileges for user role ID {userRoleId}: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Process Methods
        // Creates or updates a user privilege based on UserPrivilegeId.
        public async Task<UserPrivilege> UpsertAsync(UserPrivilege userPrivilege)
        {
            try
            {
                if (userPrivilege == null)
                    throw new ArgumentNullException(nameof(userPrivilege), "user privilege cannot be null.");

                var authState = await _authState.GetAuthenticationStateAsync();
                string userName = authState.User.FindFirst(ClaimTypes.Name)?.Value;

                // Trim and prepare data
                userPrivilege.CreatedBy = userName;
                userPrivilege.CreatedDate = userPrivilege.CreatedDate == default ? DateTime.Now : userPrivilege.CreatedDate;

                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(userPrivilege);
                if (!Validator.TryValidateObject(userPrivilege, context, validationResults, true))
                    throw new ValidationException($"{string.Join("; ", validationResults.Select(v => v.ErrorMessage))}");

                if (userPrivilege.UserPrivilegeId != 0)
                {
                    // Confirm if the user privilege already exists
                    var existingUserPrivilege = await _context.UserPrivilege.FirstOrDefaultAsync(up => up.UserPrivilegeId == userPrivilege.UserPrivilegeId);

                    if (existingUserPrivilege == null)
                        throw new InvalidOperationException($"User privilege with ID {userPrivilege.UserPrivilegeId} does not exist.");

                    // Update the existing record
                    existingUserPrivilege.CanView = userPrivilege.CanView;
                    existingUserPrivilege.CanAdd = userPrivilege.CanAdd;
                    existingUserPrivilege.CanEdit = userPrivilege.CanEdit;
                    existingUserPrivilege.CanDelete = userPrivilege.CanDelete;
                    existingUserPrivilege.ModifiedBy = userName;
                    existingUserPrivilege.ModifiedDate = DateTime.Now;
                    existingUserPrivilege.Active = userPrivilege.Active;

                    _context.UserPrivilege.Update(existingUserPrivilege);
                }
                else
                {
                    // Add the new navigation menu
                    _context.UserPrivilege.Add(userPrivilege);
                }

                await _context.SaveChangesAsync();
                return userPrivilege;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error upserting the user privilege: {ex.Message}");
                throw;
            }
        }

        // Toggles the active status of a given user privilege. If the privilege already exists, updates it; if it doesn't exist, inserts it.
        public async Task<UserPrivilege> ToggleActiveAsync(UserPrivilege userPrivilege)
        {
            try
            {
                // Toggle the active status
                userPrivilege.Active = !userPrivilege.Active;

                // Update or insert the user privilege in the database
                await UpsertAsync(userPrivilege);

                return userPrivilege;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while toggling the active status for UserPrivilege ID: {userPrivilege.UserPrivilegeId}. Error Details: {ex.Message}");
                throw;
            }
        }

        #endregion
    }
}
