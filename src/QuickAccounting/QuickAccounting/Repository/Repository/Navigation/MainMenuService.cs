using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Setting.Navigation;
using QuickAccounting.Repository.Interface.Navigation;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace QuickAccounting.Repository.Repository.Navigation
{
    public class MainMenuService : IMainMenu
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthenticationStateProvider _authState;

        #region Constructor
        // Constructor to initialize the service with the database context and auth
        public MainMenuService(ApplicationDbContext context, AuthenticationStateProvider authState)
        {
            try
            {
                _context = context ?? throw new ArgumentNullException(nameof(context), "Database context cannot be null.");
                _authState = authState;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing Main Menu Service: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Fetch Mathods
        // Fetches a list of all main menus, ordered by MainMenuName in ascending order.
        public async Task<List<MainMenu>> GetAllAsync()
        {
            try
            {
                var result = await (from mm in _context.MainMenu
                                    orderby mm.MainMenuName ascending
                                    select mm
                                    ).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching main menus: {ex.Message}");
                throw;
            }
        }

        // Fetches a list of active main menus, ordered by MainMenuName in ascending order.
        public async Task<List<MainMenu>> GetActiveAsync()
        {
            try
            {
                var result = await (from mm in _context.MainMenu
                                    where mm.Active == true
                                    orderby mm.MainMenuName ascending
                                    select mm
                                    ).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching active main menus: {ex.Message}");
                throw;
            }
        }

        // Fetches a object of main menu for a specified main menu id.
        public async Task<MainMenu> GetByIdAsync(int mainMenuId)
        {
            try
            {
                var result = await (from mm in _context.MainMenu
                                    where mm.MainMenuId == mainMenuId
                                    select mm
                                    ).FirstOrDefaultAsync();

                if (result == null)
                    throw new KeyNotFoundException($"Main menu with ID {mainMenuId} was not found.");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching main menu with ID {mainMenuId}: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Process Mathods
        // Creates a new main menu or updates an existing one based on MainMenuId.
        public async Task<MainMenu> UpsertAsync(MainMenu mainMenu)
        {
            try
            {
                // Null check
                if (mainMenu == null)
                    throw new ArgumentNullException(nameof(mainMenu), "Main menu cannot be null.");

                // Fetch authentication state
                var authState = await _authState.GetAuthenticationStateAsync();
                string userName = authState.User.FindFirst(ClaimTypes.Name)?.Value;

                // Trim and standardize inputs
                mainMenu.MainMenuName = mainMenu.MainMenuName.Trim();
                mainMenu.Code = mainMenu.Code?.Trim().ToUpper();
                mainMenu.Url = mainMenu.Url?.Trim();
                mainMenu.IconName = mainMenu.IconName?.Trim();
                mainMenu.Description = mainMenu.Description?.Trim();
                mainMenu.CreatedBy = userName;
                mainMenu.CreatedDate = mainMenu.CreatedDate == default ? DateTime.Now : mainMenu.CreatedDate;
                mainMenu.Active = mainMenu.Active;

                // Validate the MainMenu object
                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(mainMenu);
                if (!Validator.TryValidateObject(mainMenu, context, validationResults, true))
                    throw new ValidationException($"{string.Join("; ", validationResults.Select(v => v.ErrorMessage))}");

                // Check if a main menu with the same name already exists
                var duplicateMainMenu = await _context.MainMenu.FirstOrDefaultAsync(mm => mm.MainMenuName.ToLower() == mainMenu.MainMenuName.ToLower() && mm.MainMenuId != mainMenu.MainMenuId);
                if (duplicateMainMenu != null)
                    throw new ValidationException($"A main menu with the name '{mainMenu.MainMenuName}' already exists. Please use a unique main menu name.");

                // Check if the main menu already exists
                var existingMainMenu = await _context.MainMenu.FirstOrDefaultAsync(mm => mm.MainMenuId == mainMenu.MainMenuId);

                if (existingMainMenu != null)
                {
                    // Update existing record
                    existingMainMenu.MainMenuName = mainMenu.MainMenuName;
                    existingMainMenu.Code = mainMenu.Code;
                    existingMainMenu.Url = mainMenu.Url;
                    existingMainMenu.IconName = mainMenu.IconName;
                    existingMainMenu.Description = mainMenu.Description;
                    existingMainMenu.ModifiedBy = userName;
                    existingMainMenu.ModifiedDate = DateTime.Now;
                    existingMainMenu.Active = mainMenu.Active;

                    _context.MainMenu.Update(existingMainMenu);
                }
                else
                {
                    // Add new record
                    _context.MainMenu.Add(mainMenu);
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                return mainMenu;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error upserting the main menu: {ex.Message}");
                throw;
            }
        }

        // Toggles the active status of a main menu by its ID.
        public async Task<MainMenu> ToggleActiveAsync(int mainMenuId)
        {
            try
            {
                // Fetch the main menu by ID
                var mainMenu = await _context.MainMenu.FirstOrDefaultAsync(mm => mm.MainMenuId == mainMenuId);

                // Null check
                if (mainMenu == null)
                    throw new KeyNotFoundException($"Main menu with ID {mainMenuId} was not found.");

                // Fetch authentication state
                var authState = await _authState.GetAuthenticationStateAsync();
                string userName = authState.User.FindFirst(ClaimTypes.Name)?.Value;

                // Toggle the active status
                mainMenu.Active = !mainMenu.Active;
                mainMenu.ModifiedBy = userName;
                mainMenu.ModifiedDate = DateTime.Now;

                // Update the menu group in the database
                _context.MainMenu.Update(mainMenu);
                await _context.SaveChangesAsync();

                return mainMenu;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error toggling Active status for main menu with ID {mainMenuId}: {ex.Message}");
                throw;
            }
        }

        #endregion
    }
}
