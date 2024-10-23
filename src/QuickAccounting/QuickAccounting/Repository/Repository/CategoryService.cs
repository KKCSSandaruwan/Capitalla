using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Setting;
using QuickAccounting.Repository.Interface;

namespace QuickAccounting.Repository.Repository
{
    public class CategoryService : ICategory
    {
        private readonly ApplicationDbContext _context;
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CheckName(string name)
        {
            var checkResult = (from progm in _context.ProductGroup
                               where progm.GroupName == name
                               select progm.GroupId).Count();
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
            var checkResult = (from progm in _context.ProductGroup
                               where progm.GroupName == name
                               select progm.GroupId).Count();
            if (checkResult > 0)
            {

                var checkAccount = (from progm in _context.ProductGroup
                                    where progm.GroupName == name
                                    select progm.GroupId).FirstOrDefault();
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
                                    where progm.GroupId == id
                                    select progm.GroupId).CountAsync();
            if (checkResult > 0)
            {
                return false;
            }
            else
            {
                ProductGroup user = await _context.ProductGroup.FindAsync(id);
                _context.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<List<ProductGroupView>> GetAll()
        {
            var result = await(from a in _context.ProductGroup
                               select new ProductGroupView
                               {
                                   GroupId = a.GroupId,
                                   GroupName = a.GroupName,
                                   Image = a.Image
                               }).ToListAsync();
            return result;
        }

        public async Task<ProductGroup> GetbyId(int id)
        {
            ProductGroup model = await _context.ProductGroup.FindAsync(id);
            return model;
        }

        public async Task<int> Save(ProductGroup model)
        {
            await _context.ProductGroup.AddAsync(model);
            await _context.SaveChangesAsync();
            int id = model.GroupId;
            return id;
        }

        public async Task<bool> Update(ProductGroup model)
        {
            _context.ProductGroup.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
