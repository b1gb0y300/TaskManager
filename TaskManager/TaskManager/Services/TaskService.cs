using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.Services
{
    public class TaskService : ITaskService
    {
        public async Task<List<TaskItem>> GetAllAsync()
        {
            using var db = new AppDbContext();
            return await db.Tasks
                .OrderBy(t => t.IsCompleted) // сначала невыполненные
                .ThenBy(t => t.DueDate)      // потом по дедлайну
                .ToListAsync();
        }

        public async Task AddAsync(TaskItem task)
        {
            using var db = new AppDbContext();
            db.Tasks.Add(task);
            await db.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskItem task)
        {
            using var db = new AppDbContext();
            db.Tasks.Update(task);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(TaskItem task)
        {
            using var db = new AppDbContext();
            db.Tasks.Remove(task);
            await db.SaveChangesAsync();
        }
    }
}
