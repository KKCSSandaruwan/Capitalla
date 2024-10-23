using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;

namespace QuickAccounting.Repository.Interface
{
    public interface IPurchaseOrder
	{
       Task<List<PurchaseOrderView>> GetAll();
       Task<List<PurchaseOrderView>> PurchaseInvoiceSearch(DateTime fromDate, DateTime toDate, string voucherNo, int ledgerId);
        Task<PurchaseOrderView> PurchaseInvoiceMasterView(int id);
        Task<List<ProductView>> PurchaseInvoiceDetailsView(int id);
        Task<bool> CheckName(string name);
       Task<int> CheckNameId(string name);
        Task<int> Save(PurchaseOrder model);    
        Task<bool> Update(PurchaseOrder model);
        Task<bool> UpdatePurchaseInvoice(PurchaseOrder model);
        Task<bool> Approved(PurchaseOrder model);
        Task<PurchaseOrder> GetbyId(int id);
        Task<bool> Delete(PurchaseOrder master);
        Task<string> GetSerialNo();
        Task<List<PurchaseOrderView>> PaymentAllocations(int id);

    }
}
