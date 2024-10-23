using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Authentication;
using QuickAccounting.Data.Setting;
using QuickAccounting.Data.ViewModel;
using QuickAccounting.Repository.Interface;
using System.Data;

namespace QuickAccounting.Repository.Repository
{
    public class PrivilegeService : IPrivilege
    {
        private readonly ApplicationDbContext _context;
        private readonly DatabaseConnection _conn;
        public PrivilegeService(ApplicationDbContext context, DatabaseConnection conn)
        {
            _context = context;
            _conn = conn;
        }

        public async Task<bool> Delete(int RoleId)
        {
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
            }
            SqlCommand cmd = new SqlCommand("DELETE FROM Privilege where RoleId=@RoleId", sqlcon);
            cmd.CommandType = CommandType.Text;
            SqlParameter para = new SqlParameter();
            para = cmd.Parameters.Add("@RoleId", SqlDbType.Int);
            para.Value = RoleId;
            cmd.ExecuteNonQuery();
            sqlcon.Close();
            return true;
        }

        public async Task<IList<Privilege>> GetbyId(int id)
        {
            var varlist = (from a in _context.Privilege
                           where a.RoleId == id
                           select new Privilege
                           {
                               PrivilegeId = a.PrivilegeId,
                               RoleId = a.RoleId,
                               FormName = a.FormName,
                               AddAction = a.AddAction,
                               EditAction = a.EditAction,
                               DeleteAction = a.DeleteAction,
                               ShowAction = a.ShowAction,
                               CompanyId = a.CompanyId,
                               SettingType = a.SettingType,
                               IsActive = a.IsActive,
                               AddedDate = a.AddedDate,
                               ModifyDate = a.ModifyDate
                           }).ToList();

            return varlist;
        }

        public async Task<PrivilegeView> PriviliageCheck(string FormName, string roleNames)
        {
            var result = await (from a in _context.Privilege
                                join b in _context.Role on a.RoleId equals b.RoleId
                                where a.FormName == FormName && b.RoleName == roleNames
                                select new PrivilegeView
                                {
                                    PrivilegeId = a.PrivilegeId,
                                    FormName = a.FormName,
                                    ShowAction = a.ShowAction
                                }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<bool> Save(Privilege model)
        {
            SqlConnection sqlcon = new SqlConnection(_conn.DbConn);
            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
            }
            SqlCommand cmd = new SqlCommand("INSERT INTO Privilege (FormName,AddAction,EditAction,DeleteAction,ShowAction,RoleId,CompanyId,SettingType,IsActive,AddedDate,ModifyDate)VALUES(@FormName,@AddAction,@EditAction,@DeleteAction,@ShowAction,@RoleId,@CompanyId,@SettingType,@IsActive,@AddedDate,@ModifyDate)", sqlcon);
            cmd.CommandType = CommandType.Text;
            SqlParameter para = new SqlParameter();
            para = cmd.Parameters.Add("@FormName", SqlDbType.NVarChar);
            para.Value = model.FormName;
            para = cmd.Parameters.Add("@AddAction", SqlDbType.Bit);
            para.Value = model.AddAction;
            para = cmd.Parameters.Add("@EditAction", SqlDbType.Bit);
            para.Value = model.EditAction;
            para = cmd.Parameters.Add("@DeleteAction", SqlDbType.Bit);
            para.Value = model.DeleteAction;
            para = cmd.Parameters.Add("@ShowAction", SqlDbType.Bit);
            para.Value = model.ShowAction;
            para = cmd.Parameters.Add("@RoleId", SqlDbType.Int);
            para.Value = model.RoleId;
            para = cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
            para.Value = model.CompanyId;
            para = cmd.Parameters.Add("@SettingType", SqlDbType.NVarChar);
            para.Value = model.SettingType;
            para = cmd.Parameters.Add("@IsActive", SqlDbType.Bit);
            para.Value = model.IsActive;
            para = cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime);
            para.Value = model.AddedDate;
            para = cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime);
            para.Value = model.ModifyDate;
            cmd.ExecuteNonQuery();
            sqlcon.Close();
            return true;
        }
        public async Task<Role> GetId(int id)
        {
            Role model = await _context.Role.FindAsync(id);
            return model;
        }
    }
}
