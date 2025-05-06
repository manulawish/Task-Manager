using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Application.Interfaces;
using TaskManagementApp.Infrastructure.Persistence;
using TaskManagementApp.Infrastructure.Repositories;

namespace TaskManagementApp.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private ITaskRepository? _taskRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public ITaskRepository TaskRepository => _taskRepository ??= new TaskRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
