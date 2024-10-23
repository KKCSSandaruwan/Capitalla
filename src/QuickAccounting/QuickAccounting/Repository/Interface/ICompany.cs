using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;
using QuickAccounting.Data.ViewModel;

namespace QuickAccounting.Repository.Interface
{
    public interface ICompany
    {
        Task<bool> Update(Company model);
        Task<Company> GetbyId(int id);
    }
}
