using QuickAccounting.Data.Setting.SystemUser;

namespace QuickAccounting.Repository.Interface.SystemUser
{
    public interface IUserPrivilege
    {
        #region Fetch Methods
        /// <summary>
        /// Fetches a list of all user privileges.
        /// </summary>
        /// <returns>A list of all user privileges.</returns>
        /// <exception cref="Exception">Thrown when there is an error fetching user privileges.</exception>
        Task<List<UserPrivilege>> GetAllAsync();

        /// <summary>
        /// Fetches a list of active user privileges.
        /// </summary>
        /// <returns>A list of active user privileges.</returns>
        /// <exception cref="Exception">Thrown when there is an error fetching active user privileges.</exception>
        Task<List<UserPrivilege>> GetActiveAsync();

        /// <summary>
        /// Fetches a user privilege object for a specified user privilege ID.
        /// </summary>
        /// <param name="userPrivilegeId">The ID of the user privilege to retrieve.</param>
        /// <returns>A user privilege object.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no user privilege with the given ID is found.</exception>
        /// <exception cref="Exception">Thrown when there is an error fetching the user privilege.</exception>
        Task<UserPrivilege> GetByIdAsync(int userPrivilegeId);

        /// <summary>
        /// Fetches a list of user privileges along with associated navigation menu data
        /// for a specified user role, with optional filters for menu group, main menu, and sub-menu.
        /// </summary>
        /// <param name="userRoleId">The user role ID to fetch privileges for (default is 0, which fetches all roles).</param>
        /// <param name="menuGroupId">Filter by menu group ID (default is 0, no filter applied).</param>
        /// <param name="mainMenuId">Filter by main menu ID (default is 0, no filter applied).</param>
        /// <param name="subMenuId">Filter by sub-menu ID (default is 0, no filter applied).</param>
        /// <returns>A list of user privileges mapped to navigation menus with their respective permissions.</returns>
        Task<List<UserPrivilegeView>> GetRoleMappedNavigationAsync(int userRoleId = 0, int menuGroupId = 0, int mainMenuId = 0, int subMenuId = 0);

        #endregion

        #region Process Methods
        /// <summary>
        /// Creates or updates a user privilege based on its ID. If the ID is 0, it inserts a new user privilege; otherwise, updates an existing one.
        /// </summary>
        /// <param name="userPrivilege">The user privilege object to create or update.</param>
        /// <returns>The created or updated user privilege object.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the provided user privilege is null.</exception>
        /// <exception cref="ValidationException">Thrown if validation fails on the provided user privilege object.</exception>
        /// <exception cref="InvalidOperationException">Thrown if attempting to update a non-existent user privilege.</exception>
        Task<UserPrivilege> UpsertAsync(UserPrivilege userPrivilege);

        /// <summary>
        /// Toggles the active status of a user privilege by its ID. If the user privilege doesn't exist, it will be inserted; otherwise, updated.
        /// </summary>
        /// <param name="userPrivilege">The user privilege object containing the active status to toggle.</param>
        /// <returns>The updated user privilege object with the toggled active status.</returns>
        Task<UserPrivilege> ToggleActiveAsync(UserPrivilege userPrivilege);

        #endregion
    }
}
