using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;

namespace QuickAccounting.Repository.Interface
{
    public interface ISalesReturn
    {
        Task<List<SalesReturnMasterView>> GetAll();
        Task<List<SalesReturnMasterView>> SalesReturnInvoiceSearch(DateTime fromDate, DateTime toDate, string voucherNo, int ledgerId);
        Task<SalesReturnMasterView> SalesReturnInvoiceMasterView(int id);
        Task<List<ProductView>> SalesReturnInvoiceDetailsView(int id);
        Task<bool> CheckName(string name);
        Task<int> CheckNameId(string name);
        Task<int> Save(SalesReturnMaster model);
        Task<bool> Update(SalesReturnMaster model);
        Task<bool> UpdateSalesInvoice(SalesReturnMaster model);
        Task<bool> Approved(SalesReturnMaster model);
        Task<SalesReturnMaster> GetbyId(int id);
        Task<bool> Delete(SalesReturnMaster master);
        Task<string> GetSerialNo();
    }
}
