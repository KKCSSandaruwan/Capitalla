using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;
using QuickAccounting.Repository.Interface;
using System.Data;

namespace QuickAccounting.Repository.Repository
{
    public class SalesReturnService : ISalesReturn
    {
        private readonly ApplicationDbContext _context;
        private readonly DatabaseConnection _conn;
        public SalesReturnService(ApplicationDbContext context, DatabaseConnection conn)
        {
            _context = context;
            _conn = conn;
        }
        public async Task<bool> CheckName(string name)
        {
            var checkResult = (from progm in _context.SalesReturnMaster
                               where progm.VoucherNo == name
                               select progm.SalesReturnMasterId).Count();
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
            var checkResult = (from progm in _context.SalesReturnMaster
                               where progm.VoucherNo == name
                               select progm.SalesReturnMasterId).Count();
            if (checkResult > 0)
            {

                var checkAccount = (from progm in _context.SalesReturnMaster
                                    where progm.VoucherNo == name
                                    select progm.SalesReturnMasterId).FirstOrDefault();
                return checkAccount;
            }
            else
            {
                return 0;
            }
        }

        public async Task<bool> Delete(SalesReturnMaster master)
        {
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            try
            {

                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand cmd = new SqlCommand("SalesReturnInvoiceDelete", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter para = new SqlParameter();
                para = cmd.Parameters.Add("@SalesReturnMasterId", SqlDbType.Int);
                para.Value = master.SalesReturnMasterId;
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
        public async Task<List<SalesReturnMasterView>> GetAll()
        {
            var result = await (from a in _context.SalesReturnMaster
                                join b in _context.InvoiceSetting on a.VoucherTypeId equals b.VoucherTypeId
                                join c in _context.AccountLedger on a.LedgerId equals c.LedgerId
                                select new SalesReturnMasterView
                                {
                                    SalesReturnMasterId = a.SalesReturnMasterId,
                                    VoucherNo = a.VoucherNo,
                                    Date = a.Date,
                                    GrandTotal = a.GrandTotal,
                                    BillDiscount = a.BillDiscount,
                                    TotalTax = a.TotalTax,
                                    Narration = a.Narration,
                                    Status = a.Status,
                                    VoucherTypeName = b.VoucherTypeName,
                                    LedgerName = c.LedgerName
                                }).ToListAsync();
            return result;
        }
        public async Task<List<SalesReturnMasterView>> SalesReturnInvoiceSearch(DateTime FromDate, DateTime ToDate, string VoucherNo, int LedgerId)
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var para = new DynamicParameters();
                para.Add("@FromDate", FromDate);
                para.Add("@ToDate", ToDate);
                para.Add("@VoucherNo", VoucherNo);
                para.Add("@LedgerId", LedgerId);
                var ListofPlan = sqlcon.Query<SalesReturnMasterView>("SalesReturnInvoiceSearch", para, null, true, 0, commandType: CommandType.StoredProcedure).ToList();
                return ListofPlan;
            }
        }

        public async Task<SalesReturnMaster> GetbyId(int id)
        {
            SalesReturnMaster model = await _context.SalesReturnMaster.FindAsync(id);
            return model;
        }

        public async Task<int> Save(SalesReturnMaster model)
        {
            _context.SalesReturnMaster.Add(model);
            await _context.SaveChangesAsync();
            int id = model.SalesReturnMasterId;



            //SalesDetails table
            foreach (var item in model.listOrder)
            {
                //AddSalesDetails
                SalesReturnDetails details = new SalesReturnDetails();
                details.SalesReturnMasterId = id;
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
                details.SCCSLAmount = item.SCCSLAmount;
                details.TaxRate = item.TaxRate;
                details.TaxId = item.TaxId;
                details.SalesReturnDetailsId = item.SalesReturnDetailsId;
                _context.SalesReturnDetails.Add(details);
                await _context.SaveChangesAsync();
                int intPurchaseDId = details.SalesReturnDetailsId;
                //AddStockPosting

                
            }
            return id;
        }

        public async Task<bool> Update(SalesReturnMaster model)
        {
            try
            {
                _context.SalesReturnMaster.Update(model);
                await _context.SaveChangesAsync();



                //SalesDetails table
                foreach (var item in model.listOrder)
                {
                    //AddSalesDetails
                    if (item.SalesReturnDetailsId == 0)
                    {
                        SalesReturnDetails details = new SalesReturnDetails();
                        details.SalesReturnMasterId = model.SalesReturnMasterId;
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
                        details.SalesDetailsId = item.SalesDetailsId;
                        _context.SalesReturnDetails.Add(details);
                        await _context.SaveChangesAsync();
                        int intPurchaseDId = details.SalesReturnDetailsId;
                    }
                    else
                    {
                        SalesReturnDetails details = new SalesReturnDetails();
                        details.SalesReturnDetailsId = item.SalesReturnDetailsId;
                        details.SalesReturnMasterId = model.SalesReturnMasterId;
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
                        details.SalesDetailsId = item.SalesDetailsId;
                        _context.SalesReturnDetails.Update(details);
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
        public async Task<bool> Approved(SalesReturnMaster model)
        {
            try
            {
                model.Status = "Approved";
                _context.SalesReturnMaster.Update(model);
                await _context.SaveChangesAsync();



                //SalesDetails table
                foreach (var item in model.listOrder)
                {
                    //AddSalesDetails
                        //AddStockPosting

                        StockPosting stockposting = new StockPosting();
                        stockposting.Date = model.Date;
                        stockposting.ProductId = item.ProductId;
                        stockposting.InwardQty = item.Qty;
                        stockposting.OutwardQty = 0;
                        stockposting.UnitId = item.UnitId;
                        stockposting.BatchId = item.BatchId;
                        stockposting.Rate = item.Rate;
                        stockposting.DetailsId = item.SalesReturnDetailsId;
                        stockposting.InvoiceNo = model.VoucherNo;
                        stockposting.VoucherNo = model.VoucherNo;
                        stockposting.VoucherTypeId = model.VoucherTypeId;
                        stockposting.AgainstInvoiceNo = String.Empty;
                        stockposting.AgainstVoucherNo = String.Empty;
                        stockposting.AgainstVoucherTypeId = 0;
                        stockposting.WarehouseId = model.WarehouseId;
                        stockposting.StockCalculate = "Sales";
                        stockposting.CompanyId = model.CompanyId;
                        stockposting.FinancialYearId = model.FinancialYearId;
                        stockposting.AddedDate = DateTime.Now;
                        stockposting.Active = true;
						_context.StockPosting.Add(stockposting);
                        await _context.SaveChangesAsync();
                }


                    //LedgerPosting
                    //Customer
                    LedgerPosting ledgerPosting = new LedgerPosting();
                    ledgerPosting.Date = model.Date;
                    ledgerPosting.NepaliDate = String.Empty;
                    ledgerPosting.LedgerId = model.LedgerId;
                    ledgerPosting.Debit = 0;
                    ledgerPosting.Credit = model.GrandTotal;
                    ledgerPosting.VoucherNo = model.VoucherNo;
                    ledgerPosting.DetailsId = model.SalesReturnMasterId;
                    ledgerPosting.YearId = model.FinancialYearId;
                    ledgerPosting.InvoiceNo = model.VoucherNo;
                    ledgerPosting.VoucherTypeId = model.VoucherTypeId;
                    ledgerPosting.CompanyId = model.CompanyId;
                    ledgerPosting.LongReference = model.Narration;
                    ledgerPosting.ReferenceN = model.Narration;
                    ledgerPosting.ChequeNo = String.Empty;
                    ledgerPosting.ChequeDate = String.Empty;
                    ledgerPosting.AddedDate = DateTime.Now;
                    ledgerPosting.Active = true;
					_context.LedgerPosting.Add(ledgerPosting);
                    await _context.SaveChangesAsync();

                    //SalesAccount Ledger send with out vat
                    LedgerPosting purchasePosting = new LedgerPosting();
                    decimal decSupplierCustomerAmount = Math.Round(model.GrandTotal - model.TotalTax - model.TotalSCCSL, 2);
                    purchasePosting.Date = model.Date;
                    purchasePosting.NepaliDate = String.Empty;
                    purchasePosting.LedgerId = model.AccountTypeId;
                    purchasePosting.Debit = decSupplierCustomerAmount;
                    purchasePosting.Credit = 0;
                    purchasePosting.VoucherNo = model.VoucherNo;
                    purchasePosting.DetailsId = model.SalesReturnMasterId;
                    purchasePosting.YearId = model.FinancialYearId;
                    purchasePosting.InvoiceNo = model.VoucherNo;
                    purchasePosting.VoucherTypeId = model.VoucherTypeId;
                    purchasePosting.CompanyId = model.CompanyId;
                    purchasePosting.LongReference = model.Narration;
                    purchasePosting.ReferenceN = model.Narration;
                    purchasePosting.ChequeNo = String.Empty;
                    purchasePosting.ChequeDate = String.Empty;
                    purchasePosting.AddedDate = DateTime.Now;
                    purchasePosting.Active = true;
					_context.LedgerPosting.Add(purchasePosting);
                    await _context.SaveChangesAsync();

                    //Tax
                    if (model.TotalTax > 0)
                    {
                        LedgerPosting vatPosting = new LedgerPosting();
                        vatPosting.Date = model.Date;
                        vatPosting.NepaliDate = String.Empty;
                        vatPosting.LedgerId = 4;
                        vatPosting.Debit = model.TotalTax;
                        vatPosting.Credit = 0;
                        vatPosting.VoucherNo = model.VoucherNo;
                        vatPosting.DetailsId = model.SalesReturnMasterId;
                        vatPosting.YearId = model.FinancialYearId;
                        vatPosting.InvoiceNo = model.VoucherNo;
                        vatPosting.VoucherTypeId = model.VoucherTypeId;
                        vatPosting.CompanyId = model.CompanyId;
                        vatPosting.LongReference = model.Narration;
                        vatPosting.ReferenceN = model.Narration;
                        vatPosting.ChequeNo = String.Empty;
                        vatPosting.ChequeDate = String.Empty;
                        vatPosting.AddedDate = DateTime.Now;
                        vatPosting.Active = true;
						_context.LedgerPosting.Add(vatPosting);
                        await _context.SaveChangesAsync();
                    }

                //SCCSL
                if (model.TotalSCCSL > 0)
                {
                    LedgerPosting vatPosting = new LedgerPosting();
                    vatPosting.Date = model.Date;
                    vatPosting.NepaliDate = String.Empty;
                    vatPosting.LedgerId = 22;
                    vatPosting.Debit = model.TotalSCCSL;
                    vatPosting.Credit = 0;
                    vatPosting.VoucherNo = model.VoucherNo;
                    vatPosting.DetailsId = model.SalesMasterId;
                    vatPosting.YearId = model.FinancialYearId;
                    vatPosting.InvoiceNo = model.VoucherNo;
                    vatPosting.VoucherTypeId = model.VoucherTypeId;
                    vatPosting.CompanyId = model.CompanyId;
                    vatPosting.LongReference = model.Narration;
                    vatPosting.ReferenceN = model.Narration;
                    vatPosting.ChequeNo = String.Empty;
                    vatPosting.ChequeDate = String.Empty;
                    vatPosting.AddedDate = DateTime.Now;
                    vatPosting.Active = true;
					_context.LedgerPosting.Add(vatPosting);
                    await _context.SaveChangesAsync();
                }
                return true;
                //dbTran.Commit();
            }
            catch (Exception)
            {
                //dbTran.Rollback();
                return false;
            }
        }
        public async Task<bool> UpdateSalesInvoice(SalesReturnMaster model)
        {
            try
            {
                model.Status = "Approved";
                _context.SalesReturnMaster.Update(model);
                await _context.SaveChangesAsync();



                //SalesDetails table
                foreach (var item in model.listOrder)
                {
                    ///AddSalesDetails
                    if (item.SalesReturnDetailsId == 0)
                    {
                        SalesReturnDetails details = new SalesReturnDetails();
                        details.SalesReturnMasterId = model.SalesReturnMasterId;
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
                        details.SalesDetailsId = item.SalesDetailsId;
                        _context.SalesReturnDetails.Add(details);
                        await _context.SaveChangesAsync();
                        int intPurchaseDId = details.SalesReturnDetailsId;

                        StockPosting stockposting = new StockPosting();
                        stockposting.Date = model.Date;
                        stockposting.ProductId = item.ProductId;
                        stockposting.InwardQty = item.Qty;
                        stockposting.OutwardQty = 0;
                        stockposting.UnitId = item.UnitId;
                        stockposting.BatchId = item.BatchId;
                        stockposting.Rate = item.Rate;
                        stockposting.DetailsId = intPurchaseDId;
                        stockposting.InvoiceNo = model.VoucherNo;
                        stockposting.VoucherNo = model.VoucherNo;
                        stockposting.VoucherTypeId = model.VoucherTypeId;
                        stockposting.AgainstInvoiceNo = String.Empty;
                        stockposting.AgainstVoucherNo = String.Empty;
                        stockposting.AgainstVoucherTypeId = 0;
                        stockposting.WarehouseId = model.WarehouseId;
                        stockposting.StockCalculate = "Sales";
                        stockposting.CompanyId = model.CompanyId;
                        stockposting.FinancialYearId = model.FinancialYearId;
                        stockposting.AddedDate = DateTime.Now;
                        _context.StockPosting.Add(stockposting);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        SalesReturnDetails details = new SalesReturnDetails();
                        details.SalesReturnDetailsId = item.SalesReturnDetailsId;
                        details.SalesReturnMasterId = model.SalesReturnMasterId;
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
                        details.SalesDetailsId = item.SalesDetailsId;
                        _context.SalesReturnDetails.Update(details);
                        await _context.SaveChangesAsync();
                        //UpdateStockPosting
                        //GetStockkPostingId
                        var returnstockpostingG = (from progm in _context.StockPosting
                                                   where progm.VoucherTypeId == model.VoucherTypeId && progm.VoucherNo == model.VoucherNo && progm.DetailsId == item.SalesReturnDetailsId
                                                   select progm.StockPostingId).FirstOrDefault();

                        StockPosting stockposting = new StockPosting();
                        stockposting.StockPostingId = returnstockpostingG;
                        stockposting.Date = model.Date;
                        stockposting.ProductId = item.ProductId;
                        stockposting.InwardQty = item.Qty;
                        stockposting.OutwardQty = 0;
                        stockposting.UnitId = item.UnitId;
                        stockposting.BatchId = item.BatchId;
                        stockposting.Rate = item.Rate;
                        stockposting.DetailsId = item.SalesReturnDetailsId;
                        stockposting.InvoiceNo = model.VoucherNo;
                        stockposting.VoucherNo = model.VoucherNo;
                        stockposting.VoucherTypeId = model.VoucherTypeId;
                        stockposting.AgainstInvoiceNo = String.Empty;
                        stockposting.AgainstVoucherNo = String.Empty;
                        stockposting.AgainstVoucherTypeId = 0;
                        stockposting.WarehouseId = model.WarehouseId;
                        stockposting.StockCalculate = "Sales";
                        stockposting.CompanyId = model.CompanyId;
                        stockposting.FinancialYearId = model.FinancialYearId;
                        stockposting.ModifyDate = DateTime.Now;
                        _context.StockPosting.Update(stockposting);
                        await _context.SaveChangesAsync();
                    }
                    
                }

                //DeleteSalesInvoiceLedger
                using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
                {
                    var paraScDelete = new DynamicParameters();
                    paraScDelete.Add("@DetailsId", model.SalesReturnMasterId);
                    paraScDelete.Add("@VoucherTypeId", model.VoucherTypeId);
                    var valueScDelete = sqlcon.Query<int>("DELETE FROM LedgerPosting where DetailsId=@DetailsId AND VoucherTypeId=@VoucherTypeId", paraScDelete, null, true, 0, commandType: CommandType.Text);
                }
                //LedgerPosting
                //Custtomer
                LedgerPosting ledgerPosting = new LedgerPosting();
                ledgerPosting.Date = model.Date;
                ledgerPosting.NepaliDate = String.Empty;
                ledgerPosting.LedgerId = model.LedgerId;
                ledgerPosting.Debit = 0;
                ledgerPosting.Credit = model.GrandTotal;
                ledgerPosting.VoucherNo = model.VoucherNo;
                ledgerPosting.DetailsId = model.SalesReturnMasterId;
                ledgerPosting.YearId = model.FinancialYearId;
                ledgerPosting.InvoiceNo = model.VoucherNo;
                ledgerPosting.VoucherTypeId = model.VoucherTypeId;
                ledgerPosting.CompanyId = model.CompanyId;
                ledgerPosting.LongReference = model.Narration;
                ledgerPosting.ReferenceN = model.Narration;
                ledgerPosting.ChequeNo = String.Empty;
                ledgerPosting.ChequeDate = String.Empty;
                ledgerPosting.AddedDate = DateTime.Now;
                _context.LedgerPosting.Add(ledgerPosting);
                await _context.SaveChangesAsync();

                //SalesAccount Ledger send with out vat
                LedgerPosting purchasePosting = new LedgerPosting();
                decimal decSupplierCustomerAmount = Math.Round(model.GrandTotal - model.TotalTax, 2);
                purchasePosting.Date = model.Date;
                purchasePosting.NepaliDate = String.Empty;
                purchasePosting.LedgerId = 2;
                purchasePosting.Debit = decSupplierCustomerAmount;
                purchasePosting.Credit = 0;
                purchasePosting.VoucherNo = model.VoucherNo;
                purchasePosting.DetailsId = model.SalesReturnMasterId;
                purchasePosting.YearId = model.FinancialYearId;
                purchasePosting.InvoiceNo = model.VoucherNo;
                purchasePosting.VoucherTypeId = model.VoucherTypeId;
                purchasePosting.CompanyId = model.CompanyId;
                purchasePosting.LongReference = model.Narration;
                purchasePosting.ReferenceN = model.Narration;
                purchasePosting.ChequeNo = String.Empty;
                purchasePosting.ChequeDate = String.Empty;
                purchasePosting.AddedDate = DateTime.Now;
                _context.LedgerPosting.Add(purchasePosting);
                await _context.SaveChangesAsync();

                //Tax
                if (model.TotalTax > 0)
                {
                    LedgerPosting vatPosting = new LedgerPosting();
                    vatPosting.Date = model.Date;
                    vatPosting.NepaliDate = String.Empty;
                    vatPosting.LedgerId = 4;
                    vatPosting.Debit = model.TotalTax;
                    vatPosting.Credit = 0;
                    vatPosting.VoucherNo = model.VoucherNo;
                    vatPosting.DetailsId = model.SalesReturnMasterId;
                    vatPosting.YearId = model.FinancialYearId;
                    vatPosting.InvoiceNo = model.VoucherNo;
                    vatPosting.VoucherTypeId = model.VoucherTypeId;
                    vatPosting.CompanyId = model.CompanyId;
                    vatPosting.LongReference = model.Narration;
                    vatPosting.ReferenceN = model.Narration;
                    vatPosting.ChequeNo = String.Empty;
                    vatPosting.ChequeDate = String.Empty;
                    vatPosting.AddedDate = DateTime.Now;
                    _context.LedgerPosting.Add(vatPosting);
                    await _context.SaveChangesAsync();
                }
                foreach (var deleteitem in model.listDelete)
                {
                    SalesReturnDetails x = _context.SalesReturnDetails.Find(deleteitem.DeleteSalesReturnId);
                    _context.SalesReturnDetails.Remove(x);
                    await _context.SaveChangesAsync();
                }
                foreach (var deleteitem in model.listDelete)
                {
                    var returnStockposting = (from ledgerpostingss in _context.StockPosting
                                              where ledgerpostingss.DetailsId == deleteitem.DeleteSalesReturnId && ledgerpostingss.VoucherNo == model.VoucherNo && ledgerpostingss.VoucherTypeId == model.VoucherTypeId
                                              select ledgerpostingss.StockPostingId).FirstOrDefault();
                    StockPosting returnView = _context.StockPosting.Find(returnStockposting);
                    _context.StockPosting.Remove(returnView);
                    _context.SaveChanges();
                }
                return true;
                //dbTran.Commit();
            }
            catch (Exception)
            {
                //dbTran.Rollback();
                return false;
            }
        }
        public async Task<SalesReturnMasterView> SalesReturnInvoiceMasterView(int id)
        {
            var result = await (from a in _context.SalesReturnMaster
                                join b in _context.InvoiceSetting on a.VoucherTypeId equals b.VoucherTypeId
                                join c in _context.AccountLedger on a.LedgerId equals c.LedgerId
                                where a.SalesReturnMasterId == id
                                select new SalesReturnMasterView
                                {
                                    SalesReturnMasterId = a.SalesReturnMasterId,
                                    VoucherNo = a.VoucherNo,
                                    Date = a.Date,
                                    GrandTotal = a.GrandTotal,
                                    BillDiscount = a.BillDiscount,
                                    TotalTax = a.TotalTax,
                                    NetAmounts = a.NetAmounts,
                                    TotalAmount = a.TotalAmount,
                                    Narration = a.Narration,
                                    Status = a.Status,
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

        public async Task<List<ProductView>> SalesReturnInvoiceDetailsView(int id)
        {
            var varlist = (from a in _context.SalesReturnDetails
                           join b in _context.Product on a.ProductId equals b.ProductId
                           join c in _context.Unit on a.UnitId equals c.UnitId
                           where a.SalesReturnMasterId == id
                           select new ProductView
                           {
                               SalesReturnDetailsId = a.SalesReturnDetailsId,
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
                var val = sqlcon.Query<string>("SELECT ISNULL(MAX(SerialNo+1),1) as VoucherNo FROM SalesReturnMaster", null, null, true, 0, commandType: CommandType.Text).FirstOrDefault();
                return val;
            }
        }
    }
}
