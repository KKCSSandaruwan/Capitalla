using QuickAccounting.Data.HrPayroll;
using QuickAccounting.Data.HrPayrollModel;

namespace QuickAccounting.Repository.Interface
{
    public interface IEmployee
	{
        Task<List<EmployeeView>> GetAll();
       Task<bool> CheckName(string name);
       Task<int> CheckNameId(string name);
        Task<int> Save(Employee model);
        Task<bool> Update(Employee model);
        Task<Employee> GetbyId(int id);
        Task<bool> Delete(int id);
    }
}
