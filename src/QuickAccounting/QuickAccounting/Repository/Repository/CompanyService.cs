using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Setting.Corporate;
using QuickAccounting.Data.Setting.Navigation;
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

        public async Task<List<Company>> GetAllAsync()
        {
            try
            {
                var result = await (from c in _context.Company
                                    orderby c.CompanyName ascending
                                    select c).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching companies: {ex.Message}");
                throw;
            }
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
