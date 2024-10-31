using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;
using QuickAccounting.Data.ViewModel;

namespace QuickAccounting.Repository.Interface
{
    public interface IPaymentIn
    {
        Task<IList<PaymentReceiveView>> GetAll(DateTime FromDate, DateTime ToDate, string VoucherNo, string type);
        Task<PaymentReceiveView> PaymentInView(int id);
        Task<IList<PaymentReceiveView>> PaymentInDetailsView(int id);
        Task<IList<ReceiptDetailsView>> ReceiptDetailsAllView(int id);
        Task<int> Save(ReceiptMaster model);
        Task<bool> ApprovedOk(ReceiptMaster model);
        Task<bool> Update(ReceiptMaster model);
        Task<ReceiptMaster> GetbyId(int id);
        Task<string> GetSerialNo();
        Task<bool> Delete(ReceiptMaster model);

        /// <summary>
        /// Asynchronously retrieves a list of customers with settled invoices.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> that represents the asynchronous operation, containing a list of <see cref="AccountLedgerView"/> objects for settled customers.
        /// </returns>
        Task<List<AccountLedgerView>> GetSettledCustomersAsync();

        /// <summary>
        /// Asynchronously retrieves a list of settled sales invoices within a specified date range and for an optional customer.
        /// </summary>
        /// <param name="fromDate">The start date of the invoice period to filter results.</param>
        /// <param name="toDate">The end date of the invoice period to filter results.</param>
        /// <param name="customerId">The optional customer ID to filter by; defaults to 0 to include all customers.</param>
        /// <returns>
        /// A <see cref="Task{List{SalesInvoiceView}}"/> representing the asynchronous operation.
        /// The task result contains a list of <see cref="SalesInvoiceView"/> objects for settled invoices that match the criteria.
        /// </returns>
        Task<List<SalesMasterView>> GetSettledSalesInvoicesAsync(DateTime fromDate, DateTime toDate, int? customerId = 0);
    }
}
