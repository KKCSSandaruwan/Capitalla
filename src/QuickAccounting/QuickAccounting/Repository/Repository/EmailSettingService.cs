using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Setting;
using QuickAccounting.Data.ViewModel;
using QuickAccounting.Repository.Interface;

namespace QuickAccounting.Repository.Repository
{
    public class EmailSettingService : IEmailSetting
    {
        private readonly ApplicationDbContext _context;
        public EmailSettingService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<EmailSetting> GetbyId(int id)
        {
            EmailSetting model = await _context.EmailSetting.FindAsync(id);
            return model;
        }

        public async Task<bool> Update(EmailSetting model)
        {
            _context.EmailSetting.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
