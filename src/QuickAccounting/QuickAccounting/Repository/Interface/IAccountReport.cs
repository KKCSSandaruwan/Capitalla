using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;
using System.Data;

namespace QuickAccounting.Repository.Interface
{
    public interface IAccountReport
    {
        List<FinancialReport> TrailBalance(DateTime fromDate, DateTime toDate, int CompanyId);
        DataSet BalanceSheet(DateTime toDate, int CompanyId);
        DataSet BalanceSheetDetailed(DateTime toDate, int CompanyId, int AccountGroupId, bool isAsset);
        decimal StockValueGetOnDate(DateTime date, string calculationMethod, bool isOpeningStock, bool isFromBalanceSheet);
        DataSet ProfitAndLossAnalysisUpToaDateForBalansheet(DateTime fromDate, DateTime toDate, int CompanyId);
        DataSet ProfitAndLossAnalysisUpToaDateForPreviousYears(DateTime toDate, int CompanyId);
        DataSet ProfitAndLossAnalysis(DateTime fromDate, DateTime toDate, int CompanyId);
        DataSet ProfitAndLossAnalysisDetailed(DateTime fromDate, DateTime toDate, int CompanyId);
        decimal StockValueGetOnDate(DateTime date, DateTime dtToDate, string calculationMethod, bool isOpeningStock, bool isFromBalanceSheet);
        DataSet DayBook(DateTime fromDate, DateTime toDate, int VoucherTypeId, int LedgerId);
        DataSet LedgercountReport(DateTime fromDate, DateTime toDate, int LedgerId, string LedgerName, int CompanyId);
    }
}
