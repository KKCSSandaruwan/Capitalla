using QuickAccounting.Data.Setting.Corporate;

namespace QuickAccounting.Repository.Interface.Corporate
{
    public interface ICompanyDup
    {
        #region Fetch Methods
        /// <summary>
        /// Fetches a list of all companies, ordered by CompanyName in ascending order.
        /// </summary>
        /// <returns>A list of all companies.</returns>
        /// <exception cref="Exception">Thrown when there is an error fetching companies.</exception>
        Task<List<CompanyDup>> GetAllAsync();

        /// <summary>
        /// Fetches a list of active companies, ordered by CompanyName in ascending order.
        /// </summary>
        /// <returns>A list of active companies.</returns>
        /// <exception cref="Exception">Thrown when there is an error fetching active companies.</exception>
        Task<List<CompanyDup>> GetActiveAsync();

        /// <summary>
        /// Fetches a company object for the specified CompanyId.
        /// </summary>
        /// <param name="companyId">The ID of the company to retrieve.</param>
        /// <returns>A company object.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no company with the given ID is found.</exception>
        /// <exception cref="Exception">Thrown when there is an error retrieving the company.</exception>
        Task<CompanyDup> GetByIdAsync(int companyId);

        #endregion

        #region Process Methods
        /// <summary>
        /// Creates a new company or updates an existing one based on the provided CompanyId.
        /// </summary>
        /// <param name="company">The company object containing the details to upsert.</param>
        /// <returns>The upserted company object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided company is null.</exception>
        /// <exception cref="ValidationException">Thrown when the provided company fails validation.</exception>
        /// <exception cref="Exception">Thrown when there is an error while upserting the company.</exception>
        Task<CompanyDup> UpsertAsync(CompanyDup company);

        /// <summary>
        /// Toggles the active status of a company based on its ID.
        /// </summary>
        /// <param name="companyId">The ID of the company whose active status is to be toggled.</param>
        /// <returns>The updated company object with the new active status.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no company with the given ID is found.</exception>
        /// <exception cref="Exception">Thrown when there is an error toggling the active status.</exception>
        Task<CompanyDup> ToggleActiveAsync(int companyId);

        #endregion
    }
}
