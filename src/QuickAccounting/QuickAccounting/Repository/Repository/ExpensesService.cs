using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;
using QuickAccounting.Repository.Interface;
using System.Data;

namespace QuickAccounting.Repository.Repository
{
    public class ExpensesService : IExpenses
    {
        private readonly ApplicationDbContext _context;
        private readonly DatabaseConnection _conn;
        public ExpensesService(ApplicationDbContext context, DatabaseConnection conn)
        {
            _context = context;
            _conn = conn;
        }
        public async Task<bool> Delete(ExpenseMaster master)
        {
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            try
            {

                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand cmd = new SqlCommand("ExpenseInvoiceDelete", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter para = new SqlParameter();
                para = cmd.Parameters.Add("@ExpensiveMasterId", SqlDbType.Int);
                para.Value = master.ExpensiveMasterId;
                para = cmd.Parameters.Add("@VoucherTypeId", SqlDbType.Int);
                para.Value = master.VoucherTypeId;
                para = cmd.Parameters.Add("@VoucherNo", SqlDbType.NVarChar);
                para.Value = master.VoucherNo;
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

        public async Task<IList<ExpensesMasterView>> GetAll()
        {
            var result = await (from a in _context.ExpenseMaster
                                join b in _context.AccountLedger on a.LedgerId equals b.LedgerId
                                select new ExpensesMasterView
                                {
                                    ExpensiveMasterId = a.ExpensiveMasterId,
                                    Amount = a.Amount,
                                    Narration = a.Narration,
                                    VoucherNo = a.VoucherNo,
                                    Status = a.Status,
                                    Date = a.Date,
                                    LedgerName = b.LedgerName
                                }).ToListAsync();
            return result;
        }
        public async Task<ExpensesMasterView> ExpensesView(int id)
        {
            var result = await (from a in _context.ExpenseMaster
                                join b in _context.AccountLedger on a.LedgerId equals b.LedgerId
                                where a.ExpensiveMasterId == id
                                select new ExpensesMasterView
                                {
                                    ExpensiveMasterId = a.ExpensiveMasterId,
                                    Amount = a.Amount,
                                    Narration = a.Narration,
                                    VoucherNo = a.VoucherNo,
                                    Status = a.Status,
                                    Date = a.Date,
                                    SerialNo = a.SerialNo,
                                    WarehouseId = a.WarehouseId,
                                    CompanyId = a.CompanyId,
                                    AddedDate = a.AddedDate,
                                    LedgerName = b.LedgerName,
                                    Email = b.Email,
                                    Phone = b.Phone,
                                    Address = b.Address
                                }).FirstOrDefaultAsync();
            return result;
        }
        public async Task<IList<ExpensesDetailsView>> ExpensesDetailsView(int id)
        {
            var result = await (from a in _context.ExpensesDetails
                                join b in _context.AccountLedger on a.LedgerId equals b.LedgerId
                                where a.ExpensiveMasterId == id
                                select new ExpensesDetailsView
                                {
                                    Id = id+1,
                                    ExpensesDetailsId = a.ExpensesDetailsId,
                                    Amount = a.Amount,
                                    Narration = a.Narration,
                                    LedgerId = a.LedgerId,
                                    LedgerName = b.LedgerName,
                                    LedgerCode = b.LedgerCode
                                }).ToListAsync();
            return result;
        }
        public async Task<ExpenseMaster> GetbyId(int id)
        {
            ExpenseMaster model = await _context.ExpenseMaster.FindAsync(id);
            return model;
        }

        public async Task<string> GetSerialNo()
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var val = sqlcon.Query<string>("SELECT ISNULL(MAX(SerialNo+1),1) as VoucherNo FROM ExpenseMaster", null, null, true, 0, commandType: CommandType.Text).FirstOrDefault();
                return val;
            }
        }

        public async Task<int> Save(ExpenseMaster model)
        {
            _context.ExpenseMaster.Add(model);
            await _context.SaveChangesAsync();
            int id = model.ExpensiveMasterId;



            //PaymentDetails table
            foreach (var item in model.listOrder)
            {
                //AddExpensesDetails
                ExpensesDetails details = new ExpensesDetails();
                if (item.LedgerId > 0)
                {
                    details.ExpensiveMasterId = id;
                    details.LedgerId = item.LedgerId;
                    details.Amount = item.Amount;
                    details.Narration = item.Narration;
                    _context.ExpensesDetails.Add(details);
                    await _context.SaveChangesAsync();
                    int intPurchaseDId = details.ExpensesDetailsId;
                }
            }
            return id;
        }

        public async Task<bool> ApprovedOk(ExpenseMaster model)
        {
            try
            {
                _context.ExpenseMaster.Update(model);
                await _context.SaveChangesAsync();
                //ExpensesDetails table
                foreach (var item in model.listOrder)
                {
                    //AddExpensesDetails
                    ExpensesDetails details = new ExpensesDetails();
                    if (item.LedgerId > 0)
                    {
                        details.ExpensesDetailsId = item.ExpensesDetailsId;
                        details.ExpensiveMasterId = model.ExpensiveMasterId;
                        details.LedgerId = item.LedgerId;
                        details.Amount = item.Amount;
                        details.Narration = item.Narration;
                        _context.ExpensesDetails.Update(details);
                        await _context.SaveChangesAsync();

                        //PostingExpensesLedger
                        LedgerPosting cashPosting = new LedgerPosting();
                        cashPosting.Date = model.Date;
                        cashPosting.NepaliDate = String.Empty;
                        cashPosting.LedgerId = item.LedgerId;
                        cashPosting.Debit = item.Amount;
                        cashPosting.Credit = 0;
                        cashPosting.VoucherNo = model.VoucherNo;
                        cashPosting.DetailsId = model.ExpensiveMasterId;
                        cashPosting.YearId = model.FinancialYearId;
                        cashPosting.InvoiceNo = model.VoucherNo;
                        cashPosting.VoucherTypeId = model.VoucherTypeId;
                        cashPosting.CompanyId = model.CompanyId;
                        cashPosting.LongReference = model.Narration;
                        cashPosting.ReferenceN = item.Narration;
                        cashPosting.ChequeNo = String.Empty;
                        cashPosting.ChequeDate = String.Empty;
                        cashPosting.AddedDate = DateTime.UtcNow;
                        _context.LedgerPosting.Add(cashPosting);
                        await _context.SaveChangesAsync();
                    }
                }

                //LedgerPosting
                //Supplier
                LedgerPosting ledgerPosting = new LedgerPosting();
                ledgerPosting.Date = model.Date;
                ledgerPosting.NepaliDate = String.Empty;
                ledgerPosting.LedgerId = model.LedgerId;
                ledgerPosting.Debit = 0;
                ledgerPosting.Credit = model.Amount;
                ledgerPosting.VoucherNo = model.VoucherNo;
                ledgerPosting.DetailsId = model.ExpensiveMasterId;
                ledgerPosting.YearId = model.FinancialYearId;
                ledgerPosting.InvoiceNo = model.VoucherNo;
                ledgerPosting.VoucherTypeId = model.VoucherTypeId;
                ledgerPosting.CompanyId = model.CompanyId;
                ledgerPosting.LongReference = model.Narration;
                ledgerPosting.ReferenceN = model.Narration;
                ledgerPosting.ChequeNo = String.Empty;
                ledgerPosting.ChequeDate = String.Empty;
                ledgerPosting.AddedDate = DateTime.UtcNow;
                _context.LedgerPosting.Add(ledgerPosting);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Update(ExpenseMaster model)
        {
            try
            {
                _context.ExpenseMaster.Update(model);
                await _context.SaveChangesAsync();

                //DeleteExpenseLedger
                using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
                {
                    var paraScDelete = new DynamicParameters();
                    paraScDelete.Add("@DetailsId", model.ExpensiveMasterId);
                    paraScDelete.Add("@VoucherTypeId", model.VoucherTypeId);
                    var valueScDelete = sqlcon.Query<int>("DELETE FROM LedgerPosting where DetailsId=@DetailsId AND VoucherTypeId=@VoucherTypeId", paraScDelete, null, true, 0, commandType: CommandType.Text);
                }
                //ExpensesDetails table
                foreach (var item in model.listOrder)
                {
                    //AddExpensesDetails
                    ExpensesDetails details = new ExpensesDetails();
                    if (item.ExpensesDetailsId == 0)
                    {
                        details.ExpensiveMasterId = model.ExpensiveMasterId;
                        details.LedgerId = item.LedgerId;
                        details.Amount = item.Amount;
                        details.Narration = item.Narration;
                        _context.ExpensesDetails.Add(details);
                        await _context.SaveChangesAsync();

                        //PostingExpensesLedger
                        LedgerPosting cashPosting = new LedgerPosting();
                        cashPosting.Date = model.Date;
                        cashPosting.NepaliDate = String.Empty;
                        cashPosting.LedgerId = item.LedgerId;
                        cashPosting.Debit = item.Amount;
                        cashPosting.Credit = 0;
                        cashPosting.VoucherNo = model.VoucherNo;
                        cashPosting.DetailsId = model.ExpensiveMasterId;
                        cashPosting.YearId = model.FinancialYearId;
                        cashPosting.InvoiceNo = model.VoucherNo;
                        cashPosting.VoucherTypeId = model.VoucherTypeId;
                        cashPosting.CompanyId = model.CompanyId;
                        cashPosting.LongReference = model.Narration;
                        cashPosting.ReferenceN = item.Narration;
                        cashPosting.ChequeNo = String.Empty;
                        cashPosting.ChequeDate = String.Empty;
                        cashPosting.AddedDate = DateTime.UtcNow;
                        _context.LedgerPosting.Add(cashPosting);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        details.ExpensesDetailsId = item.ExpensesDetailsId;
                        details.ExpensiveMasterId = model.ExpensiveMasterId;
                        details.LedgerId = item.LedgerId;
                        details.Amount = item.Amount;
                        details.Narration = item.Narration;
                        _context.ExpensesDetails.Update(details);
                        await _context.SaveChangesAsync();

                        //PostingExpensesLedger
                        LedgerPosting cashPosting = new LedgerPosting();
                        cashPosting.Date = model.Date;
                        cashPosting.NepaliDate = String.Empty;
                        cashPosting.LedgerId = item.LedgerId;
                        cashPosting.Debit = item.Amount;
                        cashPosting.Credit = 0;
                        cashPosting.VoucherNo = model.VoucherNo;
                        cashPosting.DetailsId = model.ExpensiveMasterId;
                        cashPosting.YearId = model.FinancialYearId;
                        cashPosting.InvoiceNo = model.VoucherNo;
                        cashPosting.VoucherTypeId = model.VoucherTypeId;
                        cashPosting.CompanyId = model.CompanyId;
                        cashPosting.LongReference = model.Narration;
                        cashPosting.ReferenceN = item.Narration;
                        cashPosting.ChequeNo = String.Empty;
                        cashPosting.ChequeDate = String.Empty;
                        cashPosting.AddedDate = DateTime.UtcNow;
                        _context.LedgerPosting.Add(cashPosting);
                        await _context.SaveChangesAsync();
                    }
                }

                //LedgerPosting
                //Supplier
                LedgerPosting ledgerPosting = new LedgerPosting();
                ledgerPosting.Date = model.Date;
                ledgerPosting.NepaliDate = String.Empty;
                ledgerPosting.LedgerId = model.LedgerId;
                ledgerPosting.Debit = 0;
                ledgerPosting.Credit = model.Amount;
                ledgerPosting.VoucherNo = model.VoucherNo;
                ledgerPosting.DetailsId = model.ExpensiveMasterId;
                ledgerPosting.YearId = model.FinancialYearId;
                ledgerPosting.InvoiceNo = model.VoucherNo;
                ledgerPosting.VoucherTypeId = model.VoucherTypeId;
                ledgerPosting.CompanyId = model.CompanyId;
                ledgerPosting.LongReference = model.Narration;
                ledgerPosting.ReferenceN = model.Narration;
                ledgerPosting.ChequeNo = String.Empty;
                ledgerPosting.ChequeDate = String.Empty;
                ledgerPosting.AddedDate = DateTime.UtcNow;
                _context.LedgerPosting.Add(ledgerPosting);
                await _context.SaveChangesAsync();

                foreach (var deleteitem in model.listDelete)
                {
                    ExpensesDetails x = _context.ExpensesDetails.Find(deleteitem.DeleteExpenditureId);
                    _context.ExpensesDetails.Remove(x);
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
