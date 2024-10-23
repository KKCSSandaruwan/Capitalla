using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;
using QuickAccounting.Data.ViewModel;
using QuickAccounting.Repository.Interface;
using System.Data;

namespace QuickAccounting.Repository.Repository
{
    public class CustomerSupplierService : ICustomerSupplier
    {
        private readonly ApplicationDbContext _context;
        private readonly DatabaseConnection _conn;
        public CustomerSupplierService(ApplicationDbContext context, DatabaseConnection conn)
        {
            _context = context;
            _conn = conn;
        }
        public async Task<bool> CheckName(string name)
        {
            var checkResult = (from progm in _context.AccountLedger
                               where progm.LedgerName == name && progm.Active == true
							   select progm.LedgerId).Count();
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
            var checkResult = (from progm in _context.AccountLedger
                               where progm.LedgerName == name
                               select progm.LedgerId).Count();
            if (checkResult > 0)
            {

                var checkAccount = (from progm in _context.AccountLedger
                                    where progm.LedgerName == name
                                    select progm.LedgerId).FirstOrDefault();
                return checkAccount;
            }
            else
            {
                return 0;
            }
        }

        public async Task<bool> Delete(int LedgerId)
        {
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand cmd = new SqlCommand("AccountLedgerDelete", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter para = new SqlParameter();
                para = cmd.Parameters.Add("@LedgerId", SqlDbType.Int);
                para.Value = LedgerId;
                long rowAffacted = cmd.ExecuteNonQuery();
                //transaction.Commit();
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

        public async Task<List<AccountLedgerView>> GetAll(int id)
        {
            var result = await(from a in _context.AccountLedger
                               join b in _context.AccountGroup on a.AccountGroupId equals b.AccountGroupId
                               where a.AccountGroupId == id && a.Active == true
                               select new AccountLedgerView
                               {
                                   LedgerId = a.LedgerId,
                                   LedgerName = a.LedgerName,
                                   LedgerCode = a.LedgerCode,
                                   Phone = a.Phone,
                                   Email = a.Email,
                                   Address = a.Address,
                                   IsDefault = b.IsDefault
                               }).ToListAsync();
            return result;
        }
        public async Task<List<AccountLedgerView>> GetCashOrBank()
        {
            var result = await (from a in _context.AccountLedger
                                join b in _context.AccountGroup on a.AccountGroupId equals b.AccountGroupId
                                where a.AccountGroupId == 27 || a.AccountGroupId == 28
                                select new AccountLedgerView
                                {
                                    LedgerId = a.LedgerId,
                                    LedgerName = a.LedgerName,
                                    LedgerCode = a.LedgerCode,
                                    Phone = a.Phone,
                                    Email = a.Email,
                                    Address = a.Address,
                                    IsDefault = b.IsDefault
                                }).ToListAsync();
            return result;
        }
        public async Task<List<AccountLedgerView>> GetAll()
        {
            var result = await (from a in _context.AccountLedger
                                join b in _context.AccountGroup on a.AccountGroupId equals b.AccountGroupId
                                select new AccountLedgerView
                                {
                                    LedgerId = a.LedgerId,
                                    LedgerName = a.LedgerName,
                                    LedgerCode = a.LedgerCode,
                                    AccountGroupName = b.AccountGroupName,
                                    Phone = a.Phone,
                                    Email = a.Email,
                                    Address = a.Address,
                                    IsDefault = b.IsDefault
                                }).ToListAsync();
            return result;
        }

        public async Task<AccountLedger> GetbyId(int id)
        {
            AccountLedger model = await _context.AccountLedger.FindAsync(id);
            return model;
        }
        public async Task<int> Save(AccountLedger model)
        {
			model.Active = true;
			await _context.AccountLedger.AddAsync(model);
            await _context.SaveChangesAsync();
            int id = model.LedgerId;
            if(model.OpeningBalance > 0)
            {
                //PostingOpeningBalance
                //Customer
                LedgerPosting ledgerPosting = new LedgerPosting();
                ledgerPosting.Date = DateTime.Now;
                ledgerPosting.NepaliDate = String.Empty;
                ledgerPosting.LedgerId = id;
                if(model.CrOrDr == "Dr")
                {
                    ledgerPosting.Debit = model.OpeningBalance;
                    ledgerPosting.Credit = 0;
                }
                else
                {
                    ledgerPosting.Credit = model.OpeningBalance;
                    ledgerPosting.Debit = 0;
                }
                ledgerPosting.VoucherNo = id.ToString();
                ledgerPosting.DetailsId = id;
                ledgerPosting.YearId = 1;
                ledgerPosting.InvoiceNo = id.ToString();
                ledgerPosting.VoucherTypeId = 1;
                ledgerPosting.CompanyId = model.CompanyId;
                ledgerPosting.LongReference = String.Empty;
                ledgerPosting.ReferenceN = String.Empty;
                ledgerPosting.ChequeNo = String.Empty;
                ledgerPosting.ChequeDate = String.Empty;
                ledgerPosting.AddedDate = DateTime.Now;
                _context.LedgerPosting.Add(ledgerPosting);
                await _context.SaveChangesAsync();
            }
            return id;
        }

        public async Task<bool> Update(AccountLedger model)
        {
            _context.AccountLedger.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IList<AccountLedgerView>> GetAllExpenses()
        {
            var result = await (from a in _context.AccountLedger
                                join b in _context.AccountGroup on a.AccountGroupId equals b.AccountGroupId
                                where a.AccountGroupId == 13 || a.AccountGroupId == 15
                                select new AccountLedgerView
                                {
                                    LedgerId = a.LedgerId,
                                    LedgerName = a.LedgerName,
                                    LedgerCode = a.LedgerCode,
                                    Phone = a.Phone,
                                    Email = a.Email,
                                    Address = a.Address,
                                    IsDefault = b.IsDefault
                                }).ToListAsync();
            return result;
        }
        public async Task<string> GetSerialNo()
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var val = sqlcon.Query<string>("SELECT ISNULL(MAX(LedgerCode+1),1) as LedgerCode FROM AccountLedger", null, null, true, 0, commandType: CommandType.Text).FirstOrDefault();
                return val;
            }
        }
    }
}
