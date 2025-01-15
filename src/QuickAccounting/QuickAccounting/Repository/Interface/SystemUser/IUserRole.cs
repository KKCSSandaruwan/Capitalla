using QuickAccounting.Data.Setting.SystemUser;

namespace QuickAccounting.Repository.Interface.SystemUser
{
    public interface IUserRole
    {
        #region Fetch Methods
        /// <summary>
        /// Fetches a list of all user roles, ordered by user role name in ascending order.
        /// </summary>
        /// <returns>A list of all user roles.</returns>
        /// <exception cref="Exception">Thrown when there is an error fetching user roles.</exception>
        Task<List<UserRole>> GetAllAsync();

        /// <summary>
        /// Fetches a list of active user roles, ordered by user role name in ascending order.
        /// </summary>
        /// <returns>A list of active user roles.</returns>
        /// <exception cref="Exception">Thrown when there is an error fetching active user roles.</exception>
        Task<List<UserRole>> GetActiveAsync();

        /// <summary>
        /// Fetches a user role object for the specified user role ID.
        /// </summary>
        /// <param name="roleId">The ID of the user role to retrieve.</param>
        /// <returns>A user role object.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no user role with the given ID is found.</exception>
        /// <exception cref="Exception">Thrown when there is an error retrieving the user role.</exception>
        Task<UserRole> GetByIdAsync(int roleId);

        #endregion

        #region Process Methods
        /// <summary>
        /// Creates a new user role or updates an existing one based on the provided RoleId.
        /// </summary>
        /// <param name="userRole">The user role object containing the details to upsert.</param>
        /// <returns>The upserted user role object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided user role is null.</exception>
        /// <exception cref="ValidationException">Thrown when the provided user role fails validation.</exception>
        /// <exception cref="Exception">Thrown when there is an error while upserting the user role.</exception>
        Task<UserRole> UpsertAsync(UserRole userRole);

        /// <summary>
        /// Toggles the active status of a user role based on its ID.
        /// </summary>
        /// <param name="roleId">The ID of the user role whose active status is to be toggled.</param>
        /// <returns>The updated user role object with the new active status.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no user role with the given ID is found.</exception>
        /// <exception cref="Exception">Thrown when there is an error toggling the active status.</exception>
        Task<UserRole> ToggleActiveAsync(int roleId);

        #endregion
    }
}
