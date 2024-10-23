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
    }
}
