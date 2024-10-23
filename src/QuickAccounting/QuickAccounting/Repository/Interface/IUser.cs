
using QuickAccounting.Data.Authentication;
using QuickAccounting.Data.ViewModel;

namespace QuickAccounting.Repository.Interface
{
    public interface IUser
    {
        Task<List<UserView>> GetAll();
        Task<List<Role>> GetAllRole();
        Task<bool> CheckName(string name);
        Task<int> CheckNameId(string name);
        Task<int> Save(UserMaster model);
        Task<bool> Update(UserMaster model);
        Task<UserMaster> GetbyId(int id);
        Task<UserMaster> UserProfile(string email);
        Task<bool> Delete(int id);
    }
}
