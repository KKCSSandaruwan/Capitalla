using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;

namespace QuickAccounting.Repository.Interface
{
    public interface IAccountLedger
    {
        Task<List<AccountLedger>> GetSalesLedgers();
    }
}
