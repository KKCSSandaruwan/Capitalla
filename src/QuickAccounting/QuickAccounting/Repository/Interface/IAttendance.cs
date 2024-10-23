using QuickAccounting.Data.HrPayroll;
using QuickAccounting.Data.HrPayrollModel;
using QuickAccounting.Data.Setting;
using QuickAccounting.Data.ViewModel;

namespace QuickAccounting.Repository.Interface
{
    public interface IAttendance
	{
		Task<IList<DailyAttendanceView>> GetAll();
		decimal HolliDayChecking(DateTime date);
		decimal HolidaySettings(DateTime date);
        Task<DailyAttendanceDetails> GetAttandanceDetails(string date, int employeeid);
        bool DailyAttendanceMasterMasterIdSearch(DateTime strDate);
		IList<DailyAttendanceView> DailyAttendanceDetailsSearchGridFill();
		Task<int> Save(DailyAttendanceMaster model);
		Task<bool> Update(DailyAttendanceMaster model);
		Task<bool> Delete(int id);
	}
}
