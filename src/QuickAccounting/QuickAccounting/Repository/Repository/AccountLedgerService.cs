using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;
using QuickAccounting.Repository.Interface;

namespace QuickAccounting.Repository.Repository
{
    public class AccountLedgerService : IAccountLedger
    {
        private readonly ApplicationDbContext _context;
        public AccountLedgerService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<AccountLedger>> GetSalesLedgers()
        {
            return await _context.AccountLedger.Where(x => x.AccountGroupId == 10).ToListAsync();
        }

        // Retrieves a list of available payment sources.
        public async Task<List<AccountLedgerView>> GetPaymentSourcesAsync()
        {
            try
            {
                var result = await (from al in _context.AccountLedger
                                    join ag in _context.AccountGroup on al.AccountGroupId equals ag.AccountGroupId
                                    where new[] { 27, 28 }.Contains(al.AccountGroupId)
                                    select new AccountLedgerView
                                    {
                                        LedgerId = al.LedgerId,
                                        LedgerName = al.LedgerName,
                                        LedgerCode = al.LedgerCode,
                                    }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving payment sources: {ex.Message}");
                throw new Exception("An error occurred while fetching payment sources.", ex);
            }
        }

        // Retrieves account ledger details for a specified account ledger ID.
        public async Task<AccountLedger> GetAccountLedgersAsync(int accountLedgerId)
        {
            try
            {
                var result = await (from al in _context.AccountLedger
                                    where al.LedgerId == accountLedgerId
                                    select al).FirstOrDefaultAsync();

                if (result == null)
                    throw new KeyNotFoundException($"Account ledger with ID {accountLedgerId} not found.");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving account ledger for ID {accountLedgerId}: {ex.Message}");
                throw new Exception("An error occurred while fetching the account ledger. Please try again later.", ex);
            }
        }
    }
}
