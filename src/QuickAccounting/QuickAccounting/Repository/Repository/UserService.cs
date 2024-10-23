using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.Authentication;
using QuickAccounting.Data.ViewModel;
using QuickAccounting.Repository.Interface;

namespace QuickAccounting.Repository.Services
{
    public class UserService : IUser
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CheckName(string name)
        {
            var checkResult = (from progm in _context.UserMaster
                               where progm.Email == name
                               select progm.UserId).Count();
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
            var checkResult = (from progm in _context.UserMaster
                               where progm.Email == name
                               select progm.UserId).Count();
            if (checkResult > 0)
            {

                var checkAccount = (from progm in _context.UserMaster
                                    where progm.Email == name
                                    select progm.UserId).FirstOrDefault();
                return checkAccount;
            }
            else
            {
                return 0;
            }
        }

        public async Task<bool> Delete(int id)
        {
                UserMaster user = await _context.UserMaster.FindAsync(id);
                _context.Remove(user);
                await _context.SaveChangesAsync();
                return true;
        }

        public async Task<List<UserView>> GetAll()
        {
            var result = await (from a in _context.UserMaster
                                join b in _context.Role on a.RoleId equals b.RoleId
                                select new UserView
                                {
                                    UserId = a.UserId,
                                    UserName = a.UserName,
                                    Email = a.Email,
                                    Phone = a.Phone,
                                    Active = a.Active,
                                    Role = b.RoleName,
                                    Image = a.Image
                                }).ToListAsync();
            return result;
        }
        public async Task<UserMaster> UserProfile(string email)
        {
            var result = await (from a in _context.UserMaster
                                where a.Email == email
                                select new UserMaster
                                {
                                    UserId = a.UserId,
                                    UserName = a.UserName,
                                    Email = a.Email,
                                    Password = a.Password,
                                    ConfirmPassword = a.ConfirmPassword,
                                    Phone = a.Phone,
                                    Active = a.Active,
                                    RoleId = a.RoleId,
                                    Image = a.Image,
                                    AddedDate = a.AddedDate,
                                    ModifyDate = a.ModifyDate
                                }).FirstOrDefaultAsync();
            return result;
        }
        public async Task<List<Role>> GetAllRole()
        {
            var result = await (from a in _context.Role
                                select new Role
                                {
                                    RoleId = a.RoleId,
                                    RoleName = a.RoleName
                                }).ToListAsync();
            return result;
        }

        public async Task<UserMaster> GetbyId(int id)
        {
            UserMaster model = await _context.UserMaster.FindAsync(id);
            return model;
        }

        public async Task<int> Save(UserMaster model)
        {
            await _context.UserMaster.AddAsync(model);
            await _context.SaveChangesAsync();
            int id = model.UserId;
            return id;
        }

        public async Task<bool> Update(UserMaster model)
        {
            _context.UserMaster.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
