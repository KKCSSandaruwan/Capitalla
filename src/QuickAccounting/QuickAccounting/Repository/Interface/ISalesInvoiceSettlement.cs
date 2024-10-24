using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;

namespace QuickAccounting.Repository.Interface
{
    public interface ISalesInvoiceSettlement
    {
        /// <summary>
        /// Asynchronously retrieves a list of customers with unsettled invoices.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> that represents the asynchronous operation, containing a list of <see cref="AccountLedgerView"/> objects for unsettled customers.
        /// </returns>
        Task<List<AccountLedgerView>> GetUnsettledCustomersAsync();

        /// <summary>
        /// Asynchronously retrieves a list of unsettled invoices for the specified customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer whose unsettled invoices are to be retrieved.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> representing the asynchronous operation, containing a list of <see cref="SalesMaster"/> objects representing the customer's unsettled invoices.
        /// </returns>
        Task<List<SalesMaster>> GetUnsettledInvoicesByCustomerAsync(int customerId);
    }
}
