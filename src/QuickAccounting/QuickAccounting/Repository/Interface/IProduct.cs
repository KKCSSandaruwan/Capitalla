using QuickAccounting.Data.Setting;
using QuickAccounting.Data.ViewModel;

namespace QuickAccounting.Repository.Interface
{
    public interface IProduct
    {
        Task<List<ProductView>> GetAll();
        Task<List<ProductView>> ProductWithStock();
        Task<List<ProductView>> ProductWithCategory(int GroupId);
        Task<bool> CheckName(string name);
       Task<int> CheckNameId(string name);
        Task<int> Save(Product model);
        Task<bool> Update(Product model);
        Task<Product> GetbyId(int id);
        Task<bool> Delete(int id);
        string GetProductNo();
    }
}
