using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;

namespace QuickAccounting.Repository.Interface
{
    public interface IPurchaseInvoice
    {
       Task<List<PurchaseMasterView>> GetAll();
       Task<List<PurchaseMasterView>> PurchaseInvoiceSearch(DateTime fromDate, DateTime toDate, string voucherNo, int ledgerId);
        Task<PurchaseMasterView> PurchaseInvoiceMasterView(int id);
        Task<List<ProductView>> PurchaseInvoiceDetailsView(int id);
        Task<bool> CheckName(string name);
       Task<int> CheckNameId(string name);
        Task<int> Save(PurchaseMaster model);
        Task<bool> Update(PurchaseMaster model);
        Task<bool> UpdatePurchaseInvoice(PurchaseMaster model);
        Task<bool> Approved(PurchaseMaster model);
        Task<PurchaseMaster> GetbyId(int id);
        Task<bool> Delete(PurchaseMaster master);
        Task<string> GetSerialNo();
        Task<List<PurchaseMasterView>> PaymentAllocations(int id);

    }
}
