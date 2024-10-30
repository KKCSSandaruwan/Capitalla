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
        Task<List<SalesMasterView>> GetUnsettledInvoicesByCustomerAsync(int customerId);

        /// <summary>
        /// Asynchronously settles multiple sales invoices by updating invoice records, creating payment entries, 
        /// and adding ledger postings to the database.
        /// 
        /// Database Effects:
        /// 1. **SalesMaster Table**: Updates each invoice with the paid amount and balance due.
        /// 2. **ReceiptMasterDup Table**: Inserts a new payment record for the settled invoices.
        /// 3. **ReceiptDetailsDup Table**: Inserts payment details for each invoice.
        /// 4. **LedgerPosting Table**: Adds ledger entries for both payment and purchase transactions.
        /// 
        /// If any step fails, the entire operation is rolled back.
        /// </summary>
        /// <param name="invoicesToSettle">List of invoices to settle.</param>
        /// <returns>True if invoices are successfully settled, false otherwise.</returns>
        Task<bool> SettleInvoiceAsync(List<SalesMasterView> invoicesToSettle);
    }
}
