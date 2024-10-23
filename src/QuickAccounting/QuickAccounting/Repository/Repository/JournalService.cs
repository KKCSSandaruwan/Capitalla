using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.AccountModel;
using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;
using QuickAccounting.Data.ViewModel;
using QuickAccounting.Repository.Interface;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace QuickAccounting.Repository.Repository
{
    public class JournalService : IJournalVoucher
    {
        private readonly ApplicationDbContext _context;
        private readonly DatabaseConnection _conn;
        public JournalService(ApplicationDbContext context, DatabaseConnection conn)
        {
            _context = context;
            _conn = conn;
        }
        public async Task<bool> Delete(JournalMaster master)
        {
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            try
            {

                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand cmd = new SqlCommand("JournalVoucherDelete", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter para = new SqlParameter();
                para = cmd.Parameters.Add("@JournalMasterId", SqlDbType.Int);
                para.Value = master.JournalMasterId;
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

        public async Task<IList<JournalMasterView>> GetAll(DateTime FromDate , DateTime ToDate , string VoucherNo)
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var para = new DynamicParameters();
                para.Add("@FromDate", FromDate);
                para.Add("@ToDate", ToDate);
                para.Add("@VoucherNo", VoucherNo);
                var ListofPlan = sqlcon.Query<JournalMasterView>("JournalVoucherGetAll", para, null, true, 0, commandType: CommandType.StoredProcedure).ToList();
                return ListofPlan;
            }
        }
        public async Task<JournalMasterView> JournalView(int id)
        {
            var result = await (from a in _context.JournalMaster
                                where a.JournalMasterId == id
                                select new JournalMasterView
                                {
                                    JournalMasterId = a.JournalMasterId,
                                    Amount = a.Amount,
                                    Narration = a.Narration,
                                    VoucherNo = a.VoucherNo,
                                    Status = a.Status,
                                    Date = a.Date,
                                    SerialNo = a.SerialNo,
                                    CompanyId = a.CompanyId,
                                    UserId = a.UserId,
                                    AddedDate = a.AddedDate
                                }).FirstOrDefaultAsync();
            return result;
        }
        public async Task<IList<JournalDetailsView>> JournalDetailsView(int id)
        {
            var result = await (from a in _context.JournalDetails
                                join b in _context.AccountLedger on a.LedgerId equals b.LedgerId
                                where a.JournalMasterId == id
                                select new JournalDetailsView
                                {
                                    Id = id+1,
                                    JournalDetailsId = a.JournalDetailsId,
                                    Credit = a.Credit,
                                    Debit = a.Debit,
                                    Narration = a.Narration,
                                    LedgerId = a.LedgerId,
                                    LedgerName = b.LedgerName
                                }).ToListAsync();
            return result;
        }
        public async Task<JournalMaster> GetbyId(int id)
        {
            JournalMaster model = await _context.JournalMaster.FindAsync(id);
            return model;
        }

        public async Task<string> GetSerialNo()
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var val = sqlcon.Query<string>("SELECT ISNULL(MAX(SerialNo+1),1) as VoucherNo FROM JournalMaster", null, null, true, 0, commandType: CommandType.Text).FirstOrDefault();
                return val;
            }
        }

        public async Task<int> Save(JournalMaster model)
        {
            _context.JournalMaster.Add(model);
            await _context.SaveChangesAsync();
            int id = model.JournalMasterId;



            //PaymentDetails table
            foreach (var item in model.listOrder)
            {
                //AddJournalDetails
                JournalDetails details = new JournalDetails();
                if (item.LedgerId > 0)
                {
                    details.JournalMasterId = id;
                    details.LedgerId = item.LedgerId;
                    details.Debit = item.Debit;
                    details.Credit = item.Credit;
                    details.Narration = item.Narration;
                    _context.JournalDetails.Add(details);
                    await _context.SaveChangesAsync();
                    int intPurchaseDId = details.JournalDetailsId;

                }
            }
            return id;
        }

        public async Task<bool> ApprovedOk(JournalMaster model)
        {
            try
            {
                _context.JournalMaster.Update(model);
                await _context.SaveChangesAsync();
                //JournalDetails table
                foreach (var item in model.listOrder)
                {
                    //AddJournalDetails
                    JournalDetails details = new JournalDetails();
                    if (item.LedgerId > 0)
                    {
                        details.JournalDetailsId = item.JournalDetailsId;
                        details.JournalMasterId = model.JournalMasterId;
                        details.LedgerId = item.LedgerId;
                        details.Debit = item.Debit;
                        details.Credit = item.Credit;
                        details.Narration = item.Narration;
                        _context.JournalDetails.Update(details);
                        await _context.SaveChangesAsync();

                        //PostingExpensesLedger
                        LedgerPosting cashPosting = new LedgerPosting();
                        cashPosting.Date = model.Date;
                        cashPosting.NepaliDate = String.Empty;
                        cashPosting.LedgerId = item.LedgerId;
                        cashPosting.Debit = item.Debit;
                        cashPosting.Credit = item.Credit;
                        cashPosting.VoucherNo = model.VoucherNo;
                        cashPosting.DetailsId = model.JournalMasterId;
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

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Update(JournalMaster model)
        {
            try
            {
                _context.JournalMaster.Update(model);
                await _context.SaveChangesAsync();

                //DeleteExpenseLedger
                using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
                {
                    var paraScDelete = new DynamicParameters();
                    paraScDelete.Add("@DetailsId", model.JournalMasterId);
                    paraScDelete.Add("@VoucherTypeId", model.VoucherTypeId);
                    var valueScDelete = sqlcon.Query<int>("DELETE FROM LedgerPosting where DetailsId=@DetailsId AND VoucherTypeId=@VoucherTypeId", paraScDelete, null, true, 0, commandType: CommandType.Text);
                }
                //JournalDetails table
                foreach (var item in model.listOrder)
                {
                    //AddJournalDetails
                    JournalDetails details = new JournalDetails();
                    if (item.JournalDetailsId == 0)
                    {
                        details.JournalMasterId = model.JournalMasterId;
                        details.LedgerId = item.LedgerId;
                        details.Debit = item.Debit;
                        details.Credit = item.Credit;
                        details.Narration = item.Narration;
                        _context.JournalDetails.Add(details);
                        await _context.SaveChangesAsync();

                        
                    }
                    else
                    {
                        details.JournalDetailsId = item.JournalDetailsId;
                        details.JournalMasterId = model.JournalMasterId;
                        details.LedgerId = item.LedgerId;
                        details.Debit = item.Debit;
                        details.Credit = item.Credit;
                        details.Narration = item.Narration;
                        _context.JournalDetails.Update(details);
                        await _context.SaveChangesAsync();
                    }
                }

                foreach (var item in model.listOrder)
                {
                    //LedgerPosting
                    //PostingExpensesLedger
                    LedgerPosting cashPosting = new LedgerPosting();
                    cashPosting.Date = model.Date;
                    cashPosting.NepaliDate = String.Empty;
                    cashPosting.LedgerId = item.LedgerId;
                    cashPosting.Debit = item.Debit;
                    cashPosting.Credit = item.Credit;
                    cashPosting.VoucherNo = model.VoucherNo;
                    cashPosting.DetailsId = model.JournalMasterId;
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

                        foreach (var deleteitem in model.listDelete)
                {
                    JournalDetails x = _context.JournalDetails.Find(deleteitem.DeleteJournalDetailsId);
                    _context.JournalDetails.Remove(x);
                   await _context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public decimal CheckLedgerBalance(int LedgerId)
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var para = new DynamicParameters();
                para.Add("@LedgerId", LedgerId);
                var val = sqlcon.Query<decimal>("SELECT isnull(SUM(Debit),0)- isnull(sum(Credit),0)  FROM LedgerPosting WHERE LedgerId=@LedgerId", para, null, true, 0, commandType: CommandType.Text).FirstOrDefault();
                return val;
            }
        }
    }
}
