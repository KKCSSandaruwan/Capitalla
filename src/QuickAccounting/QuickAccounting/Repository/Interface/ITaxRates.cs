using QuickAccounting.Data.HrPayroll;
using QuickAccounting.Data.Setting;
using QuickAccounting.Data.ViewModel;

namespace QuickAccounting.Repository.Interface
{
    public interface ITaxRates
    {
        Task<List<TaxRatesView>> GetAll();
        Task<int> Save(TaxRates model);
        Task<bool> Update(TaxRates model);
        Task<TaxRates> GetbyId(int id);
        Task<bool> Delete(int id);
    }
}
