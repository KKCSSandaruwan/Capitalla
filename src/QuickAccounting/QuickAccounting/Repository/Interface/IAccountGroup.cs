using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;
using QuickAccounting.Data.ViewModel;

namespace QuickAccounting.Repository.Interface
{
    public interface IAccountGroup
    {
        Task<List<AccountGroupView>> GetAll();
       Task<bool> CheckName(string name);
       Task<int> CheckNameId(string name);
        Task<int> Save(AccountGroup model);
        Task<bool> Update(AccountGroup model);
        Task<AccountGroup> GetbyId(int id);
        Task<bool> Delete(int id);
    }
}
