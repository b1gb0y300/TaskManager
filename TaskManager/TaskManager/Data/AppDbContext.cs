using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using System;
using System.IO;

namespace TaskManager.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<TaskItem> Tasks { get; set; }

        private readonly string _dbPath;

        public AppDbContext()
        {
            // Папка вида: C:\Users\ТВОЁ_ИМЯ\AppData\Local
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _dbPath = Path.Combine(folder, "taskmanager.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_dbPath}");
        }
    }
}
