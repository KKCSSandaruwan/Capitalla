using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.HrPayroll;
using QuickAccounting.Repository.Interface;

namespace QuickAccounting.Repository.Repository
{
    public class TaskCategoryService : ITaskCategory
    {
        private readonly ApplicationDbContext _context;
        public TaskCategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CheckName(string name)
        {
            var checkResult = (from progm in _context.TaskCategory
                               where progm.Name == name
                               select progm.TaskcategoryId).Count();
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
            var checkResult = (from progm in _context.TaskCategory
                               where progm.Name == name
                               select progm.TaskcategoryId).Count();
            if (checkResult > 0)
            {

                var checkAccount = (from progm in _context.TaskCategory
                                    where progm.Name == name
                                    select progm.TaskcategoryId).FirstOrDefault();
                return checkAccount;
            }
            else
            {
                return 0;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var checkResult = await(from progm in _context.Tasks
                                    where progm.TaskcategoryId == id
                                    select progm.TaskcategoryId).CountAsync();
            if (checkResult > 0)
            {
                return false;
            }
            else
            {
                TaskCategory user = await _context.TaskCategory.FindAsync(id);
                _context.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<List<TaskCategoryView>> GetAll()
        {
            var result = await(from a in _context.TaskCategory
                               select new TaskCategoryView
                               {
                                   TaskcategoryId = a.TaskcategoryId,
                                   Name = a.Name
                               }).ToListAsync();
            return result;
        }

        public async Task<TaskCategory> GetbyId(int id)
        {
            TaskCategory model = await _context.TaskCategory.FindAsync(id);
            return model;
        }

        public async Task<int> Save(TaskCategory model)
        {
            await _context.TaskCategory.AddAsync(model);
            await _context.SaveChangesAsync();
            int id = model.TaskcategoryId;
            return id;
        }

        public async Task<bool> Update(TaskCategory model)
        {
            _context.TaskCategory.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
