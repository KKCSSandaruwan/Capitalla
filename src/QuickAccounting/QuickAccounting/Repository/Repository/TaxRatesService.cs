using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.HrPayroll;
using QuickAccounting.Data.ViewModel;
using QuickAccounting.Repository.Interface;

namespace QuickAccounting.Repository.Repository
{
    public class TaxRatesService: ITaxRates
    {
        private readonly ApplicationDbContext _context;
        public TaxRatesService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<TaxRatesView>> GetAll()
        {
            try
            {
                var res = await _context.TaxRates.Where(t => t.Active)
                              .Join(_context.Tax,
                                  t => t.TaxNameId,
                                  ta => ta.TaxId,
                                  (t, ta) => new TaxRatesView
                                  {
                                      Id = t.Id,
                                      FromDate = t.FromDate,
                                      TaxNameId = t.TaxNameId,
                                      TaxName = ta.TaxName,
                                      Rate = t.Rate
                                  })
                              .ToListAsync();
                return res;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<int> Save(TaxRates model)
        {
            try
            {
                
                await _context.TaxRates.AddAsync(model);
                await _context.SaveChangesAsync();
                int id = model.Id;
                return id;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<bool> Update(TaxRates model) 
        {
            
            _context.TaxRates.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<TaxRates> GetbyId(int id) 
        {
            var res = await _context.TaxRates.FindAsync(id);
            return res;
        }
        public async Task<bool> Delete(int id) 
        {
            var taxRete = await _context.TaxRates.FirstOrDefaultAsync(x => x.Id == id);
            if (taxRete != null)
            {
                taxRete.Active = false;
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
