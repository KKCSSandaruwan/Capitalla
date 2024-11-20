using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;

namespace QuickAccounting.Repository.Interface
{
    public interface IAccountLedger
    {
        Task<List<AccountLedger>> GetSalesLedgers();

        /// <summary>
        /// Asynchronously retrieves a list of available payment sources.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> that represents the asynchronous operation, containing a list of <see cref="AccountLedger"/> objects representing the payment sources.
        /// </returns>
        Task<List<AccountLedgerView>> GetPaymentSourcesAsync();

        /// <summary>
        /// Asynchronously retrieves the details of a specific account ledger based on the provided account ledger ID.
        /// </summary>
        /// <param name="accountLedgerId">The ID of the account ledger to retrieve.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation, containing an <see cref="AccountLedger"/> object with the details of the specified account ledger.
        /// </returns>
        Task<AccountLedger> GetAccountLedgersAsync(int accountLedgerId);
    }
}
