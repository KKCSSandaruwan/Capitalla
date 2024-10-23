using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.HrPayroll;
using QuickAccounting.Data.HrPayrollModel;
using QuickAccounting.Data.Inventory;
using QuickAccounting.Pages.HumanResorcePage.AdvancePaymentPage;
using QuickAccounting.Pages.HumanResorcePage.EmployeePage;
using QuickAccounting.Repository.Interface;
using System.Data;
using static MudBlazor.CategoryTypes;

namespace QuickAccounting.Repository.Repository
{
    public class AdvancePaymentService : IAdvancePayment
    {
        private readonly ApplicationDbContext _context;
		private readonly DatabaseConnection _conn;
		public AdvancePaymentService(ApplicationDbContext context , DatabaseConnection conn)
        {
            _context = context;
            _conn = conn;
        }
        public async Task<bool> CheckName(string name , int employeeid)
        {
            var checkResult = (from progm in _context.AdvancePayment
                               where progm.YearMonth == name && progm.EmployeeId == employeeid
                               select progm.AdvancePaymentId).Count();
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
            var checkResult = (from progm in _context.AdvancePayment
                               where progm.VoucherNo == name
                               select progm.AdvancePaymentId).Count();
            if (checkResult > 0)
            {

                var checkAccount = (from progm in _context.AdvancePayment
                                    where progm.VoucherNo == name
                                    select progm.AdvancePaymentId).FirstOrDefault();
                return checkAccount;
            }
            else
            {
                return 0;
            }
        }
		public async Task<string> GetSerialNo()
		{
			using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
			{
				var val = sqlcon.Query<string>("SELECT ISNULL(MAX(SerialNo+1),1) as VoucherNo FROM AdvancePayment", null, null, true, 0, commandType: CommandType.Text).FirstOrDefault();
				return val;
			}
		}
		public async Task<bool> Delete(int AdvancePaymentId)
        {
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand cmd = new SqlCommand("AdvancePaymentDelete", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter para = new SqlParameter();
                para = cmd.Parameters.Add("@AdvancePaymentId", SqlDbType.Int);
                para.Value = AdvancePaymentId;
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

        public async Task<List<AdvancePaymentView>> GetAll()
        {
            var result = await(from a in _context.AdvancePayment
                               join b in _context.Employee on a.EmployeeId equals b.EmployeeId
                               select new AdvancePaymentView
                               {
                                   AdvancePaymentId = a.AdvancePaymentId,
                                   VoucherNo = a.VoucherNo,
                                   Amount = a.Amount,
								   SalaryMonth = a.SalaryMonth,
                                   EmploymeeCode = b.EmployeeCode,
                                   EmploymeeName = b.EmployeeName,
                                   Narration = a.Narration
							   }).ToListAsync();
            return result;
        }

        public async Task<AdvancePayment> GetbyId(int id)
        {
            AdvancePayment model = await _context.AdvancePayment.FindAsync(id);
            return model;
        }

        public async Task<int> Save(AdvancePayment model)
        {
            await _context.AdvancePayment.AddAsync(model);
            await _context.SaveChangesAsync();
            int id = model.AdvancePaymentId;


            //CashBankPosting
			LedgerPosting cashPosting = new LedgerPosting();
            cashPosting.Date = model.Date;
			cashPosting.NepaliDate = String.Empty;
            cashPosting.LedgerId = model.LedgerId;
            cashPosting.Debit = 0;
            cashPosting.Credit = model.Amount;
			cashPosting.VoucherNo = model.VoucherNo;
			cashPosting.DetailsId = id;
			cashPosting.YearId = model.FinancialYearId;
			cashPosting.InvoiceNo = model.VoucherNo;
			cashPosting.VoucherTypeId = model.VoucherTypeId;
			cashPosting.CompanyId = 1;
			cashPosting.LongReference = model.Narration;
			cashPosting.ReferenceN = model.Narration;
			cashPosting.ChequeNo = String.Empty;
			cashPosting.ChequeDate = String.Empty;
			cashPosting.AddedDate = DateTime.UtcNow;
			_context.LedgerPosting.Add(cashPosting);
			await _context.SaveChangesAsync();

			//PostingAdvancePayment
			LedgerPosting advancePosting = new LedgerPosting();
			advancePosting.Date = model.Date;
			advancePosting.NepaliDate = String.Empty;
			advancePosting.LedgerId = 5;
			advancePosting.Debit = model.Amount;
			advancePosting.Credit = 0;
			advancePosting.VoucherNo = model.VoucherNo;
			advancePosting.DetailsId = id;
			advancePosting.YearId = model.FinancialYearId;
			advancePosting.InvoiceNo = model.VoucherNo;
			advancePosting.VoucherTypeId = model.VoucherTypeId;
			advancePosting.CompanyId = 1;
			advancePosting.LongReference = model.Narration;
			advancePosting.ReferenceN = model.Narration;
			advancePosting.ChequeNo = String.Empty;
			advancePosting.ChequeDate = String.Empty;
			advancePosting.AddedDate = DateTime.UtcNow;
			_context.LedgerPosting.Add(advancePosting);
			await _context.SaveChangesAsync();
			return id;
        }

        public async Task<bool> Update(AdvancePayment model)
        {
			_context.AdvancePayment.Update(model);
			await _context.SaveChangesAsync();

			//Delete
			using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
			{
				var paraScDelete = new DynamicParameters();
				paraScDelete.Add("@DetailsId", model.AdvancePaymentId);
				paraScDelete.Add("@VoucherTypeId", model.VoucherTypeId);
				var valueScDelete = sqlcon.Query<int>("DELETE FROM LedgerPosting where DetailsId=@DetailsId AND VoucherTypeId=@VoucherTypeId", paraScDelete, null, true, 0, commandType: CommandType.Text);
			}
			//CashBankPosting
			LedgerPosting cashPosting = new LedgerPosting();
			cashPosting.Date = model.Date;
			cashPosting.NepaliDate = String.Empty;
			cashPosting.LedgerId = model.LedgerId;
			cashPosting.Debit = 0;
			cashPosting.Credit = model.Amount;
			cashPosting.VoucherNo = model.VoucherNo;
			cashPosting.DetailsId = model.AdvancePaymentId;
			cashPosting.YearId = model.FinancialYearId;
			cashPosting.InvoiceNo = model.VoucherNo;
			cashPosting.VoucherTypeId = model.VoucherTypeId;
			cashPosting.CompanyId = 1;
			cashPosting.LongReference = model.Narration;
			cashPosting.ReferenceN = model.Narration;
			cashPosting.ChequeNo = String.Empty;
			cashPosting.ChequeDate = String.Empty;
			cashPosting.AddedDate = DateTime.UtcNow;
			_context.LedgerPosting.Add(cashPosting);
			await _context.SaveChangesAsync();

			//PostingAdvancePayment
			LedgerPosting advancePosting = new LedgerPosting();
			advancePosting.Date = model.Date;
			advancePosting.NepaliDate = String.Empty;
			advancePosting.LedgerId = 5;
			advancePosting.Debit = model.Amount;
			advancePosting.Credit = 0;
			advancePosting.VoucherNo = model.VoucherNo;
			advancePosting.DetailsId = model.AdvancePaymentId;
			advancePosting.YearId = model.FinancialYearId;
			advancePosting.InvoiceNo = model.VoucherNo;
			advancePosting.VoucherTypeId = model.VoucherTypeId;
			advancePosting.CompanyId = 1;
			advancePosting.LongReference = model.Narration;
			advancePosting.ReferenceN = model.Narration;
			advancePosting.ChequeNo = String.Empty;
			advancePosting.ChequeDate = String.Empty;
			advancePosting.AddedDate = DateTime.UtcNow;
			_context.LedgerPosting.Add(advancePosting);
			await _context.SaveChangesAsync();
            return true;
		}

        public async Task<AdvancePayment> GetAdvanceAmount(string name, int employeeid)
        {
            var result = await (from a in _context.AdvancePayment
                                where a.YearMonth == name && a.EmployeeId == employeeid
                                select new AdvancePayment
                                {
                                    Amount = a.Amount
                                }).FirstOrDefaultAsync();
            return result;
        }
        public async Task<IList<AdvancePaymentView>> GetAllSalaryMonth()
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                //var para = new DynamicParameters();
                //para.Add("@monthYear", monthYear);
                var ListofPlan = sqlcon.Query<AdvancePaymentView>("SELECT DISTINCT YearMonth From AdvancePayment", null, null, true, 0, commandType: CommandType.Text).ToList();
                return ListofPlan;
            }
        }
        public async Task<IList<AdvancePaymentView>> AdvancePaymentReport(DateTime fromdate, DateTime todate, int employeeid, string month)
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var para = new DynamicParameters();
                para.Add("@FromDate", fromdate);
                para.Add("@ToDate", todate);
                para.Add("@EmployeeId", employeeid);
                para.Add("@YearMonth", month);
                var ListofPlan = sqlcon.Query<AdvancePaymentView>("AdvancePaymentReport", para, null, true, 0, commandType: CommandType.StoredProcedure).ToList();
                return ListofPlan;
            }
        }
    }
}
