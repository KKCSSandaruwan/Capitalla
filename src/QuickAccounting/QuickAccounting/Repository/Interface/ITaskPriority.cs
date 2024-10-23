using QuickAccounting.Data.HrPayroll;

namespace QuickAccounting.Repository.Interface
{
    public interface ITaskPriority
    {
        Task<List<TaskPriorityView>> GetAll();
       Task<bool> CheckName(string name);
       Task<int> CheckNameId(string name);
        Task<int> Save(TaskPriority model);
        Task<bool> Update(TaskPriority model);
        Task<TaskPriority> GetbyId(int id);
        Task<bool> Delete(int id);
    }
}
