using QuickAccounting.Data.Setting;
using QuickAccounting.Data.ViewModel;

namespace QuickAccounting.Repository.Interface
{
    public interface IEmailSetting
    {
        Task<bool> Update(EmailSetting model);
        Task<EmailSetting> GetbyId(int id);
    }
}
