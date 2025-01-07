using crypto;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using NPOI.POIFS.Crypt;
using QuickAccounting.Data;
using QuickAccounting.Data.Authentication;
using QuickAccounting.Data.Setting.Corporate;
using QuickAccounting.Repository.Interface;
using QuickAccounting.Repository.Interface.Corporate;
using QuickAccounting.Repository.Interface.Navigation;
using QuickAccounting.Repository.Interface.Security;
using QuickAccounting.Repository.Interface.SystemUser;
using QuickAccounting.Repository.Repository;
using QuickAccounting.Repository.Repository.Corporate;
using QuickAccounting.Repository.Repository.Navigation;
using QuickAccounting.Repository.Repository.Security;
using QuickAccounting.Repository.Repository.SystemUser;
using QuickAccounting.Repository.Services;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddMudServices();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);

//Security
builder.Services.AddTransient<IEncryption, EncryptionService>();

//Corporate
builder.Services.AddTransient<ICompanyDup, CompanyDupService>();

// System User
builder.Services.AddTransient<IUserRole, UserRoleService>();
builder.Services.AddTransient<IUserProfile, UserProfileService>();
builder.Services.AddTransient<IUserPrivilege, UserPrivilegeService>();

// Navigation
builder.Services.AddTransient<IMenuGroup, MenuGroupService>();
builder.Services.AddTransient<IMainMenu, MainMenuService>();
builder.Services.AddTransient<ISubMenu, SubMenuService>();
builder.Services.AddTransient<INavigationMenu, NavigationMenuService>();

builder.Services.AddTransient<IBrand, BrandService>();
builder.Services.AddTransient<ITax, TaxService>();
builder.Services.AddTransient<ICurrency, CurrencyService>();
builder.Services.AddTransient<IEmailSetting, EmailSettingService>();
builder.Services.AddTransient<ICompany, CompanyService>();
builder.Services.AddTransient<IInvoiceSetting, InvoiceService>();
builder.Services.AddTransient<IUnit, UnitService>();
builder.Services.AddTransient<ICategory, CategoryService>();
builder.Services.AddTransient<IProduct, ProductService>();
builder.Services.AddTransient<UserAccountService>();
builder.Services.AddTransient<IUser, UserService>();
builder.Services.AddTransient<ICustomerSupplier, CustomerSupplierService>();
builder.Services.AddTransient<IAccountGroup, AccountGroupService>();
builder.Services.AddTransient<IGeneralSetting, GeneralService>();
builder.Services.AddTransient<IPurchaseInvoice, PurchaseInvoiceService>();
builder.Services.AddTransient<IPaymentOut, PaymentOutService>();
builder.Services.AddTransient<IExpenses, ExpensesService>();
builder.Services.AddTransient<IPurchaseReturn, PurchaseReturnService>();
builder.Services.AddTransient<ISalesInvoice, SalesInvoiceService>();
builder.Services.AddTransient<IPaymentIn, PaymentInService>();
builder.Services.AddTransient<ISalesReturn, SalesReturnService>();
builder.Services.AddTransient<IJournalVoucher, JournalService>();
builder.Services.AddTransient<IAccountReport, AccountReportService>();
builder.Services.AddTransient<IInventoryReport, InventoryReportService>();
builder.Services.AddTransient<IPrivilege, PrivilegeService>();
builder.Services.AddTransient<IPayHead, PayheadService>();
builder.Services.AddTransient<IDesignation, DesignationService>();
builder.Services.AddTransient<IEmployee, EmployeeService>();
builder.Services.AddTransient<ISalaryPackage, SalaryPackageService>();
builder.Services.AddTransient<ISalaryMonthSetting, SalaryMonthSettingService>();
builder.Services.AddTransient<ISalaryVoucher, SalaryVoucherService>();
builder.Services.AddTransient<IAdvancePayment, AdvancePaymentService>();
builder.Services.AddTransient<IBonusDeduction, BonusDeductionService>();
builder.Services.AddTransient<IBudget, BudgetService>();
builder.Services.AddTransient<IAttendance, AttendanceService>();
builder.Services.AddTransient<IDailySalaryVoucher, DailySalaryVoucherService>();
builder.Services.AddTransient<ITaskCategory, TaskCategoryService>();
builder.Services.AddTransient<ITaskPriority, TaskPriorityService>();
builder.Services.AddTransient<ITasks, TaskService>();
builder.Services.AddTransient<IAccountLedger, AccountLedgerService>();
builder.Services.AddTransient<ITaxDetails, TaxDetailsService>();
builder.Services.AddTransient<ITaxRates, TaxRatesService>();
builder.Services.AddTransient<IPurchaseOrder, PurchaseOrderService>();

builder.Services.AddTransient<IPurchaseInvoiceSettlement, PurchaseInvoiceSettlementService>();
builder.Services.AddTransient<ISalesInvoiceSettlement, SalesInvoiceSettlementService>();

builder.Services.AddTransient<DataAccess>();
builder.Services.AddTransient<DatabaseConnection>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
