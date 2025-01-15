using QuickAccounting.Data.Setting.Navigation;

namespace QuickAccounting.Repository.Interface.Navigation
{
    public interface INavigationMenu
    {
        #region Fetch Methods

        /// <summary>
        /// Fetches a list of all navigation menus, ordered by MenuGroupId in ascending order.
        /// </summary>
        /// <returns>A list of all navigation menus.</returns>
        /// <exception cref="Exception">Thrown when there is an error fetching navigation menus.</exception>
        Task<List<NavigationMenu>> GetAllAsync();

        /// <summary>
        /// Fetches a list of active navigation menus, ordered by MenuGroupId in ascending order.
        /// </summary>
        /// <returns>A list of active navigation menus.</returns>
        /// <exception cref="Exception">Thrown when there is an error fetching active navigation menus.</exception>
        Task<List<NavigationMenu>> GetActiveAsync();

        /// <summary>
        /// Fetches a list of active menu groups, which represent distinct categories or sections in the navigation menu context.
        /// Only the menu groups that are currently active and available in the system will be returned.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result is a list of active menu groups.</returns>
        Task<List<MenuGroup>> GetActiveMenuGroupsAsync();

        /// <summary>
        /// Fetches a list of active main menus for a specified menu group. Main menus represent the top-level items in the navigation structure.
        /// If no specific menu group ID is provided, the method will return main menus for all active groups.
        /// </summary>
        /// <param name="menuGroupId">The ID of the menu group. If not provided, defaults to 0, which means all menu groups.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a list of active main menus associated with the specified menu group.</returns>
        Task<List<MainMenu>> GetActiveMainMenusAsync(int menuGroupId = 0);

        /// <summary>
        /// Fetches a list of active sub menus for a specified main menu. Sub menus are the nested items under a main menu in the navigation hierarchy.
        /// If no specific main menu ID is provided, the method will return sub menus for all active main menus.
        /// </summary>
        /// <param name="mainMenuId">The ID of the main menu. If not provided, defaults to 0, which means all main menus.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a list of active sub menus associated with the specified main menu.</returns>
        Task<List<SubMenu>> GetActiveSubMenusAsync(int mainMenuId = 0);

        /// <summary>
        /// Fetches a list of unassigned main menus for a specific menu group. Unassigned main menus are the ones that are not yet linked to any menu group.
        /// This allows for the management of menus that need to be assigned to a group for further use.
        /// </summary>
        /// <param name="menuGroupId">The ID of the menu group. This parameter is used to filter the unassigned main menus for the specified menu group.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a list of unassigned main menus for the given menu group.</returns>
        Task<List<MainMenu>> GetUnassignedMainMenusAsync(int menuGroupId);

        /// <summary>
        /// Fetches a list of sub menus that are not yet associated with any main menu, or explicitly includes a specified sub menu if its ID is provided. 
        /// This method is useful for managing sub menus that are available for future assignments to main menus.
        /// </summary>
        /// <param name="subMenuId">An optional ID of a specific sub menu to include in the result, even if it is already assigned.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result is a list of sub menus, including unassigned sub menus and the specified sub menu (if provided).
        /// </returns>
        Task<List<SubMenu>> GetUnassignedSubMenusAsync(int subMenuId = 0);

        /// <summary>
        /// fetches a list of hierarchical structure of the navigation menu.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains a list of <see cref="NavigationMenuNode.MenuGroupNode"/> representing the menu hierarchy.</returns>
        Task<List<NavigationMenuNode.MenuGroupNode>> GetHierarchyAsync();

        /// <summary>
        /// Fetches a navigation menu object for the specified navigation menu ID.
        /// </summary>
        /// <param name="navigationMenuId">The ID of the navigation menu to retrieve.</param>
        /// <returns>A navigation menu object.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no navigation menu with the given ID is found.</exception>
        /// <exception cref="Exception">Thrown when there is an error retrieving the navigation menu.</exception>
        Task<NavigationMenu> GetByIdAsync(int navigationMenuId);

        #endregion

        #region Process Methods

        /// <summary>
        /// Creates a new navigation menu or updates an existing one based on the provided NavigationMenuId.
        /// </summary>
        /// <param name="navigationMenu">The navigation menu object containing the details to upsert.</param>
        /// <returns>The upserted navigation menu object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided navigation menu is null.</exception>
        /// <exception cref="ValidationException">Thrown when the provided navigation menu fails validation.</exception>
        /// <exception cref="Exception">Thrown when there is an error while upserting the navigation menu.</exception>
        Task<NavigationMenu> UpsertAsync(NavigationMenu navigationMenu);

        /// <summary>
        /// Toggles the active status of a navigation menu based on its ID.
        /// </summary>
        /// <param name="navigationMenuId">The ID of the navigation menu whose active status is to be toggled.</param>
        /// <returns>The updated navigation menu object with the new active status.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no navigation menu with the given ID is found.</exception>
        /// <exception cref="Exception">Thrown when there is an error toggling the active status.</exception>
        Task<NavigationMenu> ToggleActiveAsync(int navigationMenuId);

        #endregion
    }
}
