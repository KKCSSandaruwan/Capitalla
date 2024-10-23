using QuickAccounting.Data.HrPayroll;

namespace QuickAccounting.Repository.Interface
{
    public interface ITasks
    {
        Task<List<TaskView>> GetAll();
        Task<int> Save(Tasks model);
        Task<bool> Update(Tasks model);
        Task<Tasks> GetbyId(int id);
        Task<bool> Delete(int id);
    }
}
