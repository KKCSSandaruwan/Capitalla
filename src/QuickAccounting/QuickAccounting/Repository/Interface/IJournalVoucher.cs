using QuickAccounting.Data.AccountModel;
using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;
using QuickAccounting.Data.ViewModel;
using System.Threading.Tasks;

namespace QuickAccounting.Repository.Interface
{
    public interface IJournalVoucher
    {
        Task<IList<JournalMasterView>> GetAll(DateTime dtFromDate , DateTime dtToDate , string voucherNo);
        Task<JournalMasterView> JournalView(int id);
        Task<IList<JournalDetailsView>> JournalDetailsView(int id);
        Task<int> Save(JournalMaster model);
        Task<bool> ApprovedOk(JournalMaster model);
        Task<bool> Update(JournalMaster model);
        Task<JournalMaster> GetbyId(int id);
        Task<string> GetSerialNo();
        Task<bool> Delete(JournalMaster model);
        decimal CheckLedgerBalance(int LedgerId);
    }
}
