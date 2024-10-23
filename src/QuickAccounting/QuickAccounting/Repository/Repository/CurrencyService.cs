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
    public class CurrencyService : ICurrency
    {
        private readonly ApplicationDbContext _context;
        private readonly DatabaseConnection _conn;
        public CurrencyService(ApplicationDbContext context, DatabaseConnection conn)
        {
            _context = context;
            _conn = conn;
        }
        public async Task<bool> CheckName(string name)
        {
            var checkResult = (from progm in _context.Currency
                               where progm.CurrencyName == name
                               select progm.CurrencyId).Count();
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
            var checkResult = (from progm in _context.Currency
                               where progm.CurrencyName == name
                               select progm.CurrencyId).Count();
            if (checkResult > 0)
            {

                var checkAccount = (from progm in _context.Currency
                                    where progm.CurrencyName == name
                                    select progm.CurrencyId).FirstOrDefault();
                return checkAccount;
            }
            else
            {
                return 0;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var checkResult = await(from progm in _context.Company
                                    where progm.CurrencyId == id
                                    select progm.CurrencyId).CountAsync();
            if (checkResult > 0)
            {
                return false;
            }
            else
            {
                Currency user = await _context.Currency.FindAsync(id);
                _context.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<List<CurrencyView>> GetAll()
        {
            var result = await(from a in _context.Currency
                               select new CurrencyView
                               {
                                   CurrencyId = a.CurrencyId,
                                   CurrencyName = a.CurrencyName,
                                   CurrencySymbol = a.CurrencySymbol,
                                   NoOfDecimalPlaces = a.NoOfDecimalPlaces
                               }).ToListAsync();
            return result;
        }

        public async Task<Currency> GetbyId(int id)
        {
            Currency model = await _context.Currency.FindAsync(id);
            return model;
        }
        public async Task<CurrencyView> GetCurrencyView(int id)
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var para = new DynamicParameters();
                para.Add("@CompanyId", id);
                var ListofPlan = sqlcon.Query<CurrencyView>("SELECT dbo.Currency.CurrencySymbol FROM            dbo.Company INNER JOIN dbo.Currency ON dbo.Company.CurrencyId = dbo.Currency.CurrencyId where Company.CompanyId=@CompanyId", para, null, true, 0, commandType: CommandType.Text).FirstOrDefault();
                return ListofPlan;
            }
        }

        public async Task<int> Save(Currency model)
        {
            await _context.Currency.AddAsync(model);
            await _context.SaveChangesAsync();
            int id = model.CurrencyId;
            return id;
        }

        public async Task<bool> Update(Currency model)
        {
            _context.Currency.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
