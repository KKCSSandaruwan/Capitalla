using QuickAccounting.Data.Setting.Navigation;

namespace QuickAccounting.Repository.Interface.Navigation
{
    public interface IMainMenu
    {
        #region Fetch Mathods
        /// <summary>
        /// Fetches a list of all main menus, ordered by MainMenuName in ascending order.
        /// </summary>
        /// <returns>A list of all main menus.</returns>
        /// <exception cref="Exception">Thrown when there is an error fetching main menus.</exception>
        Task<List<MainMenu>> GetAllAsync();

        /// <summary>
        /// Fetches a list of active main menus, ordered by MainMenuName in ascending order.
        /// </summary>
        /// <returns>A list of active main menus.</returns>
        /// <exception cref="Exception">Thrown when there is an error fetching active main menus.</exception>
        Task<List<MainMenu>> GetActiveAsync();

        /// <summary>
        /// Fetches a main menu object for the specified main menu ID.
        /// </summary>
        /// <param name="mainMenuId">The ID of the main menu to retrieve.</param>
        /// <returns>A main menu object.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no main menu with the given ID is found.</exception>
        /// <exception cref="Exception">Thrown when there is an error retrieving the main menu.</exception>
        Task<MainMenu> GetByIdAsync(int mainMenuId);

        #endregion

        #region Process Mathods
        /// <summary>
        /// Creates a new main menu or updates an existing one based on the provided MenuGroupId.
        /// </summary>
        /// <param name="mainMenu">The main menu object containing the details to upsert.</param>
        /// <returns>The upserted main menu object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided main menu is null.</exception>
        /// <exception cref="Exception">Thrown when there is an error while upserting the main menu.</exception>
        Task<MainMenu> UpsertAsync(MainMenu mainMenu);

        /// <summary>
        /// Toggles the active status of a main menu based on its ID.
        /// </summary>
        /// <param name="mainMenuId">The ID of the main menu whose active status is to be toggled.</param>
        /// <returns>The updated main menu object with the new active status.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no main menu with the given ID is found.</exception>
        /// <exception cref="Exception">Thrown when there is an error toggling the active status.</exception>
        Task<MainMenu> ToggleActiveAsync(int mainMenuId);

        #endregion
    }
}
