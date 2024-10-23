using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;

namespace QuickAccounting.Repository.Interface
{
    public interface ISalesInvoice
    {
       Task<List<SalesMasterView>> GetAll();
       Task<List<SalesMasterView>> SalesInvoiceSearch(DateTime fromDate, DateTime toDate, string voucherNo, int ledgerId);
        Task<SalesMasterView> SalesInvoiceMasterView(int id);
        Task<List<ProductView>> SalesInvoiceDetailsView(int id);
        Task<bool> CheckName(string name);
       Task<int> CheckNameId(string name);
        Task<int> Save(SalesMaster model);
        Task<bool> Update(SalesMaster model);
		Task<bool> UpdateSalesMasterCreditNotes(int SalesMasterId);
		Task<bool> UpdateSalesInvoice(SalesMaster model);
        Task<bool> Approved(SalesMaster model);
        Task<SalesMaster> GetbyId(int id);
        Task<bool> Delete(SalesMaster master);
        Task<string> GetSerialNo();
        Task<List<SalesMasterView>> PaymentInAllocations(int id);

    }
}
