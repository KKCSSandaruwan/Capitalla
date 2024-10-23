using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.HrPayroll;
using QuickAccounting.Data.HrPayrollModel;
using QuickAccounting.Data.Setting;
using QuickAccounting.Repository.Interface;
using System.Data;
using static MudBlazor.CategoryTypes;

namespace QuickAccounting.Repository.Repository
{
    public class SalaryMonthSettingService : ISalaryMonthSetting
    {
        private readonly ApplicationDbContext _context;
        private readonly DatabaseConnection _conn;
        public SalaryMonthSettingService(ApplicationDbContext context, DatabaseConnection conn)
        {
            _context = context;
            _conn = conn;
        }
        public async Task<bool> CheckName(string name)
        {
            //var checkResult = (from progm in _context.MonthlySalary
            //                   where progm.PayHeadName == name
            //                   select progm.PayHeadId).Count();
            //if (checkResult > 0)
            //{
            //    return true;
            //}
            //else
            //{
                return false;
            //}
        }

        public async Task<int> CheckNameId(string name)
        {
            var checkResult = (from progm in _context.PayHead
							   where progm.PayHeadName == name
                               select progm.PayHeadId).Count();
         //   if (checkResult > 0)
         //   {

         //       var checkAccount = (from progm in _context.PayHead
									//where progm.PayHeadName == name
         //                           select progm.PayHeadId).FirstOrDefault();
         //       return checkAccount;
         //   }
         //   else
         //   {
                return 0;
            //}
        }

        public async Task<bool> Delete(int id)
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var paraScDelete = new DynamicParameters();
                paraScDelete.Add("@MonthlySalaryId", id);
                var valueScDelete = sqlcon.Query<int>("DELETE FROM MonthlySalaryDetails where MonthlySalaryId=@MonthlySalaryId DELETE FROM MonthlySalary where MonthlySalaryId=@MonthlySalaryId", paraScDelete, null, true, 0, commandType: CommandType.Text);
            }
            return true;
        }

        public async Task<List<MonthlySalarySettingView>> GetAll(string YearMonth)
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var para = new DynamicParameters();
                para.Add("@YearMonth", YearMonth);
                var ListofPlan = sqlcon.Query<MonthlySalarySettingView>("MonthlySalarySettingsEmployeeViewAll", para, null, true, 0, commandType: CommandType.StoredProcedure).ToList();
                return ListofPlan;
            }
        }
        public async Task<List<MonthlySalary>> GetAllMonth()
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var ListofPlan = sqlcon.Query<MonthlySalary>("SELECT MonthlySalaryId,YearMonth,Narration FROM MonthlySalary", null, null, true, 0, commandType: CommandType.Text).ToList();
                return ListofPlan;
            }
        }

        public async Task<MonthlySalary> GetbyId(int id)
        {
            MonthlySalary model = await _context.MonthlySalary.FindAsync(id);
            return model;
        }

        public async Task<int> Save(MonthlySalary model)
        {
            var returnSalaryId = (from progm in _context.MonthlySalary
                                       where progm.YearMonth == model.YearMonth
                                  select progm.MonthlySalaryId).FirstOrDefault();
            //DeleteSalaryMonthSetting
            if (returnSalaryId != null)
            {
                using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
                {
                    var paraScDelete = new DynamicParameters();
                    paraScDelete.Add("@MonthlySalaryId", returnSalaryId);
                    var valueScDelete = sqlcon.Query<int>("DELETE FROM MonthlySalaryDetails where MonthlySalaryId=@MonthlySalaryId DELETE FROM MonthlySalary where MonthlySalaryId=@MonthlySalaryId", paraScDelete, null, true, 0, commandType: CommandType.Text);
                }
            }
            await _context.MonthlySalary.AddAsync(model);
            await _context.SaveChangesAsync();
            int id = model.MonthlySalaryId;
            foreach(var item in model.listOrder)
            {
                MonthlySalaryDetails details = new MonthlySalaryDetails();
                details.MonthlySalaryId = id;
                details.SalaryPackageId = item.SalaryPackageId;
                details.EmployeeId = item.EmployeeId;
                _context.MonthlySalaryDetails.Update(details);
                await _context.SaveChangesAsync();
            }
            return id;
        }

        public async Task<bool> Update(MonthlySalary model)
        {
            _context.MonthlySalary.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
