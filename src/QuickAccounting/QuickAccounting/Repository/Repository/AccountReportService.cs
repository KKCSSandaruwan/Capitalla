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
    public class AccountReportService : IAccountReport
    {
        private readonly ApplicationDbContext _context;
        private readonly DatabaseConnection _conn;
        DataAccess _da;
        public AccountReportService(ApplicationDbContext context, DatabaseConnection conn, DataAccess da)
        {
            _context = context;
            _conn = conn;
            _da = da;
        }
        public List<FinancialReport> TrailBalance(DateTime fromDate, DateTime toDate, int CompanyId)
        {
            List<SqlParameter> paramColl = new List<SqlParameter>();
            paramColl.Add(new SqlParameter("@fromDate", fromDate));
            paramColl.Add(new SqlParameter("@toDate", toDate));
            paramColl.Add(new SqlParameter("@CompanyId", CompanyId));

            DataTable dtResult = _da.ExecuteDataTable("Trailbalance", paramColl);
            List<FinancialReport> dtoList = new List<FinancialReport>();
            foreach (DataRow dr in dtResult.Rows)
            {
                //RegTypeDTO dto = new RegTypeDTO();
                //dto.RegName = dr["RegName"].ToString();
                //dto.RegTypeID = Convert.ToInt32(dr["RegTypeID"]);
                //dto.Rate = Convert.ToDecimal(dr["Rate"]);
                //dtoList.Add(dto);

                dtoList.Add(new FinancialReport
                {
                    AccountGroupId = Convert.ToInt32(dr["AccountGroupId"]),
                    Name = dr["Name"].ToString(),
                    //Code = dr["Code"].ToString(),
                    OpeningBalance = dr["OpeningBalance"].ToString(),
                    Balance = dr["Balance"].ToString(),
                    //OpeningDr = Convert.ToDecimal(dr["OpeningDr"].ToString()),
                    //OpeningCr = Convert.ToDecimal(dr["OpeningCr"].ToString()),
                    Debit = Convert.ToDecimal(dr["Debit"].ToString()),
                    Credit = Convert.ToDecimal(dr["Credit"].ToString())
                    //ClosingDr = Convert.ToDecimal(dr["ClosingDr"].ToString()),
                    //ClosingCr = Convert.ToDecimal(dr["ClosingCr"].ToString())
                });

            }
            return dtoList;

        }
        public DataSet BalanceSheet(DateTime toDate, int CompanyId)
        {
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            DataSet dset = new DataSet();
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlDataAdapter sdaadapter = new SqlDataAdapter("BalanceSheet", sqlcon);
                sdaadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter prm = new SqlParameter();
                prm = sdaadapter.SelectCommand.Parameters.Add("@toDate", SqlDbType.DateTime);
                prm.Value = toDate;
                prm = sdaadapter.SelectCommand.Parameters.Add("@CompanyId", SqlDbType.Int);
                prm.Value = CompanyId;
                sdaadapter.Fill(dset);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlcon.Close();
            }
            return dset;
        }
        public DataSet BalanceSheetDetailed(DateTime toDate, int CompanyId, int AccountGroupId, bool isAsset)
        {
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            DataSet dset = new DataSet();
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlDataAdapter sdaadapter = new SqlDataAdapter("BalanceSheetDetailed", sqlcon);
                sdaadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter prm = new SqlParameter();
                prm = sdaadapter.SelectCommand.Parameters.Add("@toDate", SqlDbType.DateTime);
                prm.Value = toDate;
                prm = sdaadapter.SelectCommand.Parameters.Add("@CompanyId", SqlDbType.Int);
                prm.Value = CompanyId;
                prm = sdaadapter.SelectCommand.Parameters.Add("@AccountGroupId", SqlDbType.Int);
                prm.Value = AccountGroupId;
                prm = sdaadapter.SelectCommand.Parameters.Add("@isAsset", SqlDbType.Bit);
                prm.Value = isAsset;
                sdaadapter.Fill(dset);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlcon.Close();
            }
            return dset;
        }
        public decimal StockValueGetOnDate(DateTime date, string calculationMethod, bool isOpeningStock, bool isFromBalanceSheet)
        {
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            decimal dcstockValue = 0;
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                object obj = null;
                SqlParameter prm = new SqlParameter();
                SqlCommand sccmd = new SqlCommand();
                if (calculationMethod == "FIFO")
                {
                    if (isOpeningStock)
                    {
                        if (!isFromBalanceSheet)
                        {
                            sccmd = new SqlCommand("StockValueOnDateByFIFOForOpeningStock", sqlcon);
                            prm = sccmd.Parameters.Add("@fromDate", SqlDbType.DateTime);
                            //prm.Value = PublicVariables._dtFromDate;
                            prm.Value = DateTime.Now;
                        }
                        else
                        {
                            sccmd = new SqlCommand("StockValueOnDateByFIFOForOpeningStockForBalancesheet", sqlcon);
                            prm = sccmd.Parameters.Add("@fromDate", SqlDbType.DateTime);
                            //prm = sccmd.Parameters.Add("@date", SqlDbType.DateTime);
                            //prm.Value = PublicVariables._dtToDate;
                            prm.Value = date;
                        }
                    }
                    else
                    {
                        sccmd = new SqlCommand("StockValueOnDateByFIFO", sqlcon);
                    }
                }
                sccmd.CommandType = CommandType.StoredProcedure;
                prm = sccmd.Parameters.Add("@date", SqlDbType.DateTime);
                prm.Value = date;
                obj = sccmd.ExecuteScalar();
                if (obj != null)
                {
                    decimal.TryParse(obj.ToString(), out dcstockValue);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlcon.Close();
            }
            return dcstockValue;
        }
        public DataSet ProfitAndLossAnalysisUpToaDateForBalansheet(DateTime fromDate, DateTime toDate, int CompanyId)
        {
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            DataSet dset = new DataSet();
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlDataAdapter sdaadapter = new SqlDataAdapter("ProfitAndLossAnalysisUpToaDateForBalansheet", sqlcon);
                sdaadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter prm = new SqlParameter();
                prm = sdaadapter.SelectCommand.Parameters.Add("@fromDate", SqlDbType.DateTime);
                prm.Value = fromDate;
                prm = sdaadapter.SelectCommand.Parameters.Add("@toDate", SqlDbType.DateTime);
                prm.Value = toDate;
                prm = sdaadapter.SelectCommand.Parameters.Add("@CompanyId", SqlDbType.Int);
                prm.Value = CompanyId;
                sdaadapter.Fill(dset);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlcon.Close();
            }
            return dset;
        }
        public DataSet ProfitAndLossAnalysisUpToaDateForPreviousYears(DateTime toDate, int CompanyId)
        {
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            DataSet dset = new DataSet();
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlDataAdapter sdaadapter = new SqlDataAdapter("ProfitAndLossAnalysisUpToaDateForPreviousYears", sqlcon);
                sdaadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter prm = new SqlParameter();
                prm = sdaadapter.SelectCommand.Parameters.Add("@fromDate", SqlDbType.DateTime);
                prm.Value = toDate;
                prm = sdaadapter.SelectCommand.Parameters.Add("@CompanyId", SqlDbType.Int);
                prm.Value = CompanyId;
                sdaadapter.Fill(dset);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlcon.Close();
            }
            return dset;
        }
        public DataSet ProfitAndLossAnalysis(DateTime fromDate, DateTime toDate, int CompanyId)
        {
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            DataSet dset = new DataSet();
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlDataAdapter sdaadapter = new SqlDataAdapter("ProfitAndLossAnalysis", sqlcon);
                sdaadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter prm = new SqlParameter();
                prm = sdaadapter.SelectCommand.Parameters.Add("@fromDate", SqlDbType.DateTime);
                prm.Value = fromDate;
                prm = sdaadapter.SelectCommand.Parameters.Add("@toDate", SqlDbType.DateTime);
                prm.Value = toDate;
                prm = sdaadapter.SelectCommand.Parameters.Add("@CompanyId", SqlDbType.Int);
                prm.Value = CompanyId;
                sdaadapter.Fill(dset);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlcon.Close();
            }
            return dset;
        }
        public DataSet ProfitAndLossAnalysisDetailed(DateTime fromDate, DateTime toDate, int CompanyId)
        {
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            DataSet dset = new DataSet();
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlDataAdapter sdaadapter = new SqlDataAdapter("ProfitAndLossAnalysisDetailed", sqlcon);
                sdaadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter prm = new SqlParameter();
                prm = sdaadapter.SelectCommand.Parameters.Add("@fromDate", SqlDbType.DateTime);
                prm.Value = fromDate;
                prm = sdaadapter.SelectCommand.Parameters.Add("@toDate", SqlDbType.DateTime);
                prm.Value = toDate;
                prm = sdaadapter.SelectCommand.Parameters.Add("@CompanyId", SqlDbType.Int);
                prm.Value = CompanyId;
                sdaadapter.Fill(dset);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlcon.Close();
            }
            return dset;
        }
        public decimal StockValueGetOnDate(DateTime date, DateTime dtToDate, string calculationMethod, bool isOpeningStock, bool isFromBalanceSheet)
        {
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            decimal dcstockValue = 0;
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                object obj = null;
                SqlParameter prm = new SqlParameter();
                SqlCommand sccmd = new SqlCommand();
                if (calculationMethod == "FIFO")
                {
                    if (isOpeningStock)
                    {
                        if (!isFromBalanceSheet)
                        {
                            sccmd = new SqlCommand("StockValueOnDateByFIFOForOpeningStock", sqlcon);
                            prm = sccmd.Parameters.Add("@fromDate", SqlDbType.DateTime);
                            prm.Value = dtToDate;
                            //prm.Value = PublicVariables._dtToDate;
                            //prm.Value = PublicVariables._dtFromDate;
                        }
                        else
                        {
                            sccmd = new SqlCommand("StockValueOnDateByFIFOForOpeningStockForBalancesheet", sqlcon);
                            //prm = sccmd.Parameters.Add("@fromDate", SqlDbType.DateTime);
                            //prm = sccmd.Parameters.Add("@date", SqlDbType.DateTime);
                            prm = sccmd.Parameters.Add("@date", SqlDbType.DateTime);
                            //prm.Value = PublicVariables._dtToDate;
                            prm.Value = dtToDate;
                            //prm.Value = PublicVariables._dtToDate;
                        }
                    }
                    else
                    {
                        sccmd = new SqlCommand("StockValueOnDateByFIFO", sqlcon);
                    }
                }
                //else if (calculationMethod == "Average Cost")
                //{
                //    if (isOpeningStock)
                //    {
                //        if (!isFromBalanceSheet)
                //        {
                //            sccmd = new SqlCommand("StockValueOnDateByAVCOForOpeningStock", sqlcon);
                //        }
                //        else
                //        {
                //            sccmd = new SqlCommand("StockValueOnDateByAVCOForOpeningStockForBalanceSheet", sqlcon);
                //        }
                //        prm = sccmd.Parameters.Add("@fromDate", SqlDbType.DateTime);
                //        prm.Value = PublicVariables._dtToDate;
                //    }
                //    else
                //    {
                //        sccmd = new SqlCommand("StockValueOnDateByAVCO", sqlcon);
                //    }
                //}
                //else if (calculationMethod == "High Cost")
                //{
                //    if (isOpeningStock)
                //    {
                //        if (!isFromBalanceSheet)
                //        {
                //            sccmd = new SqlCommand("StockValueOnDateByHighCostForOpeningStock", sqlcon);
                //        }
                //        else
                //        {
                //            sccmd = new SqlCommand("StockValueOnDateByHighCostForOpeningStockBlncSheet", sqlcon);
                //        }
                //        prm = sccmd.Parameters.Add("@fromDate", SqlDbType.DateTime);
                //        prm.Value = PublicVariables._dtFromDate;
                //    }
                //    else
                //    {
                //        sccmd = new SqlCommand("StockValueOnDateByHighCost", sqlcon);
                //    }
                //}
                //else if (calculationMethod == "Low Cost")
                //{
                //    if (isOpeningStock)
                //    {
                //        if (!isFromBalanceSheet)
                //        {
                //            sccmd = new SqlCommand("StockValueOnDateByLowCostForOpeningStock", sqlcon);
                //        }
                //        else
                //        {
                //            sccmd = new SqlCommand("StockValueOnDateByLowCostForOpeningStockForBlncSheet", sqlcon);
                //        }
                //        prm = sccmd.Parameters.Add("@fromDate", SqlDbType.DateTime);
                //        prm.Value = PublicVariables._dtFromDate;
                //    }
                //    else
                //    {
                //        sccmd = new SqlCommand("StockValueOnDateByLowCost", sqlcon);
                //    }
                //}
                //else if (calculationMethod == "Last Purchase Rate")
                //{
                //    if (isOpeningStock)
                //    {
                //        if (!isFromBalanceSheet)
                //        {
                //            sccmd = new SqlCommand("StockValueOnDateByLastPurchaseRateForOpeningStock", sqlcon);
                //        }
                //        else
                //        {
                //            sccmd = new SqlCommand("StockValueOnDateByLastPurchaseRateForOpeningStockBlncSheet", sqlcon);
                //        }
                //        prm = sccmd.Parameters.Add("@fromDate", SqlDbType.DateTime);
                //        prm.Value = PublicVariables._dtFromDate;
                //    }
                //    else
                //    {
                //        sccmd = new SqlCommand("StockValueOnDateByLastPurchaseRate", sqlcon);
                //    }
                //}
                sccmd.CommandType = CommandType.StoredProcedure;
                prm = sccmd.Parameters.Add("@date", SqlDbType.DateTime);
                prm.Value = date;
                obj = sccmd.ExecuteScalar();
                if (obj != null)
                {
                    decimal.TryParse(obj.ToString(), out dcstockValue);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlcon.Close();
            }
            return dcstockValue;
        }
        public DataSet DayBook(DateTime fromDate, DateTime toDate, int VoucherTypeId, int LedgerId)
        {
            DataSet dtbl = new DataSet();
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlDataAdapter sqldadapter = new SqlDataAdapter("DayBook", sqlcon);
                sqldadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter sqlparam = new SqlParameter();
                sqlparam = sqldadapter.SelectCommand.Parameters.Add("@fromDate", SqlDbType.DateTime);
                sqlparam.Value = fromDate;
                sqlparam = sqldadapter.SelectCommand.Parameters.Add("@toDate", SqlDbType.DateTime);
                sqlparam.Value = toDate;
                sqlparam = sqldadapter.SelectCommand.Parameters.Add("@VoucherTypeId", SqlDbType.Int);
                sqlparam.Value = VoucherTypeId;
                sqlparam = sqldadapter.SelectCommand.Parameters.Add("@LedgerId", SqlDbType.Int);
                sqlparam.Value = LedgerId;
                sqldadapter.Fill(dtbl);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlcon.Close();
            }
            return dtbl;
        }
        public DataSet LedgercountReport(DateTime fromDate, DateTime toDate, int LedgerId, string LedgerName, int CompanyId)
        {
            DataSet dtbl = new DataSet();
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlDataAdapter sqldadapter = new SqlDataAdapter("LedgercountReport_Test", sqlcon);
                sqldadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter sqlparam = new SqlParameter();
                sqlparam = sqldadapter.SelectCommand.Parameters.Add("@fromDate", SqlDbType.DateTime);
                sqlparam.Value = fromDate;
                sqlparam = sqldadapter.SelectCommand.Parameters.Add("@toDate", SqlDbType.DateTime);
                sqlparam.Value = toDate;
                sqlparam = sqldadapter.SelectCommand.Parameters.Add("@LedgerId", SqlDbType.Int);
                sqlparam.Value = LedgerId;
                sqlparam = sqldadapter.SelectCommand.Parameters.Add("@ledgerName", SqlDbType.NVarChar);
                sqlparam.Value = LedgerName;
                sqlparam = sqldadapter.SelectCommand.Parameters.Add("@CompanyId", SqlDbType.Int);
                sqlparam.Value = CompanyId;
                sqldadapter.Fill(dtbl);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlcon.Close();
            }
            return dtbl;
        }
    }
}
