using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;
using QuickAccounting.Enums;
using QuickAccounting.Repository.Interface;
using System.Security.Claims;

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
        public async Task<List<SalesMasterView>> GetUnsettledInvoicesByCustomerAsync(int customerId)
        {
            try
            {
                var result = await (from sm in _context.SalesMaster
                                    where sm.PaymentStatus == InvoiceStatus.Approved.ToString() &&
                                          sm.Status != PaymentStatus.Paid.ToString() &&
                                          sm.LedgerId == customerId
                                    select new SalesMasterView
                                    {
                                        SalesMasterId = sm.SalesMasterId,
                                        VoucherNo = sm.VoucherNo,
                                        Date = sm.Date,
                                        VoucherTypeId = sm.VoucherTypeId,
                                        LedgerId = sm.LedgerId,
                                        Narration = sm.Narration,
                                        NetAmounts = sm.NetAmounts,
                                        GrandTotal = sm.GrandTotal,
                                        BillDiscount = sm.BillDiscount,
                                        TotalAmount = sm.TotalAmount,
                                        ShippingAmount = sm.ShippingAmount,
                                        PayAmount = sm.PayAmount,
                                        TotalTax = sm.TotalTax,
                                        UserId = sm.UserId,
                                        BalanceDue = sm.BalanceDue,
                                        TaxRate = sm.TaxRate,
                                        Status = sm.Status,
                                        WarehouseId = sm.WarehouseId,
                                        Reference = sm.Reference,
                                        PaymentStatus = sm.PaymentStatus,
                                        FinancialYearId = sm.FinancialYearId,
                                        CompanyId = sm.CompanyId,
                                        IsCreditNotes = sm.IsCreditNotes,
                                        AddedDate = sm.AddedDate,
                                        ModifyDate = sm.ModifyDate,
                                    }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving unsettled invoices for customer ID {customerId}: {ex.Message}");
                throw new Exception("An error occurred while fetching unsettled invoices for the specified customer.", ex);
            }
        }

        // Settles multiple sales invoices by updating records and inserting payment and ledger postings.
        public async Task<bool> SettleInvoiceAsync(List<SalesMasterView> invoicesToSettle)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
                string userName = authState.User.FindFirst(ClaimTypes.Name)?.Value;

                //Insert a new payment record for settling multiple invoices(ReceiptMasterDup Table)
                ReceiptMasterDup receiptMaster = new ReceiptMasterDup
                {
                    LedgerId = invoicesToSettle.First().LedgerId,
                    AccountId = invoicesToSettle.First().AccountId ?? 0,
                    TransactionType = TransactionType.CustomerSales.ToString(),
                    InvoiceType = InvoiceType.Sales.ToString(),
                    ProcessType = ProcessType.Multiple.ToString(),
                    FinancialYearId = invoicesToSettle.First().FinancialYearId,
                    CompanyId = invoicesToSettle.First().CompanyId,
                    UserId = userName,
                    Narration = invoicesToSettle.First().Reference,
                    AddedDate = DateTime.Now
                };

                _context.ReceiptMasterDup.Add(receiptMaster);
                await _context.SaveChangesAsync();
                int generatedRreceiptMasterId = receiptMaster.ReceiptMasterId;

                // Iterate over each invoice to update and create corresponding payment details
                foreach (var invoiceToSettle in invoicesToSettle)
                {
                    // Update the invoice details with the settlement information (SalesMaster Table)
                    var currentInvoice = await (from pm in _context.SalesMaster
                                                where pm.SalesMasterId == invoiceToSettle.SalesMasterId
                                                select pm).FirstOrDefaultAsync();

                    if (currentInvoice == null)
                        throw new KeyNotFoundException($"Invoice with ID {invoiceToSettle.SalesMasterId} not found.");

                    currentInvoice.PayAmount = invoiceToSettle.PayAmount;
                    currentInvoice.BalanceDue = invoiceToSettle.BalanceDue;
                    currentInvoice.PreviousDue = invoiceToSettle.PreviousDue;
                    currentInvoice.Status = invoiceToSettle.Status;
                    currentInvoice.Reference = invoiceToSettle.Reference;
                    currentInvoice.ModifyDate = DateTime.Now;

                    _context.SalesMaster.Update(currentInvoice);
                    await _context.SaveChangesAsync();

                    // Insert payment details for the settled invoice (ReceiptDetailsDup Table)
                    ReceiptDetailsDup receiptDetail = new ReceiptDetailsDup
                    {
                        ReceiptMasterId = generatedRreceiptMasterId,
                        SalesMasterId = invoiceToSettle.SalesMasterId,
                        TransactionStatus = TransactionStatus.Approved.ToString(),
                        ReceiveableAmount = invoiceToSettle.GrandTotal,
                        ReceivedAmount = invoiceToSettle.PayAmount,
                        DueAmount = invoiceToSettle.BalanceDue,
                        PaymentStatus = invoiceToSettle.Status,
                        AddedDate = DateTime.Now,
                    };

                    _context.ReceiptDetailsDup.Add(receiptDetail);
                    await _context.SaveChangesAsync();

                    // After saving, use the generated RreceiptMasterId to create SerialNo and VoucherNo
                    receiptDetail.SerialNo = receiptDetail.ReceiptDetailsId.ToString("D8");
                    receiptDetail.VoucherNo = $"PAY{receiptDetail.SerialNo}IN";

                    // Update the record with the new SerialNo and VoucherNo
                    _context.ReceiptDetailsDup.Update(receiptDetail);
                    await _context.SaveChangesAsync();
                }

                // Iterate over each invoice to Insert ledger postings for each invoice settled
                foreach (var invoiceToSettle in invoicesToSettle)
                {
                    // Insert a ledger posting for the payment received (LedgerPosting Table)
                    LedgerPosting paymentLedgerPosting = new LedgerPosting
                    {
                        Date = invoiceToSettle.Date,
                        NepaliDate = String.Empty,
                        LedgerId = invoiceToSettle.AccountId ?? 0,   // Account related to the payment received
                        Debit = invoiceToSettle.PayAmount,           // Payment received as debit
                        Credit = (decimal)0.00,
                        VoucherNo = invoiceToSettle.VoucherNo,
                        DetailsId = invoiceToSettle.SalesMasterId,
                        YearId = invoiceToSettle.FinancialYearId,
                        InvoiceNo = invoiceToSettle.VoucherNo,
                        VoucherTypeId = invoiceToSettle.VoucherTypeId,
                        CompanyId = invoiceToSettle.CompanyId,
                        LongReference = invoiceToSettle.Reference,
                        ReferenceN = invoiceToSettle.Narration,
                        ChequeNo = String.Empty,
                        ChequeDate = String.Empty,
                        AddedDate = DateTime.Now,
                        Active = true
                    };

                    _context.LedgerPosting.Add(paymentLedgerPosting);
                    await _context.SaveChangesAsync();

                    // Insert a sales posting to reflect the sales details in the ledger (LedgerPosting Table)
                    LedgerPosting salesLedgerPosting = new LedgerPosting
                    {
                        Date = invoiceToSettle.Date,
                        NepaliDate = String.Empty,
                        LedgerId = invoiceToSettle.LedgerId,        // Sales account for the transaction
                        Debit = (decimal)0.00,
                        Credit = invoiceToSettle.PayAmount,         // Sales amount as credit
                        VoucherNo = invoiceToSettle.VoucherNo,
                        DetailsId = invoiceToSettle.SalesMasterId,
                        YearId = invoiceToSettle.FinancialYearId,
                        InvoiceNo = invoiceToSettle.VoucherNo,
                        VoucherTypeId = invoiceToSettle.VoucherTypeId,
                        CompanyId = invoiceToSettle.CompanyId,
                        LongReference = invoiceToSettle.Reference,
                        ReferenceN = invoiceToSettle.Narration,
                        ChequeNo = String.Empty,
                        ChequeDate = String.Empty,
                        AddedDate = DateTime.Now,
                        Active = true
                    };

                    _context.LedgerPosting.Add(salesLedgerPosting);
                    await _context.SaveChangesAsync();
                }

                // Commit the transaction if everything is successful
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during invoice settlement: {ex.Message}");
                await transaction.RollbackAsync();
                throw new Exception("An error occurred during the settlement process.", ex);
            }
        }
    }
}
