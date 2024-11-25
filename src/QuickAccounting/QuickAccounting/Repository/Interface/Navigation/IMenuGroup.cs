using QuickAccounting.Data.Setting.Navigation;

namespace QuickAccounting.Repository.Interface.Navigation
{
    public interface IMenuGroup
    {
        #region Fetch Mathods
        /// <summary>
        /// Fetches a list of all menu groups, ordered by MenuGroupId in ascending order.
        /// </summary>
        /// <returns>A list of all menu groups.</returns>
        /// <exception cref="Exception">Thrown when there is an error fetching menu groups.</exception>
        Task<List<MenuGroup>> GetAllAsync();

        /// <summary>
        /// Fetches a list of active menu groups, ordered by MenuGroupId in ascending order.
        /// </summary>
        /// <returns>A list of active menu groups.</returns>
        /// <exception cref="Exception">Thrown when there is an error fetching active menu groups.</exception>
        Task<List<MenuGroup>> GetActiveAsync();

        /// <summary>
        /// Fetches a menu group object for the specified menu group ID.
        /// </summary>
        /// <param name="menuGroupId">The ID of the menu group to retrieve.</param>
        /// <returns>A menu group object.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no menu group with the given ID is found.</exception>
        /// <exception cref="Exception">Thrown when there is an error retrieving the menu group.</exception>
        Task<MenuGroup> GetByIdAsync(int menuGroupId);

        #endregion

        #region Process Mathods
        /// <summary>
        /// Creates a new menu group or updates an existing one based on the provided MenuGroupId.
        /// </summary>
        /// <param name="menuGroup">The menu group object containing the details to upsert.</param>
        /// <returns>The upserted menu group object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided menu group is null.</exception>
        /// <exception cref="Exception">Thrown when there is an error while upserting the menu group.</exception>
        Task<MenuGroup> UpsertAsync(MenuGroup menuGroup);

        /// <summary>
        /// Toggles the active status of a menu group based on its ID.
        /// </summary>
        /// <param name="menuGroupId">The ID of the menu group whose active status is to be toggled.</param>
        /// <returns>The updated menu group object with the new active status.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no menu group with the given ID is found.</exception>
        /// <exception cref="Exception">Thrown when there is an error toggling the active status.</exception>
        Task<MenuGroup> ToggleActiveAsync(int menuGroupId);

        #endregion
    }
}
