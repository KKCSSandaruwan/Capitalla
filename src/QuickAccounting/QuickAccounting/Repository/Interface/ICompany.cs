using QuickAccounting.Data.Setting.Corporate;

namespace QuickAccounting.Repository.Interface
{
    public interface ICompany
    {
        Task<List<Company>> GetAllAsync();
        Task<bool> Update(Company model);
        Task<Company> GetbyId(int id);
    }
}
