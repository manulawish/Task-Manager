using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TaskManagementApp.Domain.Entities;

namespace TaskManagementApp.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
