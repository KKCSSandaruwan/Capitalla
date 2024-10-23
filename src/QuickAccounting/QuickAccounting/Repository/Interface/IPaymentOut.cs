using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;
using QuickAccounting.Data.ViewModel;

namespace QuickAccounting.Repository.Interface
{
    public interface IPaymentOut
    {
        Task<IList<PaymentReceiveView>> GetAll( DateTime FromDate , DateTime ToDate, string VoucherNo, string type);
        Task<PaymentReceiveView> PaymentOutView(int id);
        Task<IList<PaymentReceiveView>> PaymentOutDetailsView(int id);
        Task<IList<PaymentDetailsView>> PaymentDetailsAllView(int id);
        Task<int> Save(PaymentMaster model);
        Task<bool> ApprovedOk(PaymentMaster model);
        Task<bool> Update(PaymentMaster model);
        Task<PaymentMaster> GetbyId(int id);
        Task<string> GetSerialNo();
        Task<bool> Delete(PaymentMaster model);
    }
}
