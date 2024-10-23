using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.BudgetModel;
using QuickAccounting.Repository.Interface;
using System.Data;

namespace QuickAccounting.Repository.Repository
{
    public class BudgetService : IBudget
    {
        private readonly ApplicationDbContext _context;
        private readonly DatabaseConnection _conn;
        public BudgetService(ApplicationDbContext context, DatabaseConnection conn)
        {
            _context = context;
            _conn = conn;
        }
        public async Task<bool> Delete(int BudgetMasterId)
        {
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            try
            {

                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand cmd = new SqlCommand("BudgetDelete", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter para = new SqlParameter();
                para = cmd.Parameters.Add("@BudgetMasterId", SqlDbType.Int);
                para.Value = BudgetMasterId;
                int rowAffacted = cmd.ExecuteNonQuery();
                if (rowAffacted > 0)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                sqlcon.Close();
            }
        }

        public async Task<IList<BudgetMaster>> GetAll(DateTime FromDate , DateTime ToDate , string BudgetName)
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var para = new DynamicParameters();
                para.Add("@FromDate", FromDate);
                para.Add("@ToDate", ToDate);
                para.Add("@BudgetName", BudgetName);
                var ListofPlan = sqlcon.Query<BudgetMaster>("BudgetGetAll", para, null, true, 0, commandType: CommandType.StoredProcedure).ToList();
                return ListofPlan;
            }
        }
		public async Task<IList<BudgetVariance>> GetBudgetVariance(int BudgetMasterId)
		{
			using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
			{
				var para = new DynamicParameters();
				para.Add("@BudgetMasterId", BudgetMasterId);
				var ListofPlan = sqlcon.Query<BudgetVariance>("BudgetVariance", para, null, true, 0, commandType: CommandType.StoredProcedure).ToList();
				return ListofPlan;
			}
		}
		public async Task<IList<BudgetDetailsView>> BudgetDetailsView(int id)
        {
            var result = await (from a in _context.BudgetDetails
                                join b in _context.AccountLedger on a.LedgerId equals b.LedgerId
                                where a.BudgetMasterId == id
                                select new BudgetDetailsView
                                {
                                    Id = id+1,
                                    BudgetDetailsId = a.BudgetDetailsId,
                                    BudgetMasterId = a.BudgetMasterId,
                                    Credit = a.Credit,
                                    Debit = a.Debit,
                                    LedgerId = a.LedgerId,
									LedgerName = b.LedgerName
                                }).ToListAsync();
            return result;
        }
        public async Task<IList<BudgetMaster>> GetAllBudget()
        {
            var result = await (from a in _context.BudgetMaster
                                select new BudgetMaster
                                {
                                    BudgetMasterId = a.BudgetMasterId,
                                    BudgetName = a.BudgetName
                                }).ToListAsync();
            return result;
        }
        public async Task<BudgetMaster> GetbyId(int id)
        {
			BudgetMaster model = await _context.BudgetMaster.FindAsync(id);
            return model;
        }

        public async Task<int> Save(BudgetMaster model)
        {
            _context.BudgetMaster.Add(model);
            await _context.SaveChangesAsync();
            int id = model.BudgetMasterId;



            //PaymentDetails table
            foreach (var item in model.listOrder)
            {
                //AddJournalDetails
                BudgetDetails details = new BudgetDetails();
                if (item.LedgerId > 0)
                {
                    details.BudgetMasterId = id;
                    details.LedgerId = item.LedgerId;
                    details.Debit = item.Debit;
                    details.Credit = item.Credit;
                    _context.BudgetDetails.Add(details);
                    await _context.SaveChangesAsync();
                    int intPurchaseDId = details.BudgetDetailsId;

                }
            }
            return id;
        }

        public async Task<bool> ApprovedOk(BudgetMaster model)
        {
            try
            {
                _context.BudgetMaster.Update(model);
                await _context.SaveChangesAsync();
                //BudgetDetails table
                foreach (var item in model.listOrder)
                {
                    //BudgetDetails
                    BudgetDetails details = new BudgetDetails();
                    if (item.LedgerId > 0)
                    {
                        details.BudgetMasterId = model.BudgetMasterId;
                        details.BudgetDetailsId = item.BudgetDetailsId;
						details.LedgerId = item.LedgerId;
						details.Debit = item.Debit;
						details.Credit = item.Credit;
						_context.BudgetDetails.Update(details);
                        await _context.SaveChangesAsync();
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Update(BudgetMaster model)
        {
            try
            {
                _context.BudgetMaster.Update(model);
                await _context.SaveChangesAsync();

                //BudgetDetails table
                foreach (var item in model.listOrder)
                {
					//BudgetDetails
					BudgetDetails details = new BudgetDetails();
                    if (item.BudgetDetailsId == 0)
                    {
                        details.BudgetMasterId = model.BudgetMasterId;
                        details.LedgerId = item.LedgerId;
                        details.Debit = item.Debit;
                        details.Credit = item.Credit;
                        _context.BudgetDetails.Add(details);
                        await _context.SaveChangesAsync();

                        
                    }
                    else
                    {
                        details.BudgetDetailsId = item.BudgetDetailsId;
						details.BudgetMasterId = model.BudgetMasterId;
						details.LedgerId = item.LedgerId;
						details.Debit = item.Debit;
						details.Credit = item.Credit;
						_context.BudgetDetails.Update(details);
                        await _context.SaveChangesAsync();
                    }
                }


                        foreach (var deleteitem in model.listDelete)
                {
                    BudgetDetails x = _context.BudgetDetails.Find(deleteitem.BudgetDetailsId);
                    _context.BudgetDetails.Remove(x);
                   await _context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
