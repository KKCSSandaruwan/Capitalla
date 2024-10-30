using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data.AccountModel;
using QuickAccounting.Data.Authentication;
using QuickAccounting.Data.BudgetModel;
using QuickAccounting.Data.HrPayroll;
using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;

namespace QuickAccounting.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserMaster> UserMaster { get; set; }
        public DbSet<Privilege> Privilege { get; set; }
        public DbSet<AccountGroup> AccountGroup { get; set; }
        public DbSet<ProductGroup> ProductGroup { get; set; }
        public DbSet<AccountLedger> AccountLedger { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<ExpenseMaster> ExpenseMaster { get; set; }
        public DbSet<ExpensesDetails> ExpensesDetails { get; set; }
        public DbSet<FinancialYear> FinancialYear { get; set; }
        public DbSet<IncomeMaster> IncomeMaster { get; set; }
        public DbSet<LedgerPosting> LedgerPosting { get; set; }
        public DbSet<PaymentMaster> PaymentMaster { get; set; }
        public DbSet<PaymentDetails> PaymentDetails { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<PurchaseDetails> PurchaseDetails { get; set; }
        public DbSet<PurchaseMaster> PurchaseMaster { get; set; }
        public DbSet<PurchaseReturnDetails> PurchaseReturnDetails { get; set; }
        public DbSet<PurchaseReturnMaster> PurchaseReturnMaster { get; set; }
        public DbSet<ReceiptMaster> ReceiptMaster { get; set; }
        public DbSet<ReceiptDetails> ReceiptDetails { get; set; }
        public DbSet<SalesDetails> SalesDetails { get; set; }
        public DbSet<SalesMaster> SalesMaster { get; set; }
        public DbSet<SalesQuotationMaster> SalesQuotationMaster { get; set; }
        public DbSet<SalesQuotationDetails> SalesQuotationDetails { get; set; }
        public DbSet<SalesReturnDetails> SalesReturnDetails { get; set; }
        public DbSet<SalesReturnMaster> SalesReturnMaster { get; set; }
        public DbSet<Tax> Tax { get; set; }
        public DbSet<TaxDetails> TaxDetails { get; set; }
        public DbSet<TaxRates> TaxRates { get; set; }
        public DbSet<StockPosting> StockPosting { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<InvoiceSetting> InvoiceSetting { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }
        public DbSet<EmailSetting> EmailSetting { get; set; }
        public DbSet<GeneralSetting> GeneralSetting { get; set; }
        public DbSet<JournalMaster> JournalMaster { get; set; }
        public DbSet<JournalDetails> JournalDetails { get; set; }
        //Payroll
        public DbSet<AdvancePayment> AdvancePayment { get; set; }
        public DbSet<BonusDeduction> BonusDeduction { get; set; }
        public DbSet<DailyAttendanceMaster> DailyAttendanceMaster { get; set; }
        public DbSet<DailyAttendanceDetails> DailyAttendanceDetails { get; set; }
        public DbSet<DailySalaryVoucherMaster> DailySalaryVoucherMaster { get; set; }
        public DbSet<DailySalaryVoucherDetails> DailySalaryVoucherDetails { get; set; }
        public DbSet<Designation> Designation { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<PayHead> PayHead { get; set; }
        public DbSet<SalaryPackage> SalaryPackage { get; set; }
        public DbSet<SalaryPackageDetails> SalaryPackageDetails { get; set; }
        public DbSet<MonthlySalary> MonthlySalary { get; set; }
        public DbSet<MonthlySalaryDetails> MonthlySalaryDetails { get; set; }
        public DbSet<SalaryVoucherDetails> SalaryVoucherDetails { get; set; }
        public DbSet<SalaryVoucherMaster> SalaryVoucherMaster { get; set; }
        public DbSet<TaskCategory> TaskCategory { get; set; }
        public DbSet<TaskPriority> TaskPriority { get; set; }
        public DbSet<Tasks> Tasks { get; set; }

        //Budget
        public DbSet<BudgetMaster> BudgetMaster { get; set; }
        public DbSet<BudgetDetails> BudgetDetails { get; set; }

        //PurchaseOrders
        public DbSet<PurchaseOrder> PurchaseOrder { get; set; } = null!;

        //PurchaseOrdersDetais
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = null!;


        //Payment Master & Detail (Duplicate tables)
        public DbSet<PaymentMasterDup> PaymentMasterDup { get; set; }
        public DbSet<PaymentDetailsDup> PaymentDetailsDup { get; set; }

        //Receipt Master & Detail (Duplicate tables)
        public DbSet<ReceiptMasterDup> ReceiptMasterDup { get; set; }
        public DbSet<ReceiptDetailsDup> ReceiptDetailsDup { get; set; }
    }
}
