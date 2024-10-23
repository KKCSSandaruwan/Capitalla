using Microsoft.EntityFrameworkCore;
using QuickAccounting.Data;
using QuickAccounting.Data.HrPayroll;
using QuickAccounting.Repository.Interface;

namespace QuickAccounting.Repository.Repository
{
    public class TaskService : ITasks
    {
        private readonly ApplicationDbContext _context;
        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Delete(int id)
        {
                Tasks user = await _context.Tasks.FindAsync(id);
                _context.Remove(user);
                await _context.SaveChangesAsync();
                return true;
        }

        public async Task<List<TaskView>> GetAll()
        {
            var result = await(from a in _context.Tasks
                               join b in _context.TaskCategory on a.TaskcategoryId equals b.TaskcategoryId
                               join c in _context.TaskPriority on a.TaskpriorityId equals c.TaskpriorityId
                               select new TaskView
                               {
                                   TaskId = a.TaskId,
                                   TaskName = a.TaskName,
                                   StartDate = a.StartDate,
                                   EndDate = a.EndDate,
                                   Description = a.Description,
                                   CategoryName = b.Name,
                                   PriorityName = c.Name
                               }).ToListAsync();
            return result;
        }

        public async Task<Tasks> GetbyId(int id)
        {
            Tasks model = await _context.Tasks.FindAsync(id);
            return model;
        }

        public async Task<int> Save(Tasks model)
        {
            await _context.Tasks.AddAsync(model);
            await _context.SaveChangesAsync();
            int id = model.TaskId;
            return id;
        }

        public async Task<bool> Update(Tasks model)
        {
            _context.Tasks.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
