using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repos
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Dictionary<string, object> _repositories;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        public IGeneticRepo<T> GetRepository<T>() where T : Base
        {
            var type = typeof(T).Name;
            if (!_repositories.ContainsKey(type))
            {
                return (IGeneticRepo<T>) _repositories[type];
            }
            var repo = new GeneticRepo<T>(_context, _context.Set<T>());
            _repositories[type] = repo;
            return repo;
        }
    }
}
