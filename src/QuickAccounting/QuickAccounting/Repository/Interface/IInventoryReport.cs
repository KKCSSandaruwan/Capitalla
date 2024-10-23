using Microsoft.AspNetCore.Mvc;
using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;
using System.Data;

namespace QuickAccounting.Repository.Interface
{
    public interface IInventoryReport
    {
        Task<IList<PurchaseMasterView>> ItemWisePurchase(DateTime fromDate, DateTime toDate, int ProductId);
        Task<IList<SalesMasterView>> ItemWiseSales(DateTime fromDate, DateTime toDate, int ProductId);
        IList<InventoryViewFinal> StockReport(int GroupId, int ProductId, int CompanyId);
        Task<IList<FinancialReport>> CustomerReceiveSummary(DateTime fromDate, DateTime toDate, int accountGroupId , int ledgerId);
        DataSet CustomerLedgerOpening(DateTime fromDate, int LedgerId, int CompanyId);
        DataSet CustomerLedger(DateTime fromDate, DateTime toDate, int LedgerId, int CompanyId);
        DashboardView SalesPurchaseTotal( DateTime FromDate, DateTime ToDate, int CompanyId);
        DashboardView Receive(DateTime fromDate, DateTime toDate, int CompanyId);
        DashboardView Pay(DateTime fromDate, DateTime toDate, int CompanyId);
        Task<IList<FinancialReport>> CashBankBalance(DateTime fromDate, DateTime toDate);
        Task<IList<FinancialReport>> SalesChart(DateTime fromDate, DateTime toDate);
        Task<IList<FinancialReport>> PurchaseChart(DateTime fromDate, DateTime toDate);
        Task<IList<ProductView>> GetTopProduct();
		Task<IList<FinancialReport>> GetIncomeExpenses(DateTime fromDate, DateTime toDate);
	}
}
