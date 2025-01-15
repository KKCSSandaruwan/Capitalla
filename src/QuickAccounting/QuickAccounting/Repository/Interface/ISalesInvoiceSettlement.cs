using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;
using QuickAccounting.Data.ViewModel;

namespace QuickAccounting.Repository.Interface
{
    public interface ISalesInvoiceSettlement
    {
        #region Fetch Mathods
        /// <summary>
        /// Asynchronously retrieves a list of customers with unsettled invoices.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> that represents the asynchronous operation, containing a list of <see cref="AccountLedgerView"/> objects for unsettled customers.
        /// </returns>
        Task<List<AccountLedgerView>> GetUnsettledCustomersAsync();

        /// <summary>
        /// Asynchronously retrieves a list of customers with settled invoices.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> that represents the asynchronous operation, containing a list of <see cref="AccountLedgerView"/> objects for settled customers.
        /// </returns>
        Task<List<AccountLedgerView>> GetSettledCustomersAsync();

        /// <summary>
        /// Asynchronously retrieves a list of unsettled invoices for the specified customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer whose unsettled invoices are to be retrieved.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> representing the asynchronous operation, containing a list of <see cref="SalesMaster"/> objects representing the customer's unsettled invoices.
        /// </returns>
        Task<List<SalesMasterView>> GetUnsettledInvoicesAsync(int customerId);

        /// <summary>
        /// Asynchronously retrieves a list of settled invoices with receipts for a specified date range and optional customer.
        /// </summary>
        /// <param name="fromDate">The start date of the period for retrieving settled invoices.</param>
        /// <param name="toDate">The end date of the period for retrieving settled invoices.</param>
        /// <param name="customerId">An optional customer ID to filter the settled invoices. Defaults to 0 for all customers.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> representing the asynchronous operation, containing a list of <see cref="SalesMasterView"/> objects representing settled invoices with receipts.
        /// </returns>
        Task<List<SalesMasterView>> GetSettledInvoicesWithReceiptsAsync(DateTime fromDate, DateTime toDate, int? customerId = 0);

        /// <summary>
        /// Asynchronously retrieves a specific settled invoice with its associated receipts, including optionally reversed receipts.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice to retrieve.</param>
        /// <param name="receiptId">An optional receipt ID to filter the receipts for the specified invoice. Defaults to 0 to include all receipts.</param>
        /// <param name="includeReversedReceipts">A flag indicating whether to include reversed receipts in the result. Defaults to false.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> representing the asynchronous operation, containing a <see cref="SalesMasterView"/> object with the invoice and its associated receipts.
        /// </returns>
        Task<SalesMasterView> GetSettledInvoiceWithReceiptsAsync(int invoiceId, int? receiptId = 0, bool? includeReversedReceipts = false);

        #endregion

        #region Process Mathods
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

        /// <summary>
        /// Asynchronously reverses multiple settled invoice receipts by updating invoice records, 
        /// modifying receipt details, creating reversal entries, and adding ledger postings for the reversed transactions.
        /// 
        /// Database Effects:
        /// 1. **SalesMaster Table**: Updates the invoice with the reversed amount and adjusts the balance due.
        /// 2. **ReceiptDetailsDup Table**: Marks the receipt as "Reversed" and updates the modified date and user.
        /// 3. **ReceiptReversal Table**: Inserts a reversal record for each receipt with relevant details and amounts.
        /// 4. **LedgerPosting Table**: Adds ledger entries to reverse both payment received and sales amounts.
        /// 
        /// If any step fails, the entire operation is rolled back.
        /// </summary>
        /// <param name="receiptsToReverse">List of receipts to reverse.</param>
        /// <returns>True if receipts are successfully reversed, false otherwise.</returns>
        Task<bool> ReverseSettledInvoiceAsync(List<ReceiptDetailsViewDup> receiptsToReverse);

        #endregion
    }
}
