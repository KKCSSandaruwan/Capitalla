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
    }
}
