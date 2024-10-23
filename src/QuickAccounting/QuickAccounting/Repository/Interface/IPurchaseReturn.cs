using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;

namespace QuickAccounting.Repository.Interface
{
    public interface IPurchaseReturn
    {
        Task<List<PurchaseReturnMasterView>> GetAll();
        Task<List<PurchaseReturnMasterView>> PurchaseReturnInvoiceSearch(DateTime fromDate, DateTime toDate, string voucherNo, int ledgerId);
        Task<PurchaseReturnMasterView> PurchaseReturnInvoiceMasterView(int id);
        Task<List<ProductView>> PurchaseReturnInvoiceDetailsView(int id);
        Task<bool> CheckName(string name);
        Task<int> CheckNameId(string name);
        Task<int> Save(PurchaseReturnMaster model);
        Task<bool> Update(PurchaseReturnMaster model);
        Task<bool> UpdatePurchaseInvoice(PurchaseReturnMaster model);
        Task<bool> Approved(PurchaseReturnMaster model);
        Task<PurchaseReturnMaster> GetbyId(int id);
        Task<bool> Delete(PurchaseReturnMaster master);
        Task<string> GetSerialNo();
    }
}
