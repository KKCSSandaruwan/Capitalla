using QuickAccounting.Data.Setting;

namespace QuickAccounting.Repository.Interface
{
    public interface ICompany
    {
        Task<bool> Update(Company model);
        Task<Company> GetbyId(int id);
    }
}
