using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;
using QuickAccounting.Data.ViewModel;
using QuickAccounting.Enums;
using QuickAccounting.Repository.Interface;
using System.Data;
using static MudBlazor.CategoryTypes;

namespace QuickAccounting.Repository.Repository
{
    public class PaymentOutService : IPaymentOut
    {
        private readonly ApplicationDbContext _context;
        private readonly DatabaseConnection _conn;
        private static int PaymentMasterId = 0;

        public PaymentOutService(ApplicationDbContext context, DatabaseConnection conn)
        {
            _context = context;
            _conn = conn;
        }
        public async Task<bool> Delete(PaymentMaster master)
        {
            try
            {
                //GetBalance
                var result = await (from a in _context.PaymentDetails
                                    where a.PaymentMasterId == master.PaymentMasterId
                                    select new
                                    {
                                        PurchaseMasterId = a.PurchaseMasterId,
                                        ReceiveAmount = a.ReceiveAmount
                                    }).ToListAsync();
                if (result != null)
                {
                    foreach (var item in result)
                    {
                        PurchaseMaster purchase = new PurchaseMaster();
                        using (SqlConnection sqlconn = new SqlConnection(_conn.DbConn))
                        {
                            var paras = new DynamicParameters();
                            paras.Add("@PurchaseMasterId", item.PurchaseMasterId);
                            purchase = sqlconn.Query<PurchaseMaster>("SELECT *FROM PurchaseMaster where PurchaseMasterId=@PurchaseMasterId", paras, null, true, 0, commandType: CommandType.Text).FirstOrDefault();
                        }
                        if (purchase != null)
                        {
							//// Approved -->  Start
							//decimal decPay = purchase.PayAmount;
                            //purchase.PayAmount = item.ReceiveAmount - decPay;
                            //purchase.PreviousDue = (purchase.GrandTotal) - (item.ReceiveAmount - decPay);
                            //purchase.BalanceDue = (purchase.GrandTotal) - (item.ReceiveAmount - decPay);
							//// Approved --> End
							///
							decimal decPay = purchase.PayAmount;
							purchase.PreviousDue = (purchase.GrandTotal) - decPay;
							purchase.BalanceDue = (purchase.GrandTotal) - decPay;

							if (purchase.BalanceDue == 0)
                            {
                                purchase.Status = "Paid";
                            }
                            else
                            {
                                purchase.Status = "Partial";
                            }
                            _context.PurchaseMaster.Update(purchase);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
                    if (sqlcon.State == ConnectionState.Closed)
                    {
                        sqlcon.Open();
                    }
                    SqlCommand cmd = new SqlCommand("PaymentOutDelete", sqlcon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter para = new SqlParameter();
                    para = cmd.Parameters.Add("@PaymentMasterId", SqlDbType.Int);
                    para.Value = master.PaymentMasterId;
                    para = cmd.Parameters.Add("@VoucherTypeId", SqlDbType.Int);
                    para.Value = master.VoucherTypeId;
                    int rowAffacted = cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IList<PaymentReceiveView>> GetAll(DateTime FromDate,DateTime ToDate, string VoucherNo, string type)
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var para = new DynamicParameters();
                para.Add("@FromDate", FromDate);
                para.Add("@ToDate", ToDate);
                para.Add("@VoucherNo", VoucherNo);
                para.Add("@Type", type);
                var ListofPlan = sqlcon.Query<PaymentReceiveView>("PaymentViewAll", para, null, true, 0, commandType: CommandType.StoredProcedure).ToList();
                return ListofPlan;
            }
        }

        public async Task<PaymentMaster> GetbyId(int id)
        {
            PaymentMaster model = await _context.PaymentMaster.FindAsync(id);
            return model;
        }

        public async Task<string> GetSerialNo()
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var val = sqlcon.Query<string>("SELECT ISNULL(MAX(SerialNo+1),1) as VoucherNo FROM PaymentMaster", null, null, true, 0, commandType: CommandType.Text).FirstOrDefault();
                return val;
            }
        }

        public async Task<PaymentReceiveView> PaymentOutView(int id)
        {
            var result = await (from a in _context.PaymentMaster
                                join c in _context.AccountLedger on a.LedgerId equals c.LedgerId
                                where a.PaymentMasterId == id
                                select new PaymentReceiveView
                                {
                                    PaymentMasterId = a.PaymentMasterId,
                                    Date = a.Date,
                                    VoucherNo = a.VoucherNo,
                                    Amount = a.Amount,
                                    Narration = a.Narration,
                                    PaymentType = a.PaymentType,
                                    Type = a.Type,
                                    UserId = a.UserId,
                                    AddedDate = a.AddedDate,
                                    LedgerName = c.LedgerName,
                                    Email = c.Email,
                                    Address = c.Address
                                }).FirstOrDefaultAsync();
            return result;
        }
        public async Task<IList<PaymentReceiveView>> PaymentOutDetailsView(int id)
        {
            var result = await (from a in _context.PaymentDetails
                                join c in _context.AccountLedger on a.LedgerId equals c.LedgerId
                                where a.PaymentMasterId == id
                                select new PaymentReceiveView
                                {
                                    PaymentDetailsId = a.PaymentDetailsId,
                                    PaymentMasterId = a.PaymentMasterId,
                                    LedgerId = a.LedgerId,
                                    Amount = a.TotalAmount,
                                    DueAmount = a.DueAmount,
                                    ReceiveAmount = a.ReceiveAmount,
                                    PurchaseMasterId = a.PurchaseMasterId,
                                    LedgerName = c.LedgerName
                                }).ToListAsync();
            return result;
        }
        public async Task<IList<PaymentDetailsView>> PaymentDetailsAllView(int id)
        {
            var result = await (from a in _context.PaymentDetails
                                join c in _context.AccountLedger on a.LedgerId equals c.LedgerId
                                where a.PaymentMasterId == id
                                select new PaymentDetailsView
                                {
                                    Id = id + 1,
                                    PaymentDetailsId = a.PaymentDetailsId,
                                    PaymentMasterId = a.PaymentMasterId,
                                    LedgerId = a.LedgerId,
                                    TotalAmount = a.TotalAmount,
                                    DueAmount = a.DueAmount,
                                    ReceiveAmount = a.ReceiveAmount,
                                    PurchaseMasterId = a.PurchaseMasterId,
                                    LedgerName = c.LedgerName
                                }).ToListAsync();
            return result;
        }

        public async Task<int> Save(PaymentMaster model)
        {
            //_context.PaymentMaster.Add(model);
            //await _context.SaveChangesAsync();
            //int id = model.PaymentMasterId;

            // Insert a new payment record for settling multiple invoices (PaymentMasterDup Table)
            PaymentMasterDup paymentMaster = new PaymentMasterDup
            {
                LedgerId = model.LedgerId,
                AccountId = 0,
                PaymentType = PaymentMethod.Supplier.ToString(),
                SettlmentType = SettlmentType.Single.ToString(),
                InvoiceType = InvoiceType.Purchase.ToString(),
                FinancialYearId = model.FinancialYearId,
                CompanyId = model.CompanyId,
                UserId = model.UserId,
                Narration = model.Narration,
                AddedDate = DateTime.Now
            };

            _context.PaymentMasterDup.Add(paymentMaster);
            await _context.SaveChangesAsync();
            int generatedPaymentMasterId = PaymentMasterId =  paymentMaster.PaymentMasterId;



            //PaymentDetails table
            PaymentDetailsDup paymentDetail = new PaymentDetailsDup
            {
                PaymentMasterId = generatedPaymentMasterId,
                PurchaseMasterId = model.PurchaseMasterId,
                TotalAmount = model.listOrder.First().TotalAmount,
                PaidAmount = model.listOrder.First().ReceiveAmount,
                //DueAmount = model.listOrder.First().DueAmount,
                DueAmount = model.listOrder.First().TotalAmount - model.listOrder.First().ReceiveAmount,
                PaymentStatus = PaymentStatus.Unpaid.ToString(),
                AddedDate = DateTime.Now,
            };

            _context.PaymentDetailsDup.Add(paymentDetail);
            await _context.SaveChangesAsync();

            // After saving, use the generated PaymentDetailId to create SerialNo and VoucherNo
            paymentDetail.SerialNo = paymentDetail.PaymentDetailId.ToString("D8");
            paymentDetail.VoucherNo = InvoiceStatus.Draft.ToString();

            // Update the record with the new SerialNo and VoucherNo
            _context.PaymentDetailsDup.Update(paymentDetail);
            await _context.SaveChangesAsync();



            //foreach (var item in model.listOrder)
            //{
            //    //AddPaymentDetails
            //    PaymentDetails details = new PaymentDetails();
            //    if (item.LedgerId > 0)
            //    {
            //        details.PaymentMasterId = generatedPaymentMasterId;
            //        details.LedgerId = item.LedgerId;
            //        details.PurchaseMasterId = item.PurchaseMasterId;
            //        details.TotalAmount = item.TotalAmount;
            //        details.ReceiveAmount = item.ReceiveAmount;
            //        details.DueAmount = item.DueAmount;
            //        _context.PaymentDetails.Add(details);
            //        await _context.SaveChangesAsync();
            //        int intPurchaseDId = details.PaymentDetailsId;
            //    }
            //}
            return generatedPaymentMasterId;
        }

        public async Task<bool> ApprovedOk(PaymentMaster model)
        {
            try
            {


				var currentPaymentDetail = await (from pm in _context.PaymentDetailsDup
												  where pm.PaymentMasterId == PaymentMasterId
                                                  select pm).FirstOrDefaultAsync();

				// After saving, use the generated PaymentDetailId to create SerialNo and VoucherNo
				currentPaymentDetail.SerialNo = currentPaymentDetail.PaymentDetailId.ToString("D8");
				currentPaymentDetail.VoucherNo = $"PAY{currentPaymentDetail.SerialNo}OUT";
				if (currentPaymentDetail.DueAmount == 0)
				{
					currentPaymentDetail.PaymentStatus = "Paid";
				}
				else
				{
					currentPaymentDetail.PaymentStatus = "Partial";
				}

				// Update the record with the new SerialNo and VoucherNo
				_context.PaymentDetailsDup.Update(currentPaymentDetail);
				await _context.SaveChangesAsync();



                // Update the invoice details with the settlement information (PurchaseMaster Table)
                var currentInvoice = await (from pm in _context.PurchaseMaster
                                            where pm.PurchaseMasterId == currentPaymentDetail.PurchaseMasterId
                                            select pm).FirstOrDefaultAsync();

                if (currentInvoice == null)
                    throw new KeyNotFoundException($"Invoice with ID {currentPaymentDetail.PurchaseMasterId} not found.");

                currentInvoice.PayAmount = currentPaymentDetail.PaidAmount;
                currentInvoice.BalanceDue = currentPaymentDetail.DueAmount;
                //currentInvoice.PreviousDue = invoiceToSettle.PreviousDue;
                currentInvoice.Status = currentPaymentDetail.PaymentStatus;
                //currentInvoice.Reference = invoiceToSettle.Reference;
                currentInvoice.ModifyDate = DateTime.Now;

                _context.PurchaseMaster.Update(currentInvoice);
                await _context.SaveChangesAsync();



                //CashAndBank
                LedgerPosting cashPosting = new LedgerPosting();
				cashPosting.Date = model.Date;
				cashPosting.NepaliDate = String.Empty;
				cashPosting.LedgerId = model.LedgerId;
				cashPosting.Debit = 0;
				cashPosting.Credit = model.Amount;
				cashPosting.VoucherNo = model.VoucherNo;
				cashPosting.DetailsId = model.PaymentMasterId;
				cashPosting.YearId = model.FinancialYearId;
				cashPosting.InvoiceNo = model.VoucherNo;
				cashPosting.VoucherTypeId = model.VoucherTypeId;
				cashPosting.CompanyId = model.CompanyId;
				cashPosting.LongReference = model.Narration;
				cashPosting.ReferenceN = model.Narration;
				cashPosting.ChequeNo = String.Empty;
				cashPosting.ChequeDate = String.Empty;
				cashPosting.AddedDate = DateTime.UtcNow;
				cashPosting.Active = true;
				_context.LedgerPosting.Add(cashPosting);
				await _context.SaveChangesAsync();

				//LedgerPosting
				//Supplier
				LedgerPosting ledgerPosting = new LedgerPosting();
				ledgerPosting.Date = model.Date;
				ledgerPosting.NepaliDate = String.Empty;
				ledgerPosting.LedgerId = model.LedgerId;
				ledgerPosting.Debit = model.Amount;
				ledgerPosting.Credit = 0;
				ledgerPosting.VoucherNo = model.VoucherNo;
				ledgerPosting.DetailsId = model.PaymentMasterId;
				ledgerPosting.YearId = model.FinancialYearId;
				ledgerPosting.InvoiceNo = model.VoucherNo;
				ledgerPosting.VoucherTypeId = model.VoucherTypeId;
				ledgerPosting.CompanyId = model.CompanyId;
				ledgerPosting.LongReference = model.Narration;
				ledgerPosting.ReferenceN = model.Narration;
				ledgerPosting.ChequeNo = String.Empty;
				ledgerPosting.ChequeDate = String.Empty;
				ledgerPosting.AddedDate = DateTime.UtcNow;
				ledgerPosting.Active = true;
				_context.LedgerPosting.Add(ledgerPosting);
				await _context.SaveChangesAsync();
				return true;


				//_context.PaymentMaster.Update(model);
				//            await _context.SaveChangesAsync();
				//            //PaymentDetails table
				//            foreach (var item in model.listOrder)
				//            {
				//                //AddPaymentDetails
				//                PaymentDetails details = new PaymentDetails();
				//                if (model.Amount > 0)
				//                {
				//                    details.PaymentDetailsId = item.PaymentDetailsId;
				//                    details.PaymentMasterId = model.PaymentMasterId;
				//                    details.LedgerId = item.LedgerId;
				//                    details.PurchaseMasterId = item.PurchaseMasterId;
				//                    details.TotalAmount = item.TotalAmount;
				//                    details.ReceiveAmount = item.ReceiveAmount;
				//                    details.DueAmount = item.DueAmount;
				//                    _context.PaymentDetails.Update(details);
				//                    await _context.SaveChangesAsync();

				//                    if (model.PurchaseMasterId > 0)
				//                    {
				//                        PurchaseMaster master = new PurchaseMaster();
				//                        using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
				//                        {
				//                            var para = new DynamicParameters();
				//                            para.Add("@PurchaseMasterId", item.PurchaseMasterId);
				//                            master = sqlcon.Query<PurchaseMaster>("SELECT *FROM PurchaseMaster where PurchaseMasterId=@PurchaseMasterId", para, null, true, 0, commandType: CommandType.Text).FirstOrDefault();
				//                        }
				//                        decimal decPay = master.PayAmount;
				//                        master.PayAmount = item.ReceiveAmount + decPay;
				//                        master.PreviousDue = (master.GrandTotal) - (item.ReceiveAmount + decPay);
				//                        master.BalanceDue = (master.GrandTotal) - (item.ReceiveAmount + decPay);
				//                        if (master.BalanceDue == 0)
				//                        {
				//                            master.Status = "Paid";
				//                        }
				//                        else
				//                        {
				//                            master.Status = "Partial";
				//                        }
				//                        _context.PurchaseMaster.Update(master);
				//                        await _context.SaveChangesAsync();
				//                    }


				//                    //CashAndBank
				//                    LedgerPosting cashPosting = new LedgerPosting();
				//                    cashPosting.Date = model.Date;
				//                    cashPosting.NepaliDate = String.Empty;
				//                    cashPosting.LedgerId = item.LedgerId;
				//                    cashPosting.Debit = 0;
				//                    cashPosting.Credit = model.Amount;
				//                    cashPosting.VoucherNo = model.VoucherNo;
				//                    cashPosting.DetailsId = model.PaymentMasterId;
				//                    cashPosting.YearId = model.FinancialYearId;
				//                    cashPosting.InvoiceNo = model.VoucherNo;
				//                    cashPosting.VoucherTypeId = model.VoucherTypeId;
				//                    cashPosting.CompanyId = model.CompanyId;
				//                    cashPosting.LongReference = model.Narration;
				//                    cashPosting.ReferenceN = model.Narration;
				//                    cashPosting.ChequeNo = String.Empty;
				//                    cashPosting.ChequeDate = String.Empty;
				//                    cashPosting.AddedDate = DateTime.UtcNow;
				//                    cashPosting.Active = true;
				//		_context.LedgerPosting.Add(cashPosting);
				//                    await _context.SaveChangesAsync();
				//                }
				//            }



			}
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Update(PaymentMaster model)
        {
            try
            {
                _context.PaymentMaster.Update(model);
                await _context.SaveChangesAsync();

                //DeletePaymetLedger
                using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
                {
                    var paraScDelete = new DynamicParameters();
                    paraScDelete.Add("@DetailsId", model.PaymentMasterId);
                    paraScDelete.Add("@VoucherTypeId", model.VoucherTypeId);
                    var valueScDelete = sqlcon.Query<int>("DELETE FROM LedgerPosting where DetailsId=@DetailsId AND VoucherTypeId=@VoucherTypeId", paraScDelete, null, true, 0, commandType: CommandType.Text);
                }
                //PaymentDetails table
                foreach (var item in model.listOrder)
                {
                    //AddPaymentDetails
                    PaymentDetails details = new PaymentDetails();
                    if (item.PaymentDetailsId == 0)
                    {
                        details.PaymentMasterId = model.PaymentMasterId;
                        details.LedgerId = item.LedgerId;
                        details.TotalAmount = item.TotalAmount;
                        details.ReceiveAmount = item.ReceiveAmount;
                        details.DueAmount = item.DueAmount;
                        _context.PaymentDetails.Add(details);
                        await _context.SaveChangesAsync();

                        //PostingCashBankLedger
                        LedgerPosting cashPosting = new LedgerPosting();
                        cashPosting.Date = model.Date;
                        cashPosting.NepaliDate = String.Empty;
                        cashPosting.LedgerId = item.LedgerId;
                        cashPosting.Debit = 0;
                        cashPosting.Credit = item.TotalAmount;
                        cashPosting.VoucherNo = model.VoucherNo;
                        cashPosting.DetailsId = model.PaymentMasterId;
                        cashPosting.YearId = model.FinancialYearId;
                        cashPosting.InvoiceNo = model.VoucherNo;
                        cashPosting.VoucherTypeId = model.VoucherTypeId;
                        cashPosting.CompanyId = model.CompanyId;
                        cashPosting.LongReference = model.Narration;
                        cashPosting.ReferenceN = model.Narration;
                        cashPosting.ChequeNo = String.Empty;
                        cashPosting.ChequeDate = String.Empty;
                        cashPosting.AddedDate = DateTime.UtcNow;
                        _context.LedgerPosting.Add(cashPosting);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        details.PaymentDetailsId = item.PaymentDetailsId;
                        details.PaymentMasterId = model.PaymentMasterId;
                        details.LedgerId = item.LedgerId;
                        details.TotalAmount = item.TotalAmount;
                        details.ReceiveAmount = item.ReceiveAmount;
                        details.DueAmount = item.DueAmount;
                        _context.PaymentDetails.Update(details);
                        await _context.SaveChangesAsync();

                        //PostingCashBankLedger
                        LedgerPosting cashPosting = new LedgerPosting();
                        cashPosting.Date = model.Date;
                        cashPosting.NepaliDate = String.Empty;
                        cashPosting.LedgerId = item.LedgerId;
                        cashPosting.Debit = 0;
                        cashPosting.Credit = item.TotalAmount;
                        cashPosting.VoucherNo = model.VoucherNo;
                        cashPosting.DetailsId = model.PaymentMasterId;
                        cashPosting.YearId = model.FinancialYearId;
                        cashPosting.InvoiceNo = model.VoucherNo;
                        cashPosting.VoucherTypeId = model.VoucherTypeId;
                        cashPosting.CompanyId = model.CompanyId;
                        cashPosting.LongReference = model.Narration;
                        cashPosting.ReferenceN = model.Narration;
                        cashPosting.ChequeNo = String.Empty;
                        cashPosting.ChequeDate = String.Empty;
                        cashPosting.AddedDate = DateTime.UtcNow;
                        _context.LedgerPosting.Add(cashPosting);
                        await _context.SaveChangesAsync();
                    }
                }

                //LedgerPosting
                //Supplier
                LedgerPosting ledgerPosting = new LedgerPosting();
                ledgerPosting.Date = model.Date;
                ledgerPosting.NepaliDate = String.Empty;
                ledgerPosting.LedgerId = model.LedgerId;
                ledgerPosting.Debit = model.Amount;
                ledgerPosting.Credit = 0;
                ledgerPosting.VoucherNo = model.VoucherNo;
                ledgerPosting.DetailsId = model.PaymentMasterId;
                ledgerPosting.YearId = model.FinancialYearId;
                ledgerPosting.InvoiceNo = model.VoucherNo;
                ledgerPosting.VoucherTypeId = model.VoucherTypeId;
                ledgerPosting.CompanyId = model.CompanyId;
                ledgerPosting.LongReference = model.Narration;
                ledgerPosting.ReferenceN = model.Narration;
                ledgerPosting.ChequeNo = String.Empty;
                ledgerPosting.ChequeDate = String.Empty;
                ledgerPosting.AddedDate = DateTime.UtcNow;
                _context.LedgerPosting.Add(ledgerPosting);
                await _context.SaveChangesAsync();

                foreach (var deleteitem in model.listDelete)
                {
                    PaymentDetails x = _context.PaymentDetails.Find(deleteitem.PaymentDetailsId);
                    _context.PaymentDetails.Remove(x);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
