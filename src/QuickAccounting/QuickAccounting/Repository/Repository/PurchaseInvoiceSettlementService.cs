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
    public class PurchaseInvoiceSettlementService : IPurchaseInvoiceSettlement
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        // Constructor to initialize the service with the database context and auth
        public PurchaseInvoiceSettlementService(ApplicationDbContext context, AuthenticationStateProvider authenticationStateProvider)
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

        // Retrieves a list of suppliers with unsettled invoices.
        public async Task<List<AccountLedgerView>> GetUnsettledSuppliersAsync()
        {
            try
            {
                var result = await (from pm in _context.PurchaseMaster
                                    join al in _context.AccountLedger
                                    on pm.LedgerId equals al.LedgerId
                                    where pm.Status == "Unpaid" &&
                                          pm.PaymentStatus == "Approved"
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
                Console.WriteLine($"Error retrieving unsettled suppliers: {ex.Message}");
                throw new Exception("An error occurred while fetching unsettled suppliers.");
            }
        }

        // Retrieves unsettled invoices for a specified supplier.
        public async Task<List<PurchaseMaster>> GetUnsettledInvoicesBySupplierAsync(int supplierId)
        {
            try
            {
                var result = await (from pm in _context.PurchaseMaster
                                                       .Include(pm => pm.PurchaseDetails)
                                    where pm.Active == true &&
                                           pm.Status != "Paid" &&
                                           pm.PaymentStatus == "Approved" &&
                                           pm.LedgerId == supplierId
                                    select pm).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving unsettled invoices for supplier ID {supplierId}: {ex.Message}");
                throw new Exception("An error occurred while fetching unsettled invoices for the specified supplier.");
            }
        }

        // Settles multiple invoices by updating records and inserting payment and ledger postings.
        public async Task<bool> SettleInvoiceAsync(List<PurchaseMaster> invoicesToSettle)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
                string userName = authState.User.FindFirst(ClaimTypes.Name)?.Value;

                // Insert a new payment record for settling multiple invoices (PaymentMasterDup Table)
                PaymentMasterDup paymentMaster = new PaymentMasterDup
                {
                    LedgerId = invoicesToSettle.First().LedgerId,
                    AccountId = invoicesToSettle.First().AccountId ?? 0,
                    //TransactionType = TransactionType.SupplierPurchase.ToString(),
                    InvoiceType = InvoiceType.Purchase.ToString(),
                    SettlmentType = ProcessType.Multiple.ToString(),
                    PaymentType = PaymentMethod.Supplier.ToString(),
                    FinancialYearId = invoicesToSettle.First().FinancialYearId,
                    CompanyId = invoicesToSettle.First().CompanyId,
                    UserId = userName,
                    Narration = invoicesToSettle.First().Reference,
                    AddedDate = DateTime.Now
                };

                _context.PaymentMasterDup.Add(paymentMaster);
                await _context.SaveChangesAsync();
                int generatedPaymentMasterId = paymentMaster.PaymentMasterId;

                // Iterate over each invoice to update and create corresponding payment details
                foreach (var invoiceToSettle in invoicesToSettle)
                {
                    // Update the invoice details with the settlement information (PurchaseMaster Table)
                    var currentInvoice = await (from pm in _context.PurchaseMaster
                                                where pm.PurchaseMasterId == invoiceToSettle.PurchaseMasterId
                                                select pm).FirstOrDefaultAsync();

                    if (currentInvoice == null)
                        throw new KeyNotFoundException($"Invoice with ID {invoiceToSettle.PurchaseMasterId} not found.");

                    currentInvoice.PayAmount = invoiceToSettle.PayAmount;
                    currentInvoice.BalanceDue = invoiceToSettle.BalanceDue;
                    currentInvoice.PreviousDue = invoiceToSettle.PreviousDue;
                    currentInvoice.Status = invoiceToSettle.Status;
                    currentInvoice.Reference = invoiceToSettle.Reference;
                    currentInvoice.ModifyDate = DateTime.Now;

                    _context.PurchaseMaster.Update(currentInvoice);
                    await _context.SaveChangesAsync();

                    // Insert payment details for the settled invoice (PaymentDetailsDup Table)
                    PaymentDetailsDup paymentDetail = new PaymentDetailsDup
                    {
                        PaymentMasterId = generatedPaymentMasterId,
                        PurchaseMasterId = invoiceToSettle.PurchaseMasterId,
                        TotalAmount = invoiceToSettle.GrandTotal,
                        PaidAmount = invoiceToSettle.PayAmount,
                        DueAmount = invoiceToSettle.BalanceDue,
                        PaymentStatus = invoiceToSettle.Status,
                        AddedDate = DateTime.Now,
                    };

                    _context.PaymentDetailsDup.Add(paymentDetail);
                    await _context.SaveChangesAsync();

                    // After saving, use the generated PaymentDetailId to create SerialNo and VoucherNo
                    paymentDetail.SerialNo = paymentDetail.PaymentDetailId.ToString("D8");
                    paymentDetail.VoucherNo = $"PAY{paymentDetail.SerialNo}OUT";

                    // Update the record with the new SerialNo and VoucherNo
                    _context.PaymentDetailsDup.Update(paymentDetail);
                    await _context.SaveChangesAsync();
                }


                // Iterate over each invoice to Insert ledger postings for each invoice settled
                foreach (var invoiceToSettle in invoicesToSettle)
                {
                    // Insert a ledger posting for the payment made (LedgerPosting Table)
                    LedgerPosting paymentLedgerPosting = new LedgerPosting
                    {
                        Date = invoiceToSettle.Date,
                        NepaliDate = String.Empty,
                        LedgerId = invoiceToSettle.AccountId ?? 0,
                        Debit = (decimal)0.00,
                        Credit = invoiceToSettle.PayAmount,
                        VoucherNo = invoiceToSettle.VoucherNo,
                        DetailsId = invoiceToSettle.PurchaseMasterId,
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

                    // Insert a purchase posting to reflect the purchase details in the ledger (LedgerPosting Table)
                    LedgerPosting purchaseLedgerPosting = new LedgerPosting
                    {
                        Date = invoiceToSettle.Date,
                        NepaliDate = String.Empty,
                        LedgerId = invoiceToSettle.LedgerId,
                        Debit = invoiceToSettle.PayAmount,
                        Credit = (decimal)0.00,
                        VoucherNo = invoiceToSettle.VoucherNo,
                        DetailsId = invoiceToSettle.PurchaseMasterId,
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

                    _context.LedgerPosting.Add(purchaseLedgerPosting);
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
