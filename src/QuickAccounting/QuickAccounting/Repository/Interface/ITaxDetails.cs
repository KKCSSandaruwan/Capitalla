using QuickAccounting.Data.HrPayroll;
using QuickAccounting.Data.Setting;
using QuickAccounting.Data.ViewModel;

namespace QuickAccounting.Repository.Interface
{
    public interface ITaxDetails
    {
        Task<bool> Delete(int Id);
        Task<List<TaxDetails>> GetTaxCodes();
        Task<List<TaxRatesView>> GetTaxDetailsByTaxCode(string TaxCode, DateTime Date);
        Task<List<TaxDetailsView>> GetTaxDetails();
        Task<TaxDetailsView> GetTaxDetailById(int Id);
        Task<List<TaxView>> GetTaxAll();
        Task<int> Save(TaxDetailsView model);
        Task<bool> Update(TaxDetails model);
        Task<bool> CheckTaxCode(string TaxCode);
        Task<bool> CheckTaxCodeUpdate(string TaxCode, int Id);
    }
}
