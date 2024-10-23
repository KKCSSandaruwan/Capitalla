using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.HrPayroll;
using QuickAccounting.Repository.Interface;

namespace QuickAccounting.Repository.Repository
{
    public class TaskPriorityService : ITaskPriority
    {
        private readonly ApplicationDbContext _context;
        public TaskPriorityService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CheckName(string name)
        {
            var checkResult = (from progm in _context.TaskPriority
                               where progm.Name == name
                               select progm.TaskpriorityId).Count();
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
            var checkResult = (from progm in _context.TaskPriority
                               where progm.Name == name
                               select progm.TaskpriorityId).Count();
            if (checkResult > 0)
            {

                var checkAccount = (from progm in _context.TaskPriority
                                    where progm.Name == name
                                    select progm.TaskpriorityId).FirstOrDefault();
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
                                    where progm.TaskpriorityId == id
                                    select progm.TaskpriorityId).CountAsync();
            if (checkResult > 0)
            {
                return false;
            }
            else
            {
                TaskPriority user = await _context.TaskPriority.FindAsync(id);
                _context.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<List<TaskPriorityView>> GetAll()
        {
            var result = await(from a in _context.TaskPriority
                               select new TaskPriorityView
                               {
                                   TaskpriorityId = a.TaskpriorityId,
                                   Name = a.Name
                               }).ToListAsync();
            return result;
        }

        public async Task<TaskPriority> GetbyId(int id)
        {
            TaskPriority model = await _context.TaskPriority.FindAsync(id);
            return model;
        }

        public async Task<int> Save(TaskPriority model)
        {
            await _context.TaskPriority.AddAsync(model);
            await _context.SaveChangesAsync();
            int id = model.TaskpriorityId;
            return id;
        }

        public async Task<bool> Update(TaskPriority model)
        {
            _context.TaskPriority.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
