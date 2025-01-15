using QuickAccounting.Data.Setting.SystemUser;
using QuickAccounting.Data.ViewModel.SystemUser;

namespace QuickAccounting.Repository.Interface.SystemUser
{
    public interface IUserProfile
    {
        #region Fetch Methods
        /// <summary>
        /// Fetches a list of all user profiles, ordered by user name in ascending order.
        /// </summary>
        /// <returns>A list of all user profiles.</returns>
        /// <exception cref="Exception">Thrown when there is an error fetching user profiles.</exception>
        Task<List<UserProfileView>> GetAllAsync();

        /// <summary>
        /// Fetches a list of active user profiles, ordered by user name in ascending order.
        /// </summary>
        /// <returns>A list of active user profiles.</returns>
        /// <exception cref="Exception">Thrown when there is an error fetching active user profiles.</exception>
        Task<List<UserProfile>> GetActiveAsync();

        /// <summary>
        /// Fetches a user profile object for the specified user profile ID.
        /// </summary>
        /// <param name="userId">The ID of the user profile to retrieve.</param>
        /// <returns>A user profile object.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no user profile with the given ID is found.</exception>
        /// <exception cref="Exception">Thrown when there is an error retrieving the user profile.</exception>
        Task<UserProfile> GetByIdAsync(int userId);

        #endregion

        #region Process Methods
        /// <summary>
        /// Creates a new user profile or updates an existing one based on the provided UserId.
        /// </summary>
        /// <param name="userProfile">The user profile object containing the details to upsert.</param>
        /// <returns>The upserted user profile object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided user profile is null.</exception>
        /// <exception cref="ValidationException">Thrown when the provided user profile fails validation.</exception>
        /// <exception cref="Exception">Thrown when there is an error while upserting the user profile.</exception>
        Task<UserProfile> UpsertAsync(UserProfile userProfile);

        /// <summary>
        /// Toggles the active status of a user profile based on its ID.
        /// </summary>
        /// <param name="userId">The ID of the user profile whose active status is to be toggled.</param>
        /// <returns>The updated user profile object with the new active status.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no user profile with the given ID is found.</exception>
        /// <exception cref="Exception">Thrown when there is an error toggling the active status.</exception>
        Task<UserProfile> ToggleActiveAsync(int userId);

        #endregion
    }
}
