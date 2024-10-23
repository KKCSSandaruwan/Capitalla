using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;
using System.Threading.Tasks;

namespace QuickAccounting.Repository.Interface
{
    public interface IExpenses
    {
        Task<IList<ExpensesMasterView>> GetAll();
        Task<ExpensesMasterView> ExpensesView(int id);
        Task<IList<ExpensesDetailsView>> ExpensesDetailsView(int id);
        Task<int> Save(ExpenseMaster model);
        Task<bool> ApprovedOk(ExpenseMaster model);
        Task<bool> Update(ExpenseMaster model);
        Task<ExpenseMaster> GetbyId(int id);
        Task<string> GetSerialNo();
        Task<bool> Delete(ExpenseMaster model);
    }
}
