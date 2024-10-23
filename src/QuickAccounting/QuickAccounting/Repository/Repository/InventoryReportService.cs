using Dapper;
using Microsoft.Data.SqlClient;
using QuickAccounting.Data;
using QuickAccounting.Data.Setting;
using QuickAccounting.Repository.Interface;
using System.Data;

namespace QuickAccounting.Repository.Repository
{
    public class InventoryReportService : IInventoryReport
    {
        private readonly ApplicationDbContext _context;
        private readonly DatabaseConnection _conn;
        DataAccess _da;
        public InventoryReportService(ApplicationDbContext context, DatabaseConnection conn, DataAccess da)
        {
            _context = context;
            _conn = conn;
            _da = da;
        }

        public async Task<IList<PurchaseMasterView>> ItemWisePurchase(DateTime FromDate, DateTime ToDate, int ProductId)
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var para = new DynamicParameters();
                para.Add("@FromDate", FromDate);
                para.Add("@ToDate", ToDate);
                para.Add("@ProductId", ProductId);
                var ListofPlan = sqlcon.Query<PurchaseMasterView>("ItemWisePurchase", para, null, true, 0, commandType: CommandType.StoredProcedure).ToList();
                return ListofPlan;
            }
        }
        public async Task<IList<SalesMasterView>> ItemWiseSales(DateTime FromDate, DateTime ToDate, int ProductId)
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var para = new DynamicParameters();
                para.Add("@FromDate", FromDate);
                para.Add("@ToDate", ToDate);
                para.Add("@ProductId", ProductId);
                var ListofPlan = sqlcon.Query<SalesMasterView>("ItemWiseSales", para, null, true, 0, commandType: CommandType.StoredProcedure).ToList();
                return ListofPlan;
            }
        }
        public async Task<IList<FinancialReport>> CustomerReceiveSummary(DateTime FromDate, DateTime ToDate , int accountGroupId, int ledgerId)
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var para = new DynamicParameters();
                para.Add("@FromDate", FromDate);
                para.Add("@ToDate", ToDate);
                para.Add("@AccountGroupId", accountGroupId);
                para.Add("@LedgerId", ledgerId);
                var ListofPlan = sqlcon.Query<FinancialReport>("CustomerReceiveSummary", para, null, true, 0, commandType: CommandType.StoredProcedure).ToList();
                return ListofPlan;
            }
        }
        public IList<InventoryViewFinal> StockReport(int GroupId, int ProductId, int CompanyId)
        {
            IList<InventoryViewFinal> _UsersModel = new List<InventoryViewFinal>();
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand("StockReport", sqlcon);//call Stored Procedure
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GroupId", GroupId);
            cmd.Parameters.AddWithValue("@ProductId", ProductId);
            cmd.Parameters.AddWithValue("@CompanyId", CompanyId);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                InventoryViewFinal _UserModel = new InventoryViewFinal();
                _UserModel.ProductCode = reader["ProductCode"].ToString();
                _UserModel.ProductName = reader["ProductName"].ToString();
                //_UserModel.batchNo = reader["batchNo"].ToString();
                _UserModel.UnitName = reader["UnitName"].ToString();
                //_UserModel.purchaseRate = Convert.ToDecimal(reader["purchaseRate"].ToString());
                _UserModel.PurQty = Convert.ToDecimal(reader["PurQty"].ToString());
                _UserModel.SalesQty = Convert.ToDecimal(reader["SalesQty"].ToString());
                _UserModel.Rate = Convert.ToDecimal(reader["Rate"].ToString());
                _UserModel.PurchaseStockBal = Convert.ToDecimal(reader["PurchaseStockBal"].ToString());
                _UserModel.SalesStockBalance = Convert.ToDecimal(reader["SalesStockBalance"].ToString());
                _UserModel.Stock = Convert.ToDecimal(reader["Stock"].ToString());
                _UserModel.Stockvalue = Convert.ToDecimal(reader["Stockvalue"].ToString());
                _UserModel.SalesRate = Convert.ToDecimal(reader["SalesRate"].ToString());
                _UsersModel.Add(_UserModel);
            }
            reader.Close();
            sqlcon.Close();
            return _UsersModel;
        }
        public DataSet CustomerLedgerOpening(DateTime fromDate, int LedgerId, int CompanyId)
        {
            DataSet dtbl = new DataSet();
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlDataAdapter sqldadapter = new SqlDataAdapter("CustomerLedgeerOpening", sqlcon);
                sqldadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter sqlparam = new SqlParameter();
                sqlparam = sqldadapter.SelectCommand.Parameters.Add("@fromDate", SqlDbType.DateTime);
                sqlparam.Value = fromDate;
                sqlparam = sqldadapter.SelectCommand.Parameters.Add("@LedgerId", SqlDbType.Int);
                sqlparam.Value = LedgerId;
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
        public DataSet CustomerLedger(DateTime fromDate, DateTime toDate, int LedgerId, int CompanyId)
        {
            DataSet dtbl = new DataSet();
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlDataAdapter sqldadapter = new SqlDataAdapter("CustomerLedgeer", sqlcon);
                sqldadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter sqlparam = new SqlParameter();
                sqlparam = sqldadapter.SelectCommand.Parameters.Add("@fromDate", SqlDbType.DateTime);
                sqlparam.Value = fromDate;
                sqlparam = sqldadapter.SelectCommand.Parameters.Add("@toDate", SqlDbType.DateTime);
                sqlparam.Value = toDate;
                sqlparam = sqldadapter.SelectCommand.Parameters.Add("@LedgerId", SqlDbType.Int);
                sqlparam.Value = LedgerId;
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
        public DashboardView SalesPurchaseTotal( DateTime FromDate, DateTime ToDate, int CompanyId)
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var para = new DynamicParameters();
				para.Add("@FromDate", FromDate);
				para.Add("@ToDate", ToDate);
				para.Add("@CompanyId", CompanyId);
                var returnView = sqlcon.Query<DashboardView>("SalesPurchaseTotal", para, null, true, 0, CommandType.StoredProcedure).SingleOrDefault();
                return returnView;
            }
        }
        public DashboardView Receive(DateTime fromDate, DateTime toDate, int CompanyId)
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var para = new DynamicParameters();
				para.Add("@FromDate", fromDate);
				para.Add("@ToDate", toDate);
				para.Add("@CompanyId", CompanyId);
                var returnView = sqlcon.Query<DashboardView>("Receive", para, null, true, 0, CommandType.StoredProcedure).SingleOrDefault();
                return returnView;
            }
        }
        public DashboardView Pay(DateTime fromDate, DateTime toDate, int CompanyId)
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var para = new DynamicParameters();
				para.Add("@FromDate", fromDate);
				para.Add("@ToDate", toDate);
				para.Add("@CompanyId", CompanyId);
                var returnView = sqlcon.Query<DashboardView>("Pay", para, null, true, 0, CommandType.StoredProcedure).SingleOrDefault();
                return returnView;
            }
        }
        public async Task<IList<FinancialReport>> CashBankBalance(DateTime FromDate, DateTime ToDate)
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var para = new DynamicParameters();
                para.Add("@FromDate", FromDate);
                para.Add("@ToDate", ToDate);
                var ListofPlan = sqlcon.Query<FinancialReport>("CashBankBalance", para, null, true, 0, commandType: CommandType.StoredProcedure).ToList();
                return ListofPlan;
            }
        }
        public async Task<IList<FinancialReport>> SalesChart(DateTime FromDate, DateTime ToDate)
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var para = new DynamicParameters();
                para.Add("@FromDate", FromDate);
                para.Add("@ToDate", ToDate);
                var ListofPlan = sqlcon.Query<FinancialReport>("SalesChart", para, null, true, 0, commandType: CommandType.StoredProcedure).ToList();
                return ListofPlan;
            }
        }
        public async Task<IList<FinancialReport>> PurchaseChart(DateTime FromDate, DateTime ToDate)
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var para = new DynamicParameters();
                para.Add("@FromDate", FromDate);
                para.Add("@ToDate", ToDate);
                var ListofPlan = sqlcon.Query<FinancialReport>("PurchaseChart", para, null, true, 0, commandType: CommandType.StoredProcedure).ToList();
                return ListofPlan;
            }
        }

		public async Task<IList<ProductView>> GetTopProduct()
		{
			using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
			{
				var ListofPlan = sqlcon.Query<ProductView>("GettopProduct", null, null, true, 0, commandType: CommandType.StoredProcedure).ToList();
				return ListofPlan;
			}
		}
		public async Task<IList<FinancialReport>> GetIncomeExpenses(DateTime FromDate, DateTime ToDate)
		{
			using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
			{
				var para = new DynamicParameters();
				para.Add("@FromDate", FromDate);
				para.Add("@ToDate", ToDate);
				var ListofPlan = sqlcon.Query<FinancialReport>("IncomeExpenses", para, null, true, 0, commandType: CommandType.StoredProcedure).ToList();
				return ListofPlan;
			}
		}
	}
}