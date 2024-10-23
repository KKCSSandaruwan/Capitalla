using QuickAccounting.Data.HrPayroll;
using QuickAccounting.Data.HrPayrollModel;

namespace QuickAccounting.Repository.Interface
{
    public interface IAdvancePayment
    {
        Task<List<AdvancePaymentView>> GetAll();
        Task<IList<AdvancePaymentView>> GetAllSalaryMonth();
        Task<IList<AdvancePaymentView>> AdvancePaymentReport(DateTime fromdate, DateTime todate, int employeeid, string month);
        Task<string> GetSerialNo();
		Task<bool> CheckName(string name , int employeeid);
       Task<int> CheckNameId(string name);
        Task<int> Save(AdvancePayment model);
        Task<bool> Update(AdvancePayment model);
        Task<AdvancePayment> GetAdvanceAmount(string name, int employeeid);
        Task<AdvancePayment> GetbyId(int id);
        Task<bool> Delete(int id);
    }
}
