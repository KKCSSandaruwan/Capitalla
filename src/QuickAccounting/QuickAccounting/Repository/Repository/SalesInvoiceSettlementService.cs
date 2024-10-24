using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;
using QuickAccounting.Enums;
using QuickAccounting.Repository.Interface;

namespace QuickAccounting.Repository.Repository
{
    public class SalesInvoiceSettlementService : ISalesInvoiceSettlement
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        // Constructor to initialize the service with the database context and auth
        public SalesInvoiceSettlementService(ApplicationDbContext context, AuthenticationStateProvider authenticationStateProvider)
        {
            try
            {
                _context = context ?? throw new ArgumentNullException(nameof(context), "Database context cannot be null.");
                _authenticationStateProvider = authenticationStateProvider;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing PurchaseInvoiceSettlementService: {ex.Message}");
                throw;
            }
        }

        // Retrieves a list of customers with unsettled invoices.
        public async Task<List<AccountLedgerView>> GetUnsettledCustomersAsync()
        {
            try
            {
                var result = await (from sm in _context.SalesMaster
                                    join al in _context.AccountLedger
                                    on sm.LedgerId equals al.LedgerId
                                    where sm.PaymentStatus == InvoiceStatus.Approved.ToString() &&
                                          sm.Status != PaymentStatus.Paid.ToString()
                                    select new AccountLedgerView
                                    {
                                        LedgerId = al.LedgerId,
                                        LedgerCode = al.LedgerCode,
                                        LedgerName = al.LedgerName
                                    }).Distinct().ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving unsettled customers: {ex.Message}");
                throw new Exception("An error occurred while fetching unsettled customers.", ex);
            }
        }

        // Retrieves unsettled invoices for a specified customer.
        public async Task<List<SalesMaster>> GetUnsettledInvoicesByCustomerAsync(int customerId)
        {
            try
            {
                var result = await (from sm in _context.SalesMaster
                                    where sm.PaymentStatus == InvoiceStatus.Approved.ToString() &&
                                          sm.Status != PaymentStatus.Paid.ToString() &&
                                          sm.LedgerId == customerId
                                    select sm).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving unsettled invoices for customer ID {customerId}: {ex.Message}");
                throw new Exception("An error occurred while fetching unsettled invoices for the specified customer.", ex);
            }
        }
    }
}
