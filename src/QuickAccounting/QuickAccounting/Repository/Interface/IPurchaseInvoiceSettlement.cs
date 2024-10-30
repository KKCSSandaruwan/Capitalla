using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;

namespace QuickAccounting.Repository.Interface
{
    public interface IPurchaseInvoiceSettlement
    {
        /// <summary>
        /// Asynchronously retrieves a list of suppliers with unsettled invoices.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> that represents the asynchronous operation, containing a list of <see cref="AccountLedgerView"/> objects for unsettled suppliers.
        /// </returns>
        Task<List<AccountLedgerView>> GetUnsettledSuppliersAsync();

        /// <summary>
        /// Asynchronously retrieves a list of unsettled invoices for the specified supplier.
        /// </summary>
        /// <param name="supplierId">The ID of the supplier whose unsettled invoices are to be retrieved.</param>
        /// <returns>
        /// A <see cref="Task"/> that represents the asynchronous operation, containing a list of <see cref="PurchaseMaster"/> objects for the supplier's unsettled invoices.
        /// </returns>
        Task<List<PurchaseMaster>> GetUnsettledInvoicesBySupplierAsync(int supplierId);

        /// <summary>
        /// Asynchronously settles multiple invoices by updating invoice records, creating payment entries, 
        /// and adding ledger postings to the database.
        /// 
        /// Database Effects:
        /// 1. **PurchaseMaster Table**: Updates each invoice with the paid amount and balance due.
        /// 2. **PaymentMasterDup Table**: Inserts a new payment record for the settled invoices.
        /// 3. **PaymentDetailsDup Table**: Inserts payment details for each invoice.
        /// 4. **LedgerPosting Table**: Adds ledger entries for both payment and purchase transactions.
        /// 
        /// If any step fails, the entire operation is rolled back.
        /// </summary>
        /// <param name="invoicesToSettle">List of invoices to settle.</param>
        /// <returns>True if invoices are successfully settled, false otherwise.</returns>
        Task<bool> SettleInvoiceAsync(List<PurchaseMaster> invoicesToSettle);
    }
}
