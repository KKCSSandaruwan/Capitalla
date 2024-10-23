using QuickAccounting.Data.Setting;
using QuickAccounting.Data.ViewModel;

namespace QuickAccounting.Repository.Interface
{
    public interface ITax
    {
        Task<List<TaxView>> GetAll();
        Task<List<TaxView>> GetActiveAll();
        Task<bool> CheckName(string name);
       Task<int> CheckNameId(string name);
        Task<int> Save(Tax model);
        Task<bool> Update(Tax model);
        Task<Tax> GetbyId(int id);
        Task<bool> Delete(int id);
    }
}
