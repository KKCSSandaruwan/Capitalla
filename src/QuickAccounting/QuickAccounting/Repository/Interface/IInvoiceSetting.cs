using QuickAccounting.Data.Setting;
using QuickAccounting.Data.ViewModel;

namespace QuickAccounting.Repository.Interface
{
    public interface IInvoiceSetting
    {
        Task<List<InvoiceSetting>> GetAll();
       Task<int> CheckNameId(string name);
        Task<bool> Update(InvoiceSetting model);
        Task<InvoiceSetting> GetbyId(int id);
        Task<bool> Delete(int id);
    }
}
