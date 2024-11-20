using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;
using QuickAccounting.Data.ViewModel;
using QuickAccounting.Enums;
using QuickAccounting.Repository.Interface;
using QuickAccounting.Utilities;
using System.Security.Claims;

namespace QuickAccounting.Repository.Repository
{
    public class SalesInvoiceSettlementService : ISalesInvoiceSettlement
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        #region Constructor
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

        #endregion

        #region Fetch Mathods
        // Fetches a list of customers with unsettled invoices.
        public async Task<List<AccountLedgerView>> GetUnsettledCustomersAsync()
        {
            try
            {
                var invoiceStatuses = new List<String>
                {
                    InvoiceStatus.Approved.ToString()
                };

                var paymentStatuses = new List<String>
                {
                    PaymentStatus.Unpaid.ToString(),
                    PaymentStatus.Partial.ToString()
                };

                var result = await (from sm in _context.SalesMaster
                                    join al in _context.AccountLedger on sm.LedgerId equals al.LedgerId
                                    where invoiceStatuses.Contains(sm.PaymentStatus) &&
                                          paymentStatuses.Contains(sm.Status)
                                    orderby al.LedgerName ascending
                                    select new AccountLedgerView
                                    {
                                        LedgerId = al.LedgerId,
                                        LedgerCode = al.LedgerCode,
                                        LedgerName = al.LedgerName
                                    }).ToListAsync();

                // Remove duplicates
                var distinctResult = result.GroupBy(al => al.LedgerId)
                                           .Select(alg => alg.First())
                                           .ToList();

                return distinctResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching unsettled customers: {ex.Message}");
                throw new Exception("An error occurred while fetching unsettled customers.", ex);
            }
        }

        // Fetches a list of customers with settled invoices.
        public async Task<List<AccountLedgerView>> GetSettledCustomersAsync()
        {
            try
            {
                var invoiceStatuses = new List<String>
                {
                    InvoiceStatus.Approved.ToString()
                };

                var paymentStatuses = new List<String>
                {
                    PaymentStatus.Paid.ToString(),
                    PaymentStatus.Partial.ToString()
                };

                var result = await (from sm in _context.SalesMaster
                                    join al in _context.AccountLedger
                                    on sm.LedgerId equals al.LedgerId
                                    where invoiceStatuses.Contains(sm.PaymentStatus) &&
                                          paymentStatuses.Contains(sm.Status)
                                    orderby al.LedgerName ascending
                                    select new AccountLedgerView
                                    {
                                        LedgerId = al.LedgerId,
                                        LedgerCode = al.LedgerCode,
                                        LedgerName = al.LedgerName
                                    }).ToListAsync();

                // Remove duplicates
                var distinctResult = result.GroupBy(al => al.LedgerId)
                                           .Select(alg => alg.First())
                                           .ToList();

                return distinctResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching settled customers: {ex.Message}");
                throw new Exception("An error occurred while fetching settled customers.", ex);
            }
        }

        // Fetches a list of unsettled invoices for a specified customer.
        public async Task<List<SalesMasterView>> GetUnsettledInvoicesAsync(int customerId)
        {
            try
            {
                var invoiceStatuses = new List<String>
                {
                    InvoiceStatus.Approved.ToString()
                };

                var paymentStatuses = new List<String>
                {
                    PaymentStatus.Unpaid.ToString(),
                    PaymentStatus.Partial.ToString()
                };

                var result = await (from sm in _context.SalesMaster
                                    where sm.LedgerId == customerId &&
                                          invoiceStatuses.Contains(sm.PaymentStatus) &&
                                          paymentStatuses.Contains(sm.Status)
                                    orderby sm.SalesMasterId descending
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
                                        PreviousDue = sm.PreviousDue,
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
                Console.WriteLine($"Error fetching unsettled invoices for customer ID {customerId}: {ex.Message}");
                throw new Exception("An error occurred while fetching unsettled invoices for the specified customer.", ex);
            }
        }

        // Fetches a list of settled invoices with related receipt details by date range and optional customer ID filter.
        public async Task<List<SalesMasterView>> GetSettledInvoicesWithReceiptsAsync(DateTime fromDate, DateTime toDate, int? customerId = 0)
        {
            try
            {
                var invoiceStatuses = new List<String>
                {
                    InvoiceStatus.Approved.ToString()
                };

                var transactionStatus = new List<String>
                {
                    Enums.TransactionStatus.Approved.ToString()
                };

                var paymentStatuses = new List<String>
                {
                    PaymentStatus.Paid.ToString(),
                    PaymentStatus.Partial.ToString()
                };

                var result = await (from sm in _context.SalesMaster
                                    join rd in _context.ReceiptDetailsDup on sm.SalesMasterId equals rd.SalesMasterId
                                    join al in _context.AccountLedger on sm.LedgerId equals al.LedgerId into alGroup
                                    from al in alGroup.DefaultIfEmpty() // Left join with AccountLedger
                                    where rd.AddedDate >= fromDate &&
                                          rd.AddedDate <= toDate.Date.AddDays(1).AddTicks(-1) &&
                                          invoiceStatuses.Contains(sm.PaymentStatus) &&
                                          paymentStatuses.Contains(sm.Status) &&
                                          (customerId == 0 || sm.LedgerId == customerId)
                                    orderby rd.AddedDate descending
                                    select new SalesMasterView
                                    {
                                        SalesMasterId = sm.SalesMasterId,
                                        VoucherNo = sm.VoucherNo,
                                        Date = sm.Date,
                                        LedgerId = sm.LedgerId,
                                        LedgerName = al != null ? al.LedgerName : "N/A",
                                        GrandTotal = sm.GrandTotal,
                                        PayAmount = sm.PayAmount,
                                        BalanceDue = sm.BalanceDue,
                                        Status = sm.Status,
                                        ReceiptDetails = (from rd in _context.ReceiptDetailsDup
                                                          join rm in _context.ReceiptMasterDup on rd.ReceiptMasterId equals rm.ReceiptMasterId
                                                          join an in _context.AccountLedger on rm.AccountId equals an.LedgerId into anGroup
                                                          from an in anGroup.DefaultIfEmpty()
                                                          where rd.SalesMasterId == sm.SalesMasterId &&
                                                                transactionStatus.Contains(rd.TransactionStatus)
                                                          orderby rd.ReceiptDetailsId descending
                                                          select new ReceiptDetailsViewDup
                                                          {
                                                              ReceiptDetailsId = rd.ReceiptDetailsId,
                                                              ReceiptMasterId = rd.ReceiptMasterId,
                                                              SalesMasterId = rd.SalesMasterId,
                                                              VoucherNo = rd.VoucherNo,
                                                              AccountId = rm.AccountId,
                                                              AccountName = an != null ? an.LedgerName : "N/A",
                                                              ReceivedAmount = rd.ReceivedAmount,
                                                              DueAmount = rd.DueAmount,
                                                              PaymentStatus = rd.PaymentStatus,
                                                              AddedDate = rd.AddedDate
                                                          }).ToList()
                                    }).ToListAsync();

                // Remove duplicates
                var distinctResult = result
                    .GroupBy(x => x.SalesMasterId)
                    .Select(g => g.First())
                    .ToList();

                return distinctResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching settled sales invoices: {ex.Message}");
                throw new Exception("An error occurred while fetching settled sales invoices.", ex);
            }
        }

        // Fetches a object of settled invoice along with related receipt details by invoiceId and optional receiptId.
        public async Task<SalesMasterView> GetSettledInvoiceWithReceiptsAsync(int invoiceId, int? receiptId = 0, bool? includeReversedReceipts = false)
        {
            try
            {
                var transactionStatus = new List<String>
                {
                    Enums.TransactionStatus.Approved.ToString()
                };

                if ((includeReversedReceipts ?? false)) // Handles null as false
                    transactionStatus.Add(Enums.TransactionStatus.Reversed.ToString());

                var result = await (from sm in _context.SalesMaster
                                    where sm.SalesMasterId == invoiceId
                                    select new SalesMasterView
                                    {
                                        SalesMasterId = sm.SalesMasterId,
                                        VoucherNo = sm.VoucherNo,
                                        Date = sm.Date,
                                        CompanyId = sm.CompanyId,
                                        LedgerId = sm.LedgerId,
                                        GrandTotal = sm.GrandTotal,
                                        PayAmount = sm.PayAmount,
                                        BalanceDue = sm.BalanceDue,
                                        Status = sm.Status,
                                        UserId = sm.UserId,
                                        ReceiptDetails = (from rd in _context.ReceiptDetailsDup
                                                          join rm in _context.ReceiptMasterDup on rd.ReceiptMasterId equals rm.ReceiptMasterId
                                                          join ln in _context.AccountLedger on rm.LedgerId equals ln.LedgerId into lnGroup
                                                          from ln in lnGroup.DefaultIfEmpty()
                                                          join an in _context.AccountLedger on rm.AccountId equals an.LedgerId into anGroup
                                                          from an in anGroup.DefaultIfEmpty()
                                                          where (receiptId == 0 || rd.ReceiptDetailsId == receiptId) && 
                                                                rd.SalesMasterId == sm.SalesMasterId && 
                                                                transactionStatus.Contains(rd.TransactionStatus)
                                                          orderby rd.ReceiptDetailsId descending
                                                          select new ReceiptDetailsViewDup
                                                          {
                                                              ReceiptDetailsId = rd.ReceiptDetailsId,
                                                              ReceiptMasterId = rd.ReceiptMasterId,
                                                              SalesMasterId = rd.SalesMasterId,
                                                              LedgerId = rm.LedgerId,
                                                              LedgerName = ln != null ? ln.LedgerName : "N/A",
                                                              AccountId = rm.AccountId,
                                                              AccountName = an != null ? an.LedgerName : "N/A",
                                                              VoucherNo = rd.VoucherNo,
                                                              TransactionType = rm.TransactionType,
                                                              TransactionStatus = rd.TransactionStatus,
                                                              InvoiceType = rm.InvoiceType,
                                                              ProcessType = rm.ProcessType,
                                                              ReceiveableAmount = rd.ReceiveableAmount,
                                                              ReceivedAmount = rd.ReceivedAmount,
                                                              DueAmount = rd.DueAmount,
                                                              PaymentStatus = rd.PaymentStatus,
                                                              AddedBy = rm.UserId,
                                                              AddedDate = rd.AddedDate
                                                          }).ToList()
                                    }).FirstOrDefaultAsync();

                if (result == null)
                    throw new KeyNotFoundException($"Invoice with ID {invoiceId} not found.");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching settled sales invoice for Invoice ID {invoiceId}: {ex.Message}");
                throw new Exception("An error occurred while fetching the settled sales invoice.", ex);
            }
        }

        #endregion

        #region Process Mathods
        // Settles a list of multiple invoices and updates corresponding ledger entries in the database.
        public async Task<bool> SettleInvoiceAsync(List<SalesMasterView> invoicesToSettle)
        {
            // Begin a database transaction for the entire settle process
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
                    TransactionType = TransactionType.CustomerSale.ToString(),
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
                    var salesInvoice = await (from pm in _context.SalesMaster
                                              where pm.SalesMasterId == invoiceToSettle.SalesMasterId
                                              select pm).FirstOrDefaultAsync();

                    if (salesInvoice == null)
                        throw new KeyNotFoundException($"Invoice with ID {invoiceToSettle.SalesMasterId} not found.");

                    salesInvoice.PayAmount += invoiceToSettle.PayAmount;
                    salesInvoice.BalanceDue -= invoiceToSettle.PayAmount;
                    salesInvoice.PreviousDue = invoiceToSettle.PayAmount;
                    salesInvoice.Status = salesInvoice.BalanceDue == 0.00m ? PaymentStatus.Paid.ToString() : PaymentStatus.Partial.ToString();
                    salesInvoice.ModifyDate = DateTime.Now;

                    _context.SalesMaster.Update(salesInvoice);
                    await _context.SaveChangesAsync();

                    // Insert payment details for the settled invoice (ReceiptDetailsDup Table)
                    ReceiptDetailsDup receiptDetail = new ReceiptDetailsDup
                    {
                        ReceiptMasterId = generatedRreceiptMasterId,
                        SalesMasterId = invoiceToSettle.SalesMasterId,
                        SerialNo = IDGenerator.GenerateDummyID(),
                        VoucherNo = IDGenerator.GenerateDummyID(),
                        TransactionStatus = Enums.TransactionStatus.Approved.ToString(),
                        ReceiveableAmount = invoiceToSettle.PayAmount + salesInvoice.BalanceDue,
                        ReceivedAmount = invoiceToSettle.PayAmount,
                        DueAmount = salesInvoice.BalanceDue,
                        PaymentStatus = invoiceToSettle.Status,
                        AddedBy = userName,
                        AddedDate = DateTime.Now,
                    };

                    _context.ReceiptDetailsDup.Add(receiptDetail);
                    await _context.SaveChangesAsync();

                    // Update receipt detail entry with unique serial and voucher numbers (ReceiptDetailsDup Table)
                    int generatedReceiptDetailsId = receiptDetail.ReceiptDetailsId;
                    string serialNo = IDGenerator.GenerateSerialID(generatedReceiptDetailsId);
                    string voucherNo = IDGenerator.GenerateVoucherID(TransactionType.CustomerPayment, generatedReceiptDetailsId);

                    receiptDetail.SerialNo = serialNo;
                    receiptDetail.VoucherNo = voucherNo;

                    _context.ReceiptDetailsDup.Update(receiptDetail);
                    await _context.SaveChangesAsync();

                    // Insert a ledger posting for the payment received (LedgerPosting Table)
                    LedgerPosting paymentLedgerPosting = new LedgerPosting
                    {
                        Date = DateTime.Now,
                        NepaliDate = String.Empty,
                        VoucherTypeId = (int)TransactionType.CustomerPayment,
                        VoucherNo = voucherNo,
                        LedgerId = invoiceToSettle.AccountId ?? 0,
                        Debit = invoiceToSettle.PayAmount,              // Payment received (debit)
                        Credit = (decimal)0.00,                         // No credit for payment entry
                        DetailsId = invoiceToSettle.SalesMasterId,
                        YearId = invoiceToSettle.FinancialYearId,
                        InvoiceNo = invoiceToSettle.VoucherNo,
                        CompanyId = invoiceToSettle.CompanyId,
                        ReferenceN = invoiceToSettle.Reference,
                        LongReference = "N/A",
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
                        Date = DateTime.Now,
                        NepaliDate = String.Empty,
                        VoucherTypeId = (int)TransactionType.CustomerPayment,
                        VoucherNo = voucherNo,
                        LedgerId = invoiceToSettle.LedgerId,
                        Debit = (decimal)0.00,                      // No debit for sales entry
                        Credit = invoiceToSettle.PayAmount,         // Sales amount (credit)
                        DetailsId = invoiceToSettle.SalesMasterId,
                        YearId = invoiceToSettle.FinancialYearId,
                        InvoiceNo = invoiceToSettle.VoucherNo,
                        CompanyId = invoiceToSettle.CompanyId,
                        ReferenceN = invoiceToSettle.Reference,
                        LongReference = "N/A",
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

        // Reverses a list of multiple settled invoice receipts and updates corresponding ledger entries in the database.
        public async Task<bool> ReverseSettledInvoiceAsync(List<ReceiptDetailsViewDup> receiptsToReverse)
        {
            // Begin a database transaction for the entire reversal process
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Get the authentication state to retrieve the current user
                var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
                string? userName = authState.User.FindFirst(ClaimTypes.Name)?.Value;

                // Process each receipt in the list to reverse each transaction individually
                foreach (var receiptToReverse in receiptsToReverse)
                {
                    //Update the invoice details with the reverse transaction information(SalesMaster Table)
                    var salesInvoice = await (from pm in _context.SalesMaster
                                              where pm.SalesMasterId == receiptToReverse.SalesMasterId
                                              select pm).FirstOrDefaultAsync();

                    if (salesInvoice == null)
                        throw new KeyNotFoundException($"Invoice with ID {receiptToReverse.SalesMasterId} not found.");

                    salesInvoice.PayAmount -= receiptToReverse.ReceivedAmount;
                    salesInvoice.BalanceDue += receiptToReverse.ReceivedAmount;
                    salesInvoice.Status = salesInvoice.PayAmount == 0.00m ? PaymentStatus.Unpaid.ToString() : PaymentStatus.Partial.ToString();
                    salesInvoice.ModifyDate = DateTime.Now;

                    _context.SalesMaster.Update(salesInvoice);
                    await _context.SaveChangesAsync();

                    // Update the recipt details with the reverse transaction information (ReceiptDetailsDup Table)
                    var receiptDetail = await (from rd in _context.ReceiptDetailsDup
                                               where rd.ReceiptDetailsId == receiptToReverse.ReceiptDetailsId
                                               select rd).FirstOrDefaultAsync();

                    if (receiptDetail == null)
                        throw new KeyNotFoundException($"Receipt detail with ID {receiptToReverse.ReceiptDetailsId} not found.");

                    receiptDetail.TransactionStatus = Enums.TransactionStatus.Reversed.ToString();
                    receiptDetail.ModifiedBy = userName;
                    receiptDetail.ModifiedDate = DateTime.Now;

                    _context.ReceiptDetailsDup.Update(receiptDetail);
                    await _context.SaveChangesAsync();

                    // Insert transaction details for the reverse sales invoice (ReceiptReversal Table)
                    ReceiptReversal receiptReversal = new ReceiptReversal
                    {
                        ReceiptDetailsId = receiptToReverse.ReceiptDetailsId,
                        SerialNo = IDGenerator.GenerateDummyID(),
                        VoucherNo = IDGenerator.GenerateDummyID(),
                        ReversalAmount = receiptToReverse.ReceivedAmount,
                        Narration = receiptToReverse.Narration,
                        AddedBy = userName,
                        AddedDate = DateTime.Now
                    };

                    _context.ReceiptReversal.Add(receiptReversal);
                    await _context.SaveChangesAsync();

                    // Update reversal entry with unique serial and voucher numbers (ReceiptReversal Table)
                    // After saving, use the generated ReceiptReversalId to create SerialNo and VoucherNo
                    int generatedReceiptReversalId = receiptReversal.ReceiptReversalId;
                    string serialNo = IDGenerator.GenerateSerialID(generatedReceiptReversalId);
                    string voucherNo = IDGenerator.GenerateVoucherID(TransactionType.CustomerReversal, generatedReceiptReversalId);

                    receiptReversal.SerialNo = serialNo;
                    receiptReversal.VoucherNo = voucherNo;

                    _context.ReceiptReversal.Update(receiptReversal);
                    await _context.SaveChangesAsync();

                    // Insert a ledger posting for the reverse payment received (LedgerPosting Table)
                    LedgerPosting reversePaymentLedgerPosting = new LedgerPosting
                    {
                        Date = DateTime.Now,
                        NepaliDate = String.Empty,
                        VoucherTypeId = (int)TransactionType.CustomerReversal,
                        VoucherNo = voucherNo,
                        LedgerId = receiptToReverse.AccountId,
                        Debit = (decimal)0.00,                          // Reset payment received as debit
                        Credit = receiptToReverse.ReceivedAmount,       // Reverse the payment received (credit)
                        DetailsId = generatedReceiptReversalId,
                        YearId = salesInvoice.FinancialYearId,
                        InvoiceNo = salesInvoice.VoucherNo,
                        CompanyId = salesInvoice.CompanyId,
                        ReferenceN = receiptToReverse.Narration,
                        LongReference = "N/A",
                        ChequeNo = String.Empty,
                        ChequeDate = String.Empty,
                        AddedDate = DateTime.Now,
                        Active = true
                    };

                    _context.LedgerPosting.Add(reversePaymentLedgerPosting);
                    await _context.SaveChangesAsync();

                    // Insert a ledger posting for the reverse sales entry (LedgerPosting Table)
                    LedgerPosting reverseSalesLedgerPosting = new LedgerPosting
                    {
                        Date = DateTime.Now,
                        NepaliDate = String.Empty,
                        VoucherTypeId = (int)TransactionType.CustomerReversal,
                        VoucherNo = voucherNo,
                        LedgerId = receiptToReverse.LedgerId,
                        Debit = receiptToReverse.ReceivedAmount,    // Reverse the sales payment as debit
                        Credit = (decimal)0.00,                     // Reset sales amount as credit
                        DetailsId = generatedReceiptReversalId,
                        YearId = salesInvoice.FinancialYearId,
                        InvoiceNo = salesInvoice.VoucherNo,
                        CompanyId = salesInvoice.CompanyId,
                        ReferenceN = receiptToReverse.Narration,
                        LongReference = "N/A",
                        ChequeNo = String.Empty,
                        ChequeDate = String.Empty,
                        AddedDate = DateTime.Now,
                        Active = true
                    };

                    _context.LedgerPosting.Add(reverseSalesLedgerPosting);
                    await _context.SaveChangesAsync();
                }

                // Commit the transaction if everything is successful
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during invoice reversal process: {ex.Message}");
                await transaction.RollbackAsync();
                throw new Exception("An error occurred during the invoice reversal process.", ex);
            }
        }

        #endregion
    }
}
