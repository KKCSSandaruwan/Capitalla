using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Inventory;
using QuickAccounting.Repository.Interface;

namespace QuickAccounting.Repository.Repository
{
    public class AccountLedgerService: IAccountLedger
    {
        private readonly ApplicationDbContext _context;
        public AccountLedgerService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<AccountLedger>> GetSalesLedgers()
        {
            return await _context.AccountLedger.Where(x=>x.AccountGroupId == 10).ToListAsync();    
        }

    }
}
