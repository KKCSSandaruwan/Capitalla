using QuickAccounting.Data.Setting;
using QuickAccounting.Data.ViewModel;

namespace QuickAccounting.Repository.Interface
{
    public interface IBrand
    {
        Task<List<BrandView>> GetAll();
       Task<bool> CheckName(string name);
       Task<int> CheckNameId(string name);
        Task<int> Save(Brand model);
        Task<bool> Update(Brand model);
        Task<Brand> GetbyId(int id);
        Task<bool> Delete(int id);
    }
}
