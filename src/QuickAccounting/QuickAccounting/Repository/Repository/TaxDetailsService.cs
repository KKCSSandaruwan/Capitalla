using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.HrPayroll;
using QuickAccounting.Data.Setting;
using QuickAccounting.Data.ViewModel;
using QuickAccounting.Repository.Interface;
using System.Globalization;
using System.Linq.Dynamic.Core;
using System.Runtime.CompilerServices;

namespace QuickAccounting.Repository.Repository
{
    public class TaxDetailsService : ITaxDetails
    {
        private readonly ApplicationDbContext _context;
        public TaxDetailsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaxDetails>> GetTaxCodes()
        {
            var res = await _context.TaxDetails.GroupBy(td => td.TaxCode)
                      .Select(group => new TaxDetails { TaxCode = group.Key }).ToListAsync();

            return res;
        }

        public async Task<List<TaxRatesView>> GetTaxDetailsByTaxCode(string TaxCode, DateTime Date)
        {
			// DateTime fromDateFilter = DateTime.ParseExact(Date, "yyyy/MM/dd", CultureInfo.InvariantCulture);


			// ------------ 2025-01-02 
			var result = await (from td in _context.TaxDetails
								join tr in _context.TaxRates on td.TaxNameId equals tr.TaxNameId
								where td.TaxCode == TaxCode
									  && td.IsActive == true
									  && tr.FromDate == (from r in _context.TaxRates
														 where r.TaxNameId == td.TaxNameId
														 select r.FromDate).Max()
                                orderby td.TaxNameId
                                select new TaxRatesView
								{
									TaxNameId = td.TaxNameId,
									IsActive = td.IsActive,
									Rate = tr.Rate,
									FromDate = tr.FromDate
								}).ToListAsync();
            // ------------

            

			//var result =await (from td in _context.TaxDetails
			//             join tr in _context.TaxRates on td.TaxNameId equals tr.TaxNameId
			//             where tr.FromDate == _context.TaxRates.Where(r => r.FromDate <= Date)
			//                                          .Max(r => r.FromDate)
			//                && td.TaxCode == TaxCode
			//                   select new TaxRatesView
			//             {

			//                TaxNameId = td.TaxNameId,
			//                IsActive = td.IsActive,
			//                Rate = tr.Rate,
			//                FromDate = tr.FromDate
			//             }).ToListAsync();


			return result;
        }

        public async Task<List<TaxDetailsView>> GetTaxDetails()
        {
            var res = await _context.TaxDetails.Where(t => t.Active)
                        .Join(_context.Tax,
                            t => t.TaxNameId,
                            ta => ta.TaxId,
                            (t, ta) => new TaxDetailsView
                            {
                                Id = t.Id,
                                TaxCode = t.TaxCode,
                                TaxNameId = t.TaxNameId,
                                TaxName = ta.TaxName,
                                IsActive = t.IsActive
                            })
                        .ToListAsync();
            return res;
        }

        public async Task<TaxDetailsView> GetTaxDetailById(int Id)
        {

            var model = await _context.TaxDetails
                       .Where(td => td.Id == Id)
                       .Select(res => new TaxDetailsView
                       {
                           Id = res.Id,
                           TaxCode = res.TaxCode,
                           TaxNameId = res.TaxNameId,
                           IsActive = res.IsActive,
                           listModel = _context.TaxDetails
                               .Where(t => t.Id == res.Id)
                               .Join(_context.Tax,
                                   td => td.TaxNameId,
                                   t => t.TaxId,
                                   (td, t) => new TaxView { Checked = td.IsActive, TaxName = t.TaxName })
                               .ToList()
                       })
                       .SingleOrDefaultAsync();

            if (model == null)
            {
                throw new InvalidOperationException($"TaxDetails with Id {Id} not found.");
            }

            return model;

        }

        public async Task<bool> CheckTaxCode(string TaxCode)
        {
            return _context.TaxDetails.Any(x => x.TaxCode.ToUpper() == TaxCode.ToUpper());
        }

        public async  Task<bool> CheckTaxCodeUpdate(string TaxCode, int Id)
        {
            return _context.TaxDetails.Any(x => x.TaxCode.ToUpper() != TaxCode.ToUpper());
        }

        public async Task<List<TaxView>> GetTaxAll()
        {
            var result = await _context.Tax
                                .Where(x => x.IsActive).Select(x => new TaxView
                                {
                                    TaxId = x.TaxId,
                                    TaxName = x.TaxName,
                                    Checked = false
                                })
                                .ToListAsync();
            return result;
        }

        public async Task<int> Save(TaxDetailsView model)
        {
            try
            {
                foreach (var item in model.listModel)
                {
                    var res = new TaxDetails
                    {
                        TaxCode = model.TaxCode,
                        TaxNameId = item.TaxId,
                        IsActive = item.Checked,
                        CreatedDateTime = DateTime.Now,
                        Active = true
                    };
                    await _context.TaxDetails.AddAsync(res);

                }


                return await _context.SaveChangesAsync();
           
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> Update(TaxDetails model)
        {
            try
            {
                var res = _context.TaxDetails.FirstOrDefault(x => x.Id == model.Id);

                if (res != null)
                {

                  //  res.TaxCode = model.TaxCode;
                    res.IsActive = model.IsActive;

                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {

                throw;
            }
        }

      

        public async Task<bool> Delete(int Id)
        {
            var res = _context.TaxDetails.Find(Id);
            res.Active = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
