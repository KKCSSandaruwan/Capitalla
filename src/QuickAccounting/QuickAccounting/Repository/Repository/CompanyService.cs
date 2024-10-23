using QuickAccounting.Data;
using QuickAccounting.Data.Setting;
using QuickAccounting.Repository.Interface;

namespace QuickAccounting.Repository.Repository
{
    public class CompanyService : ICompany
    {
        private readonly ApplicationDbContext _context;
        public CompanyService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Company> GetbyId(int id)
        {
            Company model = await _context.Company.FindAsync(id);
            return model;
        }

        public async Task<bool> Update(Company model)
        {
            _context.Company.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
