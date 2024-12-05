using QuickAccounting.Data.Setting.Navigation;

namespace QuickAccounting.Repository.Interface.Navigation
{
    public interface ISubMenu
    {
        #region Fetch Methods

        /// <summary>
        /// Fetches a list of all sub menus, ordered by SubMenuName in ascending order.
        /// </summary>
        /// <returns>A list of all sub menus.</returns>
        /// <exception cref="Exception">Thrown when there is an error fetching sub menus.</exception>
        Task<List<SubMenu>> GetAllAsync();

        /// <summary>
        /// Fetches a list of active sub menus, ordered by SubMenuName in ascending order.
        /// </summary>
        /// <returns>A list of active sub menus.</returns>
        /// <exception cref="Exception">Thrown when there is an error fetching active sub menus.</exception>
        Task<List<SubMenu>> GetActiveAsync();

        /// <summary>
        /// Fetches a list of sub menus that are not currently assigned to any navigation menu.
        /// </summary>
        /// <returns>A list of unassigned sub menus.</returns>
        /// <exception cref="Exception">Thrown when there is an error fetching the unassigned sub menus.</exception>
        Task<List<SubMenu>> GetUnassignedAsync();

        /// <summary>
        /// Fetches a sub menu object for the specified sub menu ID.
        /// </summary>
        /// <param name="subMenuId">The ID of the sub menu to retrieve.</param>
        /// <returns>A sub menu object.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no sub menu with the given ID is found.</exception>
        /// <exception cref="Exception">Thrown when there is an error retrieving the sub menu.</exception>
        Task<SubMenu> GetByIdAsync(int subMenuId);

        #endregion

        #region Process Methods

        /// <summary>
        /// Creates a new sub menu or updates an existing one based on the provided SubMenuId.
        /// </summary>
        /// <param name="subMenu">The sub menu object containing the details to upsert.</param>
        /// <returns>The upserted sub menu object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided sub menu is null.</exception>
        /// <exception cref="ValidationException">Thrown when the provided sub menu fails validation.</exception>
        /// <exception cref="Exception">Thrown when there is an error while upserting the sub menu.</exception>
        Task<SubMenu> UpsertAsync(SubMenu subMenu);

        /// <summary>
        /// Toggles the active status of a sub menu based on its ID.
        /// </summary>
        /// <param name="subMenuId">The ID of the sub menu whose active status is to be toggled.</param>
        /// <returns>The updated sub menu object with the new active status.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no sub menu with the given ID is found.</exception>
        /// <exception cref="Exception">Thrown when there is an error toggling the active status.</exception>
        Task<SubMenu> ToggleActiveAsync(int subMenuId);

        #endregion
    }
}
