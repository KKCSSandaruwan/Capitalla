using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Setting;
using QuickAccounting.Data.ViewModel;
using QuickAccounting.Repository.Interface;

namespace QuickAccounting.Repository.Repository
{
    public class BrandService : IBrand
    {
        private readonly ApplicationDbContext _context;
        public BrandService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CheckName(string name)
        {
            var checkResult = (from progm in _context.Brand
                               where progm.Name == name
                               select progm.BrandId).Count();
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
            var checkResult = (from progm in _context.Brand
                               where progm.Name == name
                               select progm.BrandId).Count();
            if (checkResult > 0)
            {

                var checkAccount = (from progm in _context.Brand
                                    where progm.Name == name
                                    select progm.BrandId).FirstOrDefault();
                return checkAccount;
            }
            else
            {
                return 0;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var checkResult = await(from progm in _context.Product
                                    where progm.BrandId == id
                                    select progm.BrandId).CountAsync();
            if (checkResult > 0)
            {
                return false;
            }
            else
            {
                Brand user = await _context.Brand.FindAsync(id);
                _context.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<List<BrandView>> GetAll()
        {
            var result = await(from a in _context.Brand
                               select new BrandView
                               {
                                   BrandId = a.BrandId,
                                   Name = a.Name,
                                   Image = a.Image
                               }).ToListAsync();
            return result;
        }

        public async Task<Brand> GetbyId(int id)
        {
            Brand model = await _context.Brand.FindAsync(id);
            return model;
        }

        public async Task<int> Save(Brand model)
        {
            await _context.Brand.AddAsync(model);
            await _context.SaveChangesAsync();
            int id = model.BrandId;
            return id;
        }

        public async Task<bool> Update(Brand model)
        {
            _context.Brand.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
