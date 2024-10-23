using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.HrPayroll;
using QuickAccounting.Data.HrPayrollModel;
using QuickAccounting.Repository.Interface;

namespace QuickAccounting.Repository.Repository
{
    public class PayheadService : IPayHead
    {
        private readonly ApplicationDbContext _context;
        public PayheadService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CheckName(string name)
        {
            var checkResult = (from progm in _context.PayHead
                               where progm.PayHeadName == name
                               select progm.PayHeadId).Count();
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
            var checkResult = (from progm in _context.PayHead
							   where progm.PayHeadName == name
                               select progm.PayHeadId).Count();
            if (checkResult > 0)
            {

                var checkAccount = (from progm in _context.PayHead
									where progm.PayHeadName == name
                                    select progm.PayHeadId).FirstOrDefault();
                return checkAccount;
            }
            else
            {
                return 0;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var checkResult = await(from progm in _context.Employee
                                    where progm.DesignationId == id
                                    select progm.DesignationId).CountAsync();
            if (checkResult > 0)
            {
                return false;
            }
            else
            {
                PayHead user = await _context.PayHead.FindAsync(id);
                _context.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<List<PayHeadView>> GetAll()
        {
            var result = await(from a in _context.PayHead
							   select new PayHeadView
							   {
                                   PayHeadId = a.PayHeadId,
                                   PayHeadName = a.PayHeadName,
                                   Type = a.Type
							   }).ToListAsync();
            return result;
        }

        public async Task<PayHead> GetbyId(int id)
        {
			PayHead model = await _context.PayHead.FindAsync(id);
            return model;
        }

        public async Task<int> Save(PayHead model)
        {
            await _context.PayHead.AddAsync(model);
            await _context.SaveChangesAsync();
            int id = model.PayHeadId;
            return id;
        }

        public async Task<bool> Update(PayHead model)
        {
            _context.PayHead.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
