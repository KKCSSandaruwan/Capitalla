using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.HrPayroll;
using QuickAccounting.Data.HrPayrollModel;
using QuickAccounting.Pages.HumanResorcePage.EmployeePage;
using QuickAccounting.Repository.Interface;
using System.Data;

namespace QuickAccounting.Repository.Repository
{
    public class DesignationService : IDesignation
    {
        private readonly ApplicationDbContext _context;
        private readonly DatabaseConnection _conn;
        public DesignationService(ApplicationDbContext context , DatabaseConnection conn)
        {
            _context = context;
            _conn = conn;
        }
        public async Task<bool> CheckName(string name)
        {
            var checkResult = (from progm in _context.Designation
                               where progm.DesignationName == name
                               select progm.DesignationId).Count();
            if (checkResult > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<int> CheckNameId(string name)
        {
            var checkResult = (from progm in _context.Designation
							   where progm.DesignationName == name
                               select progm.DesignationId).Count();
            if (checkResult > 0)
            {

                var checkAccount = (from progm in _context.Designation
									where progm.DesignationName == name
                                    select progm.DesignationId).FirstOrDefault();
                return checkAccount;
            }
            else
            {
                return 0;
            }
        }

        public async Task<bool> Delete(int DesignationId)
        {
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand cmd = new SqlCommand("DesignationDelete", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter para = new SqlParameter();
                para = cmd.Parameters.Add("@DesignationId", SqlDbType.Int);
                para.Value = DesignationId;
                long rowAffacted = cmd.ExecuteNonQuery();
                if (rowAffacted > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                sqlcon.Close();
            }
        }

        public async Task<List<DesignationView>> GetAll()
        {
            var result = await(from a in _context.Designation
                               select new DesignationView
							   {
                                   DesignationId = a.DesignationId,
                                   DesignationName = a.DesignationName,
                                   LeaveDays = a.LeaveDays,
								   AdvanceAmount = a.AdvanceAmount,
                                   Narration = a.Narration
							   }).ToListAsync();
            return result;
        }

        public async Task<Designation> GetbyId(int id)
        {
			Designation model = await _context.Designation.FindAsync(id);
            return model;
        }

        public async Task<int> Save(Designation model)
        {
            await _context.Designation.AddAsync(model);
            await _context.SaveChangesAsync();
            int id = model.DesignationId;
            return id;
        }

        public async Task<bool> Update(Designation model)
        {
            _context.Designation.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
