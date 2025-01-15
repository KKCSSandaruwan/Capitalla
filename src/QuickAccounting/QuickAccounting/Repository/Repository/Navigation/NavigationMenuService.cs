using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Setting.Navigation;
using QuickAccounting.Repository.Interface.Navigation;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace QuickAccounting.Repository.Repository.Navigation
{
    public class NavigationMenuService : INavigationMenu
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthenticationStateProvider _authState;

        #region Constructor
        // Constructor to initialize the service with the database context and authentication state.
        public NavigationMenuService(ApplicationDbContext context, AuthenticationStateProvider authState)
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

        #region Fetch Methods
        // Fetches a list of all navigation menus.
        public async Task<List<NavigationMenu>> GetAllAsync()
        {
            try
            {
                var result = await (from nm in _context.NavigationMenu
                                    join mg in _context.MenuGroup on nm.MenuGroupId equals mg.MenuGroupId into mgGroup
                                    from mg in mgGroup.DefaultIfEmpty()
                                    join mm in _context.MainMenu on nm.MainMenuId equals mm.MainMenuId into mmGroup
                                    from mm in mmGroup.DefaultIfEmpty()
                                    join sm in _context.SubMenu on nm.SubMenuId equals sm.SubMenuId into smGroup
                                    from sm in smGroup.DefaultIfEmpty()
                                    orderby nm.MenuGroupId ascending,
                                            nm.MainMenuId ascending,
                                            nm.SubMenuId ascending
                                    select new NavigationMenu
                                    {
                                        NavigationMenuId = nm.NavigationMenuId,
                                        MenuGroupId = nm.MenuGroupId,
                                        MainMenuId = nm.MainMenuId,
                                        SubMenuId = nm.SubMenuId,
                                        CreatedBy = nm.CreatedBy,
                                        CreatedDate = nm.CreatedDate,
                                        ModifiedBy = nm.ModifiedBy,
                                        ModifiedDate = nm.ModifiedDate,
                                        Active = nm.Active,
                                        MenuGroup = mg,
                                        MainMenu = mm,
                                        SubMenu = sm,
                                    }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching navigation menus: {ex.Message}");
                throw;
            }
        }

        // Fetches a list of active navigation menus.
        public async Task<List<NavigationMenu>> GetActiveAsync()
        {
            try
            {
                var result = await (from nm in _context.NavigationMenu
                                    where nm.Active == true
                                    orderby nm.MenuGroupId ascending
                                    select nm).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching active navigation menus: {ex.Message}");
                throw;
            }
        }

        // Fetches a list of distinct active menu groups from the navigation menu context.
        public async Task<List<MenuGroup>> GetActiveMenuGroupsAsync()
        {
            try
            {
                var result = await (from nm in _context.NavigationMenu
                                    join mg in _context.MenuGroup
                                         on nm.MenuGroupId equals mg.MenuGroupId
                                    where nm.Active == true
                                    select new MenuGroup
                                    {
                                        MenuGroupId = mg.MenuGroupId,
                                        MenuGroupName = mg.MenuGroupName,
                                        IconName = mg.IconName,
                                        Description = mg.Description,
                                        CreatedBy = mg.CreatedBy,
                                        CreatedDate = mg.CreatedDate,
                                        ModifiedBy = mg.ModifiedBy,
                                        ModifiedDate = mg.ModifiedDate,
                                        Active = mg.Active
                                    }).Distinct().ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching active menu groups: {ex.Message}");
                throw;
            }
        }

        // Fetches a list of distinct active main menus from the navigation menu context.
        public async Task<List<MainMenu>> GetActiveMainMenusAsync(int menuGroupId = 0)
        {
            try
            {
                var result = await (from nm in _context.NavigationMenu
                                    join mm in _context.MainMenu
                                         on nm.MainMenuId equals mm.MainMenuId
                                    where (menuGroupId == 0 || nm.MenuGroupId == menuGroupId) &&
                                         nm.Active == true
                                    select new MainMenu
                                    {
                                        MainMenuId = mm.MainMenuId,
                                        MainMenuName = mm.MainMenuName,
                                        IconName = mm.IconName,
                                        Description = mm.Description,
                                        CreatedBy = mm.CreatedBy,
                                        CreatedDate = mm.CreatedDate,
                                        ModifiedBy = mm.ModifiedBy,
                                        ModifiedDate = mm.ModifiedDate,
                                        Active = mm.Active
                                    }).Distinct().ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching active main menus: {ex.Message}");
                throw;
            }
        }

        // Fetches a list of distinct active sub menus from the navigation menu context.
        public async Task<List<SubMenu>> GetActiveSubMenusAsync(int mainMenuId = 0)
        {
            try
            {
                var result = await (from nm in _context.NavigationMenu
                                    join sm in _context.SubMenu
                                         on nm.SubMenuId equals sm.SubMenuId
                                    where (mainMenuId == 0 || nm.MainMenuId == mainMenuId) &&
                                         nm.Active == true
                                    select new SubMenu
                                    {
                                        SubMenuId = sm.SubMenuId,
                                        SubMenuName = sm.SubMenuName,
                                        IconName = sm.IconName,
                                        Description = sm.Description,
                                        CreatedBy = sm.CreatedBy,
                                        CreatedDate = sm.CreatedDate,
                                        ModifiedBy = sm.ModifiedBy,
                                        ModifiedDate = sm.ModifiedDate,
                                        Active = sm.Active
                                    }).Distinct().ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching active sub menus: {ex.Message}");
                throw;
            }
        }

        // Fetches a list of mein menu items that are unassigned to any navigation menu context in the specified menu group.
        public async Task<List<MainMenu>> GetUnassignedMainMenusAsync(int menuGroupId)
        {
            try
            {
                // Fetches a list of MainMenuIds that are assigned to other NavigationMenus in different MenuGroups.
                var mainMenuIds = await (from nm in _context.NavigationMenu
                                         where nm.MenuGroupId != menuGroupId
                                         select nm.MainMenuId).Distinct().ToListAsync();

                // Fetches a list of MainMenu items that are not part of the assigned MainMenuIds list.
                var result = await (from mm in _context.MainMenu
                                    orderby mm.MainMenuName ascending
                                    where !mainMenuIds.Contains(mm.MainMenuId) && mm.Active == true
                                    select mm).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching unassigned main menus: {ex.Message}");
                throw;
            }
        }

        // Fetches a list of SubMenus that are not assigned to any navigation menu context.
        public async Task<List<SubMenu>> GetUnassignedSubMenusAsync(int subMenuId = 0)
        {
            try
            {
                // Fetches a list of SubMenuIds that are already assigned to NavigationMenus.
                var assignedSubMenuIds = await (from nm in _context.NavigationMenu
                                                select nm.SubMenuId).Distinct().ToListAsync();

                //Fetches a list of unassigned SubMenus and explicitly includes the provided subMenuId.
                var result = await (from sm in _context.SubMenu
                                    where (subMenuId != 0 && sm.SubMenuId == subMenuId) ||
                                          (!assignedSubMenuIds.Contains(sm.SubMenuId) && sm.Active)
                                    orderby sm.SubMenuName ascending
                                    select sm).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching unassigned sub menus: {ex.Message}");
                throw;
            }
        }

        // Fetches a navigation menu hierarchy (Tree data structure).
        public async Task<List<NavigationMenuNode.MenuGroupNode>> GetHierarchyAsync()
        {
            try
            {
                var result = await (from nm in _context.NavigationMenu
                                    where nm.Active == true
                                    group nm by new
                                    {
                                        nm.MenuGroupId,
                                        nm.MenuGroup.MenuGroupName,
                                        nm.MenuGroup.IconName
                                    } into menuGroup
                                    select new NavigationMenuNode.MenuGroupNode
                                    {
                                        MenuGroupId = menuGroup.Key.MenuGroupId,
                                        MenuGroupName = menuGroup.Key.MenuGroupName,
                                        IconName = menuGroup.Key.IconName,
                                        MainMenus = (from nm in menuGroup
                                                     group nm by new
                                                     {
                                                         nm.MainMenuId,
                                                         nm.MainMenu.MainMenuName,
                                                         nm.MainMenu.IconName,
                                                         nm.MainMenu.Url
                                                     } into mainMenu
                                                     select new NavigationMenuNode.MenuGroupNode.MainMenuNode
                                                     {
                                                         MainMenuId = mainMenu.Key.MainMenuId,
                                                         MainMenuName = mainMenu.Key.MainMenuName,
                                                         IconName = mainMenu.Key.IconName,
                                                         Url = mainMenu.Key.Url,
                                                         SubMenus = (from nm in mainMenu
                                                                     select new NavigationMenuNode.MenuGroupNode.MainMenuNode.SubMenuNode
                                                                     {
                                                                         SubMenuId = (int)nm.SubMenuId,
                                                                         SubMenuName = nm.SubMenu.SubMenuName,
                                                                         IconName = nm.SubMenu.IconName,
                                                                         Url = nm.SubMenu.Url
                                                                     }).ToList()
                                                     }).ToList()
                                    }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching navigation menu hierarchy: {ex.Message}");
                throw;
            }
        }

        // Fetches a specific navigation menu by its ID.
        public async Task<NavigationMenu> GetByIdAsync(int navigationMenuId)
        {
            try
            {
                var result = await (from nm in _context.NavigationMenu
                                    where nm.NavigationMenuId == navigationMenuId
                                    select nm).FirstOrDefaultAsync();

                if (result == null)
                    throw new KeyNotFoundException($"Navigation menu with ID {navigationMenuId} was not found.");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching navigation menu with ID {navigationMenuId}: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Process Methods
        // Creates or updates a navigation menu based on NavigationMenuId.
        public async Task<NavigationMenu> UpsertAsync(NavigationMenu navigationMenu)
        {
            try
            {
                if (navigationMenu == null)
                    throw new ArgumentNullException(nameof(navigationMenu), "Navigation menu cannot be null.");

                var authState = await _authState.GetAuthenticationStateAsync();
                string userName = authState.User.FindFirst(ClaimTypes.Name)?.Value;

                // Trim and prepare data
                navigationMenu.CreatedBy = userName;
                navigationMenu.CreatedDate = navigationMenu.CreatedDate == default ? DateTime.Now : navigationMenu.CreatedDate;

                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(navigationMenu);
                if (!Validator.TryValidateObject(navigationMenu, context, validationResults, true))
                    throw new ValidationException($"{string.Join("; ", validationResults.Select(v => v.ErrorMessage))}");

                // Check if the navigation menu hierarchy already exists
                var existingSubMenu = await _context.NavigationMenu.FirstOrDefaultAsync(nm => nm.SubMenuId == navigationMenu.SubMenuId && nm.NavigationMenuId != navigationMenu.NavigationMenuId);
                if (existingSubMenu != null)
                    throw new ValidationException("The navigation menu hierarchy you are trying to add already exists in the selected menu structure.");

                // Check if the NavigationMenu already exists
                var existingNavMenu = await _context.NavigationMenu.FirstOrDefaultAsync(nm => nm.NavigationMenuId == navigationMenu.NavigationMenuId);

                if (existingNavMenu != null)
                {
                    // Update the existing record
                    existingNavMenu.MenuGroupId = navigationMenu.MenuGroupId;
                    existingNavMenu.MainMenuId = navigationMenu.MainMenuId;
                    existingNavMenu.SubMenuId = navigationMenu.SubMenuId;
                    existingNavMenu.ModifiedBy = userName;
                    existingNavMenu.ModifiedDate = DateTime.Now;
                    existingNavMenu.Active = navigationMenu.Active;

                    _context.NavigationMenu.Update(existingNavMenu);
                }
                else
                {
                    // Add the new navigation menu
                    _context.NavigationMenu.Add(navigationMenu);
                }

                await _context.SaveChangesAsync();
                return navigationMenu;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error upserting the navigation menu: {ex.Message}");
                throw;
            }
        }

        // Toggles the active status of a navigation menu by its ID.
        public async Task<NavigationMenu> ToggleActiveAsync(int navigationMenuId)
        {
            try
            {
                var navMenu = await _context.NavigationMenu.FirstOrDefaultAsync(nm => nm.NavigationMenuId == navigationMenuId);

                if (navMenu == null)
                    throw new KeyNotFoundException($"Navigation menu with ID {navigationMenuId} was not found.");

                var authState = await _authState.GetAuthenticationStateAsync();
                string userName = authState.User.FindFirst(ClaimTypes.Name)?.Value;

                navMenu.Active = !navMenu.Active;
                navMenu.ModifiedBy = userName;
                navMenu.ModifiedDate = DateTime.Now;

                _context.NavigationMenu.Update(navMenu);
                await _context.SaveChangesAsync();

                return navMenu;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error toggling Active status for navigation menu with ID {navigationMenuId}: {ex.Message}");
                throw;
            }
        }

        #endregion
    }
}