using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Setting;
using QuickAccounting.Repository.Interface;

namespace QuickAccounting.Repository.Repository
{
    public class InvoiceService : IInvoiceSetting
    {
        private readonly ApplicationDbContext _context;
        public InvoiceService(ApplicationDbContext context)
        {
            _context = context;
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
            var checkResult = await(from progm in _context.InvoiceSetting
                                    where progm.VoucherTypeId == id
                                    select progm.VoucherTypeId).CountAsync();
            if (checkResult > 0)
            {
                return false;
            }
            else
            {
                InvoiceSetting user = await _context.InvoiceSetting.FindAsync(id);
                _context.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<List<InvoiceSetting>> GetAll()
        {
            var result = await(from a in _context.InvoiceSetting
                               select new InvoiceSetting
                               {
                                   VoucherTypeId = a.VoucherTypeId,
                                   VoucherTypeName = a.VoucherTypeName,
                                   Suffix = a.Suffix,
                                   Prefix = a.Prefix,
                                   StartIndex = a.StartIndex
                               }).ToListAsync();
            return result;
        }

        public async Task<InvoiceSetting> GetbyId(int id)
        {
            InvoiceSetting model = await _context.InvoiceSetting.FindAsync(id);
            return model;
        }

        public async Task<bool> Update(InvoiceSetting model)
        {
            _context.InvoiceSetting.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
