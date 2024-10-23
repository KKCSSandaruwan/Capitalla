using QuickAccounting.Data.Setting;

namespace QuickAccounting.Repository.Interface
{
    public interface IGeneralSetting
    {
        Task<bool> Update(GeneralSetting model);
        Task<GeneralSetting> GetbyId(int id);
    }
}
