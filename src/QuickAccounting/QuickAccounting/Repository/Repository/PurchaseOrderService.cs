using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;
using QuickAccounting.Data.ViewModel;
using QuickAccounting.Repository.Interface;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Data;
using System.Net.NetworkInformation;

namespace QuickAccounting.Repository.Repository
{
    public class PurchaseOrderService : IPurchaseOrder
    {
        private readonly ApplicationDbContext _context;
        private readonly DatabaseConnection _conn;
        public PurchaseOrderService(ApplicationDbContext context, DatabaseConnection conn)
        {
            _context = context;
            _conn = conn;
        }
        public async Task<bool> CheckName(string name)
        {
            var checkResult = (from progm in _context.PurchaseOrder
                               where progm.VoucherNo == name
                               select progm.PurchaseOrderId).Count();
            if (checkResult > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<int> CheckNameId(string name)
        {
            var checkResult = (from progm in _context.PurchaseOrder
                               where progm.VoucherNo == name
                               select progm.PurchaseOrderId).Count();
            if (checkResult > 0)
            {

                var checkAccount = (from progm in _context.PurchaseOrder
                                    where progm.VoucherNo == name
                                    select progm.PurchaseOrderId).FirstOrDefault();
                return checkAccount;
            }
            else
            {
                return 0;
            }
        }

        public async Task<bool> Delete(PurchaseOrder master)
        {
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            try
            {

                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand cmd = new SqlCommand("PurchaseOrderDelete", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter para = new SqlParameter();
                para = cmd.Parameters.Add("@PurchaseOrderId", SqlDbType.Int);
                para.Value = master.PurchaseOrderId;
                para = cmd.Parameters.Add("@VoucherTypeId", SqlDbType.Int);
                para.Value = master.VoucherTypeId;
                para = cmd.Parameters.Add("@VoucherNo", SqlDbType.NVarChar);
                para.Value = master.VoucherNo;
                int rowAffacted = cmd.ExecuteNonQuery();
                if (rowAffacted > 0)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                sqlcon.Close();
            }
        }
        public async Task<List<PurchaseOrderView>> GetAll()
        {
            var result = await (from a in _context.PurchaseOrder
                                join b in _context.InvoiceSetting on a.VoucherTypeId equals b.VoucherTypeId
                                join c in _context.AccountLedger on a.LedgerId equals c.LedgerId
                                select new PurchaseOrderView
                                {
                                    PurchaseOrderId = a.PurchaseOrderId,
                                    VoucherNo = a.VoucherNo,
                                    Date = a.Date,
                                    Reference = a.Reference,
                                    GrandTotal = a.GrandTotal,
                                    BillDiscount = a.BillDiscount,
                                    TotalTax = a.TotalTax,
                                    Narration = a.Narration,
                                    Status = a.Status,
                                    PaymentStatus = a.PaymentStatus,
                                    VoucherTypeName = b.VoucherTypeName,
                                    LedgerName = c.LedgerName
                                }).ToListAsync();
            return result;
        }
        public async Task<List<PurchaseOrderView>> PurchaseInvoiceSearch(DateTime FromDate, DateTime ToDate, string VoucherNo, int LedgerId)
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var para = new DynamicParameters();
                para.Add("@FromDate", FromDate);
                para.Add("@ToDate", ToDate);
                para.Add("@VoucherNo", VoucherNo);
                para.Add("@LedgerId", LedgerId);
                var ListofPlan = sqlcon.Query<PurchaseOrderView>("PurchaseOrderSearch", para, null, true, 0, commandType: CommandType.StoredProcedure).ToList();
                return ListofPlan;
            }
        }

        public async Task<PurchaseOrder> GetbyId(int id)
        {
            PurchaseOrder model = await _context.PurchaseOrder.FindAsync(id);
            return model;
        }

        public async Task<int> Save(PurchaseOrder model)
        {
			model.Active = true;
			_context.PurchaseOrder.Add(model);
            await _context.SaveChangesAsync();
            int id = model.PurchaseOrderId;



			//PurchaseOrderDetail table
			foreach (var item in model.listOrder)
            {
				//AddPurchaseDetails
				PurchaseOrderDetail details = new PurchaseOrderDetail();
                details.PurchaseOrderId = id;
                details.ProductId = item.ProductId;
                details.Qty = item.Qty;
                details.UnitId = item.UnitId;
                details.BatchId = item.BatchId;
                details.Rate = item.Rate;
                details.Amount = item.Amount;
                details.NetAmount = item.NetAmount;
                details.GrossAmount = item.GrossAmount;
                details.Discount = item.Discount;
                details.DiscountAmount = item.DiscountAmount;
                details.TaxAmount = item.TaxAmount;
                details.TaxRate = item.TaxRate;
                details.TaxId = item.TaxId;
                details.OrderDetailsId = item.OrderDetailsId;
                details.Active = true;
				_context.PurchaseOrderDetails.Add(details);
                await _context.SaveChangesAsync();
                int intPurchaseDId = details.PurchaseOrderDetailsId;
                //AddStockPosting

                
            }
            return id;
        }

        public async Task<bool> Update(PurchaseOrder model)
        {
            try
            {
                _context.PurchaseOrder.Update(model);
                await _context.SaveChangesAsync();



                //PurchaseDetails table
                foreach (var item in model.listOrder)
                {
                    //AddPurchaseDetails
                    if (item.PurchaseOrderDetailsId == 0)
                    {
                        PurchaseOrderDetail details = new PurchaseOrderDetail();
                        details.PurchaseOrderId = model.PurchaseOrderId;
                        details.ProductId = item.ProductId;
                        details.Qty = item.Qty;
                        details.UnitId = item.UnitId;
                        details.BatchId = item.BatchId;
                        details.Rate = item.Rate;
                        details.Amount = item.Amount;
                        details.NetAmount = item.NetAmount;
                        details.GrossAmount = item.GrossAmount;
                        details.Discount = item.Discount;
                        details.DiscountAmount = item.DiscountAmount;
                        details.TaxAmount = item.TaxAmount;
                        details.TaxRate = item.TaxRate;
                        details.TaxId = item.TaxId;
                        details.OrderDetailsId = item.OrderDetailsId;
                        _context.PurchaseOrderDetails.Add(details);
                        await _context.SaveChangesAsync();
                        int intPurchaseDId = details.PurchaseOrderDetailsId;
                    }
                    else
                    {
						PurchaseOrderDetail details = new PurchaseOrderDetail();
                        details.PurchaseOrderDetailsId = item.PurchaseOrderDetailsId;
                        details.PurchaseOrderId = model.PurchaseOrderId;
                        details.ProductId = item.ProductId;
                        details.Qty = item.Qty;
                        details.UnitId = item.UnitId;
                        details.BatchId = item.BatchId;
                        details.Rate = item.Rate;
                        details.Amount = item.Amount;
                        details.NetAmount = item.NetAmount;
                        details.GrossAmount = item.GrossAmount;
                        details.Discount = item.Discount;
                        details.DiscountAmount = item.DiscountAmount;
                        details.TaxAmount = item.TaxAmount;
                        details.TaxRate = item.TaxRate;
                        details.TaxId = item.TaxId;
                        details.OrderDetailsId = item.OrderDetailsId;
                        _context.PurchaseOrderDetails.Update(details);
                        await _context.SaveChangesAsync();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                //dbTran.Rollback();
                return false;
            }
        }
        public async Task<bool> Approved(PurchaseOrder model)
        {
            try
            {
                //GetBalance
                var saleMaster = (from progm in _context.PurchaseOrder
                                  where progm.PurchaseOrderId == model.PurchaseOrderId
                                  select progm.PayAmount).FirstOrDefault();
                if (saleMaster > 0)
                {
                    model.PayAmount = saleMaster;
                    model.PreviousDue = model.GrandTotal - model.PayAmount;
                    model.BalanceDue = model.GrandTotal - model.PayAmount;
                    if (model.GrandTotal == model.PayAmount)
                    {
                        model.Status = "Paid";
                    }
                }
                model.PaymentStatus = "Approved";
                _context.PurchaseOrder.Update(model);
                await _context.SaveChangesAsync();



                ////PurchaseDetails table
                //foreach (var item in model.listOrder)
                //{
                //    //AddPurchaseDetails
                //        //AddStockPosting

                //        StockPosting stockposting = new StockPosting();
                //        stockposting.Date = model.Date;
                //        stockposting.ProductId = item.ProductId;
                //        stockposting.InwardQty = item.Qty;
                //        stockposting.OutwardQty = 0;
                //        stockposting.UnitId = item.UnitId;
                //        stockposting.BatchId = item.BatchId;
                //        stockposting.Rate = item.Rate;
                //        stockposting.DetailsId = item.PurchaseDetailsId;
                //        stockposting.InvoiceNo = model.VoucherNo;
                //        stockposting.VoucherNo = model.VoucherNo;
                //        stockposting.VoucherTypeId = model.VoucherTypeId;
                //        stockposting.AgainstInvoiceNo = String.Empty;
                //        stockposting.AgainstVoucherNo = String.Empty;
                //        stockposting.AgainstVoucherTypeId = 0;
                //        stockposting.WarehouseId = model.WarehouseId;
                //        stockposting.StockCalculate = "Purchase";
                //        stockposting.CompanyId = model.CompanyId;
                //        stockposting.FinancialYearId = model.FinancialYearId;
                //        stockposting.AddedDate = DateTime.UtcNow;
                //        _context.StockPosting.Add(stockposting);
                //        await _context.SaveChangesAsync();
                //}


                //    //LedgerPosting
                //    //Supplier
                //    LedgerPosting ledgerPosting = new LedgerPosting();
                //    ledgerPosting.Date = model.Date;
                //    ledgerPosting.NepaliDate = String.Empty;
                //    ledgerPosting.LedgerId = model.LedgerId;
                //    ledgerPosting.Debit = 0;
                //    ledgerPosting.Credit = model.GrandTotal;
                //    ledgerPosting.VoucherNo = model.VoucherNo;
                //    ledgerPosting.DetailsId = model.PurchaseOrderId;
                //    ledgerPosting.YearId = model.FinancialYearId;
                //    ledgerPosting.InvoiceNo = model.VoucherNo;
                //    ledgerPosting.VoucherTypeId = model.VoucherTypeId;
                //    ledgerPosting.CompanyId = model.CompanyId;
                //    ledgerPosting.LongReference = model.Narration;
                //    ledgerPosting.ReferenceN = model.Narration;
                //    ledgerPosting.ChequeNo = String.Empty;
                //    ledgerPosting.ChequeDate = String.Empty;
                //    ledgerPosting.AddedDate = DateTime.UtcNow;
                //    _context.LedgerPosting.Add(ledgerPosting);
                //    await _context.SaveChangesAsync();

                //    //PurchaseAccount Ledger send with out vat
                //    LedgerPosting purchasePosting = new LedgerPosting();
                //    decimal decSupplierCustomerAmount = Math.Round(model.GrandTotal - model.TotalTax, 2);
                //    purchasePosting.Date = model.Date;
                //    purchasePosting.NepaliDate = String.Empty;
                //    purchasePosting.LedgerId = 3;
                //    purchasePosting.Debit = decSupplierCustomerAmount;
                //    purchasePosting.Credit = 0;
                //    purchasePosting.VoucherNo = model.VoucherNo;
                //    purchasePosting.DetailsId = model.PurchaseOrderId;
                //    purchasePosting.YearId = model.FinancialYearId;
                //    purchasePosting.InvoiceNo = model.VoucherNo;
                //    purchasePosting.VoucherTypeId = model.VoucherTypeId;
                //    purchasePosting.CompanyId = model.CompanyId;
                //    purchasePosting.LongReference = model.Narration;
                //    purchasePosting.ReferenceN = model.Narration;
                //    purchasePosting.ChequeNo = String.Empty;
                //    purchasePosting.ChequeDate = String.Empty;
                //    purchasePosting.AddedDate = DateTime.UtcNow;
                //    _context.LedgerPosting.Add(purchasePosting);
                //    await _context.SaveChangesAsync();

                //    //Tax
                //    if (model.TotalTax > 0)
                //    {
                //        LedgerPosting vatPosting = new LedgerPosting();
                //        vatPosting.Date = model.Date;
                //        vatPosting.NepaliDate = String.Empty;
                //        vatPosting.LedgerId = 4;
                //        vatPosting.Debit = model.TotalTax;
                //        vatPosting.Credit = 0;
                //        vatPosting.VoucherNo = model.VoucherNo;
                //        vatPosting.DetailsId = model.PurchaseOrderId;
                //        vatPosting.YearId = model.FinancialYearId;
                //        vatPosting.InvoiceNo = model.VoucherNo;
                //        vatPosting.VoucherTypeId = model.VoucherTypeId;
                //        vatPosting.CompanyId = model.CompanyId;
                //        vatPosting.LongReference = model.Narration;
                //        vatPosting.ReferenceN = model.Narration;
                //        vatPosting.ChequeNo = String.Empty;
                //        vatPosting.ChequeDate = String.Empty;
                //        vatPosting.AddedDate = DateTime.UtcNow;
                //        _context.LedgerPosting.Add(vatPosting);
                //        await _context.SaveChangesAsync();
                //    }
                return true;
                //dbTran.Commit();
            }
            catch (Exception)
            {
                //dbTran.Rollback();
                return false;
            }
        }
        public async Task<bool> UpdatePurchaseInvoice(PurchaseOrder model)
        {
            try
            {
                //GetBalance
                var saleMaster = (from progm in _context.PurchaseOrder
                                  where progm.PurchaseOrderId == model.PurchaseOrderId
                                  select progm.PayAmount).FirstOrDefault();
                if (saleMaster > 0)
                {
                    model.PayAmount = saleMaster;
                    model.PreviousDue = model.GrandTotal - model.PayAmount;
                    model.BalanceDue = model.GrandTotal - model.PayAmount;
                    if (model.GrandTotal == model.PayAmount)
                    {
                        model.Status = "Paid";
                    }
                }
                model.PaymentStatus = "Approved";
                _context.PurchaseOrder.Update(model);
                await _context.SaveChangesAsync();



                ////PurchaseDetails table
                //foreach (var item in model.listOrder)
                //{
                //    ///AddPurchaseDetails
                //    if (item.PurchaseDetailsId == 0)
                //    {
                //        PurchaseDetails details = new PurchaseDetails();
                //        details.PurchaseMasterId = model.PurchaseOrderId;
                //        details.ProductId = item.ProductId;
                //        details.Qty = item.Qty;
                //        details.UnitId = item.UnitId;
                //        details.BatchId = item.BatchId;
                //        details.Rate = item.Rate;
                //        details.Amount = item.Amount;
                //        details.NetAmount = item.NetAmount;
                //        details.GrossAmount = item.GrossAmount;
                //        details.Discount = item.Discount;
                //        details.DiscountAmount = item.DiscountAmount;
                //        details.TaxAmount = item.TaxAmount;
                //        details.TaxRate = item.TaxRate;
                //        details.TaxId = item.TaxId;
                //        details.OrderDetailsId = item.OrderDetailsId;
                //        _context.PurchaseDetails.Add(details);
                //        await _context.SaveChangesAsync();
                //        int intPurchaseDId = details.PurchaseDetailsId;

                //        StockPosting stockposting = new StockPosting();
                //        stockposting.Date = model.Date;
                //        stockposting.ProductId = item.ProductId;
                //        stockposting.InwardQty = item.Qty;
                //        stockposting.OutwardQty = 0;
                //        stockposting.UnitId = item.UnitId;
                //        stockposting.BatchId = item.BatchId;
                //        stockposting.Rate = item.Rate;
                //        stockposting.DetailsId = intPurchaseDId;
                //        stockposting.InvoiceNo = model.VoucherNo;
                //        stockposting.VoucherNo = model.VoucherNo;
                //        stockposting.VoucherTypeId = model.VoucherTypeId;
                //        stockposting.AgainstInvoiceNo = String.Empty;
                //        stockposting.AgainstVoucherNo = String.Empty;
                //        stockposting.AgainstVoucherTypeId = 0;
                //        stockposting.WarehouseId = model.WarehouseId;
                //        stockposting.StockCalculate = "Purchase";
                //        stockposting.CompanyId = model.CompanyId;
                //        stockposting.FinancialYearId = model.FinancialYearId;
                //        stockposting.AddedDate = DateTime.UtcNow;
                //        _context.StockPosting.Add(stockposting);
                //        await _context.SaveChangesAsync();
                //    }
                //    else
                //    {
                //        PurchaseDetails details = new PurchaseDetails();
                //        details.PurchaseDetailsId = item.PurchaseDetailsId;
                //        details.PurchaseMasterId = model.PurchaseOrderId;
                //        details.ProductId = item.ProductId;
                //        details.Qty = item.Qty;
                //        details.UnitId = item.UnitId;
                //        details.BatchId = item.BatchId;
                //        details.Rate = item.Rate;
                //        details.Amount = item.Amount;
                //        details.NetAmount = item.NetAmount;
                //        details.GrossAmount = item.GrossAmount;
                //        details.Discount = item.Discount;
                //        details.DiscountAmount = item.DiscountAmount;
                //        details.TaxAmount = item.TaxAmount;
                //        details.TaxRate = item.TaxRate;
                //        details.TaxId = item.TaxId;
                //        details.OrderDetailsId = item.OrderDetailsId;
                //        _context.PurchaseDetails.Update(details);
                //        await _context.SaveChangesAsync();
                //        //UpdateStockPosting
                //        //GetStockkPostingId
                //        var returnstockpostingG = (from progm in _context.StockPosting
                //                                   where progm.VoucherTypeId == model.VoucherTypeId && progm.VoucherNo == model.VoucherNo && progm.DetailsId == item.PurchaseDetailsId
                //                                   select progm.StockPostingId).FirstOrDefault();

                //        StockPosting stockposting = new StockPosting();
                //        stockposting.StockPostingId = returnstockpostingG;
                //        stockposting.Date = model.Date;
                //        stockposting.ProductId = item.ProductId;
                //        stockposting.InwardQty = item.Qty;
                //        stockposting.OutwardQty = 0;
                //        stockposting.UnitId = item.UnitId;
                //        stockposting.BatchId = item.BatchId;
                //        stockposting.Rate = item.Rate;
                //        stockposting.DetailsId = item.PurchaseDetailsId;
                //        stockposting.InvoiceNo = model.VoucherNo;
                //        stockposting.VoucherNo = model.VoucherNo;
                //        stockposting.VoucherTypeId = model.VoucherTypeId;
                //        stockposting.AgainstInvoiceNo = String.Empty;
                //        stockposting.AgainstVoucherNo = String.Empty;
                //        stockposting.AgainstVoucherTypeId = 0;
                //        stockposting.WarehouseId = model.WarehouseId;
                //        stockposting.StockCalculate = "Purchase";
                //        stockposting.CompanyId = model.CompanyId;
                //        stockposting.FinancialYearId = model.FinancialYearId;
                //        stockposting.ModifyDate = DateTime.UtcNow;
                //        _context.StockPosting.Update(stockposting);
                //        await _context.SaveChangesAsync();
                //    }
                    
                //}

                ////DeletePurchaseInvoiceLedger
                //using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
                //{
                //    var paraScDelete = new DynamicParameters();
                //    paraScDelete.Add("@DetailsId", model.PurchaseOrderId);
                //    paraScDelete.Add("@VoucherTypeId", model.VoucherTypeId);
                //    var valueScDelete = sqlcon.Query<int>("DELETE FROM LedgerPosting where DetailsId=@DetailsId AND VoucherTypeId=@VoucherTypeId", paraScDelete, null, true, 0, commandType: CommandType.Text);
                //}
                ////LedgerPosting
                ////Supplier
                //LedgerPosting ledgerPosting = new LedgerPosting();
                //ledgerPosting.Date = model.Date;
                //ledgerPosting.NepaliDate = String.Empty;
                //ledgerPosting.LedgerId = model.LedgerId;
                //ledgerPosting.Debit = 0;
                //ledgerPosting.Credit = model.GrandTotal;
                //ledgerPosting.VoucherNo = model.VoucherNo;
                //ledgerPosting.DetailsId = model.PurchaseOrderId;
                //ledgerPosting.YearId = model.FinancialYearId;
                //ledgerPosting.InvoiceNo = model.VoucherNo;
                //ledgerPosting.VoucherTypeId = model.VoucherTypeId;
                //ledgerPosting.CompanyId = model.CompanyId;
                //ledgerPosting.LongReference = model.Narration;
                //ledgerPosting.ReferenceN = model.Narration;
                //ledgerPosting.ChequeNo = String.Empty;
                //ledgerPosting.ChequeDate = String.Empty;
                //ledgerPosting.AddedDate = DateTime.UtcNow;
                //_context.LedgerPosting.Add(ledgerPosting);
                //await _context.SaveChangesAsync();

                ////PurchaseAccount Ledger send with out vat
                //LedgerPosting purchasePosting = new LedgerPosting();
                //decimal decSupplierCustomerAmount = Math.Round(model.GrandTotal - model.TotalTax, 2);
                //purchasePosting.Date = model.Date;
                //purchasePosting.NepaliDate = String.Empty;
                //purchasePosting.LedgerId = 3;
                //purchasePosting.Debit = decSupplierCustomerAmount;
                //purchasePosting.Credit = 0;
                //purchasePosting.VoucherNo = model.VoucherNo;
                //purchasePosting.DetailsId = model.PurchaseOrderId;
                //purchasePosting.YearId = model.FinancialYearId;
                //purchasePosting.InvoiceNo = model.VoucherNo;
                //purchasePosting.VoucherTypeId = model.VoucherTypeId;
                //purchasePosting.CompanyId = model.CompanyId;
                //purchasePosting.LongReference = model.Narration;
                //purchasePosting.ReferenceN = model.Narration;
                //purchasePosting.ChequeNo = String.Empty;
                //purchasePosting.ChequeDate = String.Empty;
                //purchasePosting.AddedDate = DateTime.UtcNow;
                //_context.LedgerPosting.Add(purchasePosting);
                //await _context.SaveChangesAsync();

                ////Tax
                //if (model.TotalTax > 0)
                //{
                //    LedgerPosting vatPosting = new LedgerPosting();
                //    vatPosting.Date = model.Date;
                //    vatPosting.NepaliDate = String.Empty;
                //    vatPosting.LedgerId = 4;
                //    vatPosting.Debit = model.TotalTax;
                //    vatPosting.Credit = 0;
                //    vatPosting.VoucherNo = model.VoucherNo;
                //    vatPosting.DetailsId = model.PurchaseOrderId;
                //    vatPosting.YearId = model.FinancialYearId;
                //    vatPosting.InvoiceNo = model.VoucherNo;
                //    vatPosting.VoucherTypeId = model.VoucherTypeId;
                //    vatPosting.CompanyId = model.CompanyId;
                //    vatPosting.LongReference = model.Narration;
                //    vatPosting.ReferenceN = model.Narration;
                //    vatPosting.ChequeNo = String.Empty;
                //    vatPosting.ChequeDate = String.Empty;
                //    vatPosting.AddedDate = DateTime.UtcNow;
                //    _context.LedgerPosting.Add(vatPosting);
                //    await _context.SaveChangesAsync();
                //}
                //foreach (var deleteitem in model.listDelete)
                //{
                //    PurchaseDetails x = _context.PurchaseDetails.Find(deleteitem.PurchaseDetailsId);
                //    _context.PurchaseDetails.Remove(x);
                //    await _context.SaveChangesAsync();
                //}
                //foreach (var deleteitem in model.listDelete)
                //{
                //    var returnStockposting = (from ledgerpostingss in _context.StockPosting
                //                              where ledgerpostingss.DetailsId == deleteitem.PurchaseDetailsId && ledgerpostingss.VoucherNo == model.VoucherNo && ledgerpostingss.VoucherTypeId == model.VoucherTypeId
                //                              select ledgerpostingss.StockPostingId).FirstOrDefault();
                //    StockPosting returnView = _context.StockPosting.Find(returnStockposting);
                //    _context.StockPosting.Remove(returnView);
                //    _context.SaveChanges();
                //}
                return true;
                //dbTran.Commit();
            }
            catch (Exception)
            {
                //dbTran.Rollback();
                return false;
            }
        }
        public async Task<PurchaseOrderView> PurchaseInvoiceMasterView(int id)
        {
            var result = await (from a in _context.PurchaseOrder
                                join b in _context.InvoiceSetting on a.VoucherTypeId equals b.VoucherTypeId
                                join c in _context.AccountLedger on a.LedgerId equals c.LedgerId
                                where a.PurchaseOrderId == id
                                select new PurchaseOrderView
                                {
                                    PurchaseOrderId = a.PurchaseOrderId,
                                    VoucherNo = a.VoucherNo,
                                    Date = a.Date,
                                    Reference = a.Reference,
                                    GrandTotal = a.GrandTotal,
                                    BillDiscount = a.BillDiscount,
                                    TotalTax = a.TotalTax,
                                    TotalAmount = a.TotalAmount,
                                    NetAmounts = a.NetAmounts,
                                    Narration = a.Narration,
                                    Status = a.Status,
                                    PaymentStatus = a.PaymentStatus,
                                    VoucherTypeName = b.VoucherTypeName,
                                    LedgerName = c.LedgerName,
                                    Email = c.Email,
                                    Phone = c.Phone,
                                    Address = c.Address,
                                    AddedDate = a.AddedDate,
									TaxNo = c.TaxNo
								}).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<ProductView>> PurchaseInvoiceDetailsView(int id)
        {
            var varlist = (from a in _context.PurchaseOrderDetails
						   join b in _context.Product on a.ProductId equals b.ProductId
                           join c in _context.Unit on a.UnitId equals c.UnitId
                           where a.PurchaseOrderId == id
                           select new ProductView
                           {
							   PurchaseOrderDetailsId = a.PurchaseOrderDetailsId,
                               ProductId = a.ProductId,
                               Qty = a.Qty,
                               UnitId = a.UnitId,
                               TaxId = a.TaxId,
                               TaxAmount = a.TaxAmount,
                               SalesRate = a.Rate,
                               PurchaseRate = a.Rate,
                               Amount = a.Amount,
                               TotalAmount = a.NetAmount,
                               Discount = a.Discount,
                               DiscountAmount = a.DiscountAmount,
                               NetAmount = a.NetAmount,
                               ProductName = b.ProductName,
                               ProductCode = b.ProductCode,
                               UnitName = c.UnitName
                           }).ToList();

            return varlist;
        }

        public async Task<string> GetSerialNo()
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var val = sqlcon.Query<string>("SELECT ISNULL(MAX(SerialNo+1),1) as VoucherNo FROM PurchaseOrder", null, null, true, 0, commandType: CommandType.Text).FirstOrDefault();
                return val;
            }
        }

        public async Task<List<PurchaseOrderView>> PaymentAllocations(int PurchaseOrderId)
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var para = new DynamicParameters();
                para.Add("@PurchaseOrderId", PurchaseOrderId);
                var ListofPlan = sqlcon.Query<PurchaseOrderView>("PaymentPOAllocations", para, null, true, 0, commandType: CommandType.StoredProcedure).ToList();
                return ListofPlan;
            }
        }
    }
}
