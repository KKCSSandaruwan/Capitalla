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
    public class PurchaseReturnService : IPurchaseReturn
    {
        private readonly ApplicationDbContext _context;
        private readonly DatabaseConnection _conn;
        public PurchaseReturnService(ApplicationDbContext context, DatabaseConnection conn)
        {
            _context = context;
            _conn = conn;
        }
        public async Task<bool> CheckName(string name)
        {
            var checkResult = (from progm in _context.PurchaseReturnMaster
                               where progm.VoucherNo == name
                               select progm.PurchaseReturnMasterId).Count();
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
            var checkResult = (from progm in _context.PurchaseReturnMaster
                               where progm.VoucherNo == name
                               select progm.PurchaseReturnMasterId).Count();
            if (checkResult > 0)
            {

                var checkAccount = (from progm in _context.PurchaseReturnMaster
                                    where progm.VoucherNo == name
                                    select progm.PurchaseReturnMasterId).FirstOrDefault();
                return checkAccount;
            }
            else
            {
                return 0;
            }
        }

        public async Task<bool> Delete(PurchaseReturnMaster master)
        {
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            try
            {

                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand cmd = new SqlCommand("PurchaseReturnInvoiceDelete", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter para = new SqlParameter();
                para = cmd.Parameters.Add("@PurchaseReturnMasterId", SqlDbType.Int);
                para.Value = master.PurchaseReturnMasterId;
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
        public async Task<List<PurchaseReturnMasterView>> GetAll()
        {
            var result = await (from a in _context.PurchaseReturnMaster
                                join b in _context.InvoiceSetting on a.VoucherTypeId equals b.VoucherTypeId
                                join c in _context.AccountLedger on a.LedgerId equals c.LedgerId
                                select new PurchaseReturnMasterView
                                {
                                    PurchaseReturnMasterId = a.PurchaseReturnMasterId,
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
        public async Task<List<PurchaseReturnMasterView>> PurchaseReturnInvoiceSearch(DateTime FromDate, DateTime ToDate, string VoucherNo, int LedgerId)
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var para = new DynamicParameters();
                para.Add("@FromDate", FromDate);
                para.Add("@ToDate", ToDate);
                para.Add("@VoucherNo", VoucherNo);
                para.Add("@LedgerId", LedgerId);
                var ListofPlan = sqlcon.Query<PurchaseReturnMasterView>("PurchaseReturnInvoiceSearch", para, null, true, 0, commandType: CommandType.StoredProcedure).ToList();
                return ListofPlan;
            }
        }

        public async Task<PurchaseReturnMaster> GetbyId(int id)
        {
            PurchaseReturnMaster model = await _context.PurchaseReturnMaster.FindAsync(id);
            return model;
        }

        public async Task<int> Save(PurchaseReturnMaster model)
        {
            _context.PurchaseReturnMaster.Add(model);
            await _context.SaveChangesAsync();
            int id = model.PurchaseReturnMasterId;



            //PurchaseDetails table
            foreach (var item in model.listOrder)
            {
                //AddPurchaseDetails
                PurchaseReturnDetails details = new PurchaseReturnDetails();
                details.PurchaseReturnMasterId = id;
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
                details.PurchaseReturnDetailsId = item.PurchaseReturnDetailsId;
                _context.PurchaseReturnDetails.Add(details);
                await _context.SaveChangesAsync();
                int intPurchaseDId = details.PurchaseReturnDetailsId;
                //AddStockPosting

                
            }
            return id;
        }

        public async Task<bool> Update(PurchaseReturnMaster model)
        {
            try
            {
                _context.PurchaseReturnMaster.Update(model);
                await _context.SaveChangesAsync();



                //PurchaseDetails table
                foreach (var item in model.listOrder)
                {
                    //AddPurchaseDetails
                    if (item.PurchaseReturnDetailsId == 0)
                    {
                        PurchaseReturnDetails details = new PurchaseReturnDetails();
                        details.PurchaseReturnMasterId = model.PurchaseReturnMasterId;
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
                        details.PurchaseDetailsId = item.PurchaseDetailsId;
                        _context.PurchaseReturnDetails.Add(details);
                        await _context.SaveChangesAsync();
                        int intPurchaseDId = details.PurchaseReturnDetailsId;
                    }
                    else
                    {
                        PurchaseReturnDetails details = new PurchaseReturnDetails();
                        details.PurchaseReturnDetailsId = item.PurchaseReturnDetailsId;
                        details.PurchaseReturnMasterId = model.PurchaseReturnMasterId;
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
                        details.PurchaseDetailsId = item.PurchaseDetailsId;
                        _context.PurchaseReturnDetails.Update(details);
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
        public async Task<bool> Approved(PurchaseReturnMaster model)
        {
            try
            {
                model.Status = "Approved";
                _context.PurchaseReturnMaster.Update(model);
                await _context.SaveChangesAsync();



                //PurchaseDetails table
                foreach (var item in model.listOrder)
                {
                    //AddPurchaseDetails
                        //AddStockPosting

                        StockPosting stockposting = new StockPosting();
                        stockposting.Date = model.Date;
                        stockposting.ProductId = item.ProductId;
                        stockposting.InwardQty = 0;
                        stockposting.OutwardQty = item.Qty;
                        stockposting.UnitId = item.UnitId;
                        stockposting.BatchId = item.BatchId;
                        stockposting.Rate = item.Rate;
                        stockposting.DetailsId = item.PurchaseReturnDetailsId;
                        stockposting.InvoiceNo = model.VoucherNo;
                        stockposting.VoucherNo = model.VoucherNo;
                        stockposting.VoucherTypeId = model.VoucherTypeId;
                        stockposting.AgainstInvoiceNo = String.Empty;
                        stockposting.AgainstVoucherNo = String.Empty;
                        stockposting.AgainstVoucherTypeId = 0;
                        stockposting.WarehouseId = model.WarehouseId;
                        stockposting.StockCalculate = "Purchase";
                        stockposting.CompanyId = model.CompanyId;
                        stockposting.FinancialYearId = model.FinancialYearId;
                        stockposting.AddedDate = DateTime.Now;
                        stockposting.Active = true;
						_context.StockPosting.Add(stockposting);
                        await _context.SaveChangesAsync();
                }


                    //LedgerPosting
                    //Supplier
                    LedgerPosting ledgerPosting = new LedgerPosting();
                    ledgerPosting.Date = model.Date;
                    ledgerPosting.NepaliDate = String.Empty;
                    ledgerPosting.LedgerId = model.LedgerId;
                    ledgerPosting.Debit = model.GrandTotal;
                    ledgerPosting.Credit = 0;
                    ledgerPosting.VoucherNo = model.VoucherNo;
                    ledgerPosting.DetailsId = model.PurchaseReturnMasterId;
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

                    //PurchaseAccount Ledger send with out vat
                    LedgerPosting purchasePosting = new LedgerPosting();
                    decimal decSupplierCustomerAmount = Math.Round(model.GrandTotal - model.TotalTax, 2);
                    purchasePosting.Date = model.Date;
                    purchasePosting.NepaliDate = String.Empty;
                    purchasePosting.LedgerId = 3;
                    purchasePosting.Debit = 0;
                    purchasePosting.Credit = decSupplierCustomerAmount;
                    purchasePosting.VoucherNo = model.VoucherNo;
                    purchasePosting.DetailsId = model.PurchaseReturnMasterId;
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
                        vatPosting.Debit = 0;
                        vatPosting.Credit = model.TotalTax;
                        vatPosting.VoucherNo = model.VoucherNo;
                        vatPosting.DetailsId = model.PurchaseReturnMasterId;
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
        public async Task<bool> UpdatePurchaseInvoice(PurchaseReturnMaster model)
        {
            try
            {
                model.Status = "Approved";
                _context.PurchaseReturnMaster.Update(model);
                await _context.SaveChangesAsync();



                //PurchaseDetails table
                foreach (var item in model.listOrder)
                {
                    ///AddPurchaseDetails
                    if (item.PurchaseReturnDetailsId == 0)
                    {
                        PurchaseReturnDetails details = new PurchaseReturnDetails();
                        details.PurchaseReturnMasterId = model.PurchaseReturnMasterId;
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
                        details.PurchaseDetailsId = item.PurchaseDetailsId;
                        _context.PurchaseReturnDetails.Add(details);
                        await _context.SaveChangesAsync();
                        int intPurchaseDId = details.PurchaseReturnDetailsId;

                        StockPosting stockposting = new StockPosting();
                        stockposting.Date = model.Date;
                        stockposting.ProductId = item.ProductId;
                        stockposting.InwardQty = 0;
                        stockposting.OutwardQty = item.Qty;
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
                        stockposting.StockCalculate = "Purchase";
                        stockposting.CompanyId = model.CompanyId;
                        stockposting.FinancialYearId = model.FinancialYearId;
                        stockposting.AddedDate = DateTime.Now;
                        _context.StockPosting.Add(stockposting);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        PurchaseReturnDetails details = new PurchaseReturnDetails();
                        details.PurchaseReturnDetailsId = item.PurchaseReturnDetailsId;
                        details.PurchaseReturnMasterId = model.PurchaseReturnMasterId;
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
                        details.PurchaseDetailsId = item.PurchaseDetailsId;
                        _context.PurchaseReturnDetails.Update(details);
                        await _context.SaveChangesAsync();
                        //UpdateStockPosting
                        //GetStockkPostingId
                        var returnstockpostingG = (from progm in _context.StockPosting
                                                   where progm.VoucherTypeId == model.VoucherTypeId && progm.VoucherNo == model.VoucherNo && progm.DetailsId == item.PurchaseReturnDetailsId
                                                   select progm.StockPostingId).FirstOrDefault();

                        StockPosting stockposting = new StockPosting();
                        stockposting.StockPostingId = returnstockpostingG;
                        stockposting.Date = model.Date;
                        stockposting.ProductId = item.ProductId;
                        stockposting.InwardQty = 0;
                        stockposting.OutwardQty = item.Qty;
                        stockposting.UnitId = item.UnitId;
                        stockposting.BatchId = item.BatchId;
                        stockposting.Rate = item.Rate;
                        stockposting.DetailsId = item.PurchaseReturnDetailsId;
                        stockposting.InvoiceNo = model.VoucherNo;
                        stockposting.VoucherNo = model.VoucherNo;
                        stockposting.VoucherTypeId = model.VoucherTypeId;
                        stockposting.AgainstInvoiceNo = String.Empty;
                        stockposting.AgainstVoucherNo = String.Empty;
                        stockposting.AgainstVoucherTypeId = 0;
                        stockposting.WarehouseId = model.WarehouseId;
                        stockposting.StockCalculate = "Purchase";
                        stockposting.CompanyId = model.CompanyId;
                        stockposting.FinancialYearId = model.FinancialYearId;
                        stockposting.ModifyDate = DateTime.Now;
                        _context.StockPosting.Update(stockposting);
                        await _context.SaveChangesAsync();
                    }
                    
                }

                //DeletePurchaseInvoiceLedger
                using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
                {
                    var paraScDelete = new DynamicParameters();
                    paraScDelete.Add("@DetailsId", model.PurchaseReturnMasterId);
                    paraScDelete.Add("@VoucherTypeId", model.VoucherTypeId);
                    var valueScDelete = sqlcon.Query<int>("DELETE FROM LedgerPosting where DetailsId=@DetailsId AND VoucherTypeId=@VoucherTypeId", paraScDelete, null, true, 0, commandType: CommandType.Text);
                }
                //LedgerPosting
                //Supplier
                LedgerPosting ledgerPosting = new LedgerPosting();
                ledgerPosting.Date = model.Date;
                ledgerPosting.NepaliDate = String.Empty;
                ledgerPosting.LedgerId = model.LedgerId;
                ledgerPosting.Debit = model.GrandTotal;
                ledgerPosting.Credit = 0;
                ledgerPosting.VoucherNo = model.VoucherNo;
                ledgerPosting.DetailsId = model.PurchaseReturnMasterId;
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

                //PurchaseAccount Ledger send with out vat
                LedgerPosting purchasePosting = new LedgerPosting();
                decimal decSupplierCustomerAmount = Math.Round(model.GrandTotal - model.TotalTax, 2);
                purchasePosting.Date = model.Date;
                purchasePosting.NepaliDate = String.Empty;
                purchasePosting.LedgerId = 3;
                purchasePosting.Debit = 0;
                purchasePosting.Credit = decSupplierCustomerAmount;
                purchasePosting.VoucherNo = model.VoucherNo;
                purchasePosting.DetailsId = model.PurchaseReturnMasterId;
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
                    vatPosting.Debit = 0;
                    vatPosting.Credit = model.TotalTax;
                    vatPosting.VoucherNo = model.VoucherNo;
                    vatPosting.DetailsId = model.PurchaseReturnMasterId;
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
                    PurchaseReturnDetails x = _context.PurchaseReturnDetails.Find(deleteitem.DeletePurchaseReturnId);
                    _context.PurchaseReturnDetails.Remove(x);
                    await _context.SaveChangesAsync();
                }
                foreach (var deleteitem in model.listDelete)
                {
                    var returnStockposting = (from ledgerpostingss in _context.StockPosting
                                              where ledgerpostingss.DetailsId == deleteitem.DeletePurchaseReturnId && ledgerpostingss.VoucherNo == model.VoucherNo && ledgerpostingss.VoucherTypeId == model.VoucherTypeId
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
        public async Task<PurchaseReturnMasterView> PurchaseReturnInvoiceMasterView(int id)
        {
            var result = await (from a in _context.PurchaseReturnMaster
                                join b in _context.InvoiceSetting on a.VoucherTypeId equals b.VoucherTypeId
                                join c in _context.AccountLedger on a.LedgerId equals c.LedgerId
                                where a.PurchaseReturnMasterId == id
                                select new PurchaseReturnMasterView
                                {
                                    PurchaseReturnMasterId = a.PurchaseReturnMasterId,
                                    VoucherNo = a.VoucherNo,
                                    Date = a.Date,
                                    GrandTotal = a.GrandTotal,
                                    BillDiscount = a.BillDiscount,
                                    TotalTax = a.TotalTax,
                                    TotalAmount = a.TotalAmount,
                                    Narration = a.Narration,
                                    Status = a.Status,
                                    VoucherTypeName = b.VoucherTypeName,
                                    LedgerName = c.LedgerName,
                                    Email = c.Email,
                                    Phone = c.Phone,
                                    Address = c.Address,
                                    AddedDate = a.AddedDate
                                }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<ProductView>> PurchaseReturnInvoiceDetailsView(int id)
        {
            var varlist = (from a in _context.PurchaseReturnDetails
                           join b in _context.Product on a.ProductId equals b.ProductId
                           join c in _context.Unit on a.UnitId equals c.UnitId
                           where a.PurchaseReturnMasterId == id
                           select new ProductView
                           {
                               PurchaseReturnDetailsId = a.PurchaseReturnDetailsId,
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
                var val = sqlcon.Query<string>("SELECT ISNULL(MAX(SerialNo+1),1) as VoucherNo FROM PurchaseReturnMaster", null, null, true, 0, commandType: CommandType.Text).FirstOrDefault();
                return val;
            }
        }
    }
}
