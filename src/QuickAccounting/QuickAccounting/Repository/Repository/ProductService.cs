using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Inventory;
using QuickAccounting.Data.Setting;
using QuickAccounting.Data.ViewModel;
using QuickAccounting.Repository.Interface;
using System.Data;

namespace QuickAccounting.Repository.Repository
{
    public class ProductService : IProduct
    {
        private readonly ApplicationDbContext _context;
        private readonly DatabaseConnection _conn;
        public ProductService(ApplicationDbContext context, DatabaseConnection conn)
        {
            _context = context;
            _conn = conn;
        }
        public async Task<bool> CheckName(string name)
        {
            var checkResult = (from progm in _context.Product
                               where progm.ProductName == name
                               select progm.ProductId).Count();
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
            var checkResult = (from progm in _context.Product
                               where progm.ProductName == name
                               select progm.ProductId).Count();
            if (checkResult > 0)
            {

                var checkAccount = (from progm in _context.Product
                                    where progm.ProductName == name
                                    select progm.ProductId).FirstOrDefault();
                return checkAccount;
            }
            else
            {
                return 0;
            }
        }

        public async Task<bool> Delete(int ProductId)
        {
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand cmd = new SqlCommand("ProductDelete", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter para = new SqlParameter();
                para = cmd.Parameters.Add("@ProductId", SqlDbType.Int);
                para.Value = ProductId;
                long rowAffacted = cmd.ExecuteNonQuery();
                if (rowAffacted > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                sqlcon.Close();
            }
        }
        public async Task<List<ProductView>> ProductWithStock()
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var ListofPlan = sqlcon.Query<ProductView>("ProductWithStock", null, null, true, 0, commandType: CommandType.StoredProcedure).ToList();
                return ListofPlan;
            }
        }
        public async Task<List<ProductView>> ProductWithCategory(int id)
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var para = new DynamicParameters();
                para.Add("@GroupId", id);
                var ListofPlan = sqlcon.Query<ProductView>("ProductWithGroup", para, null, true, 0, commandType: CommandType.StoredProcedure).ToList();
                return ListofPlan;
            }
        }
        public async Task<List<ProductView>> GetAll()
        {
            var result = await(from a in _context.Product
                               join b in _context.Brand on a.BrandId equals b.BrandId
                               join c in _context.ProductGroup on a.GroupId equals c.GroupId
                               join d in _context.Unit on a.UnitId equals d.UnitId
                               select new ProductView
                               {
                                   ProductId = a.ProductId,
                                   ProductCode = a.ProductCode,
                                   ProductName = a.ProductName,
                                   PurchaseRate = a.PurchaseRate,
                                   SalesRate = a.SalesRate,
                                   Mrp = a.Mrp,
                                   BrandName = b.Name,
                                   GroupName = c.GroupName,
                                   UnitName = d.UnitName,
                                   Image = a.Image
                               }).ToListAsync();
            return result;
        }

        public async Task<Product> GetbyId(int id)
        {
            Product model = await _context.Product.FindAsync(id);
            return model;
        }

        public string GetProductNo()
        {
            using (SqlConnection sqlcon = new SqlConnection(_conn.DbConn))
            {
                var val = sqlcon.Query<string>("SELECT ISNULL(MAX(ProductCode+1),1) as ProductCode FROM Product", null, null, true, 0, commandType: CommandType.Text).FirstOrDefault();
                return val;
            }
        }

        public async Task<int> Save(Product model)
        {
            await _context.Product.AddAsync(model);
            await _context.SaveChangesAsync();
            int id = model.ProductId;
            //PostingOpeningStock
            if (model.OpeningStock > 0)
            {
                StockPosting stockposting = new StockPosting();
                stockposting.Date = model.ExiparyDate;
                stockposting.ProductId = id;
                stockposting.InwardQty = model.OpeningStock;
                stockposting.OutwardQty = 0;
                stockposting.UnitId = model.UnitId;
                stockposting.BatchId = 1;
                stockposting.Rate = model.PurchaseRate;
                stockposting.DetailsId = id;
                stockposting.InvoiceNo = id.ToString();
                stockposting.VoucherNo = id.ToString();
                stockposting.VoucherTypeId = 2;
                stockposting.AgainstInvoiceNo = String.Empty;
                stockposting.AgainstVoucherNo = String.Empty;
                stockposting.AgainstVoucherTypeId = 0;
                stockposting.WarehouseId = 1;
                stockposting.StockCalculate = "OpeningStock";
                stockposting.CompanyId = 1;
                stockposting.FinancialYearId = 1;
                stockposting.AddedDate = DateTime.UtcNow;
                _context.StockPosting.Add(stockposting);
                await _context.SaveChangesAsync();
            }
            return id;
        }

        public async Task<bool> Update(Product model)
        {
            _context.Product.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
