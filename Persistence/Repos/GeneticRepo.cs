using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repos
{
    public class GeneticRepo<T> : IGeneticRepo<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GeneticRepo(AppDbContext context,DbSet<T> dbSet)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T item)
        {
             await _dbSet.AddAsync(item);
        }

        public async Task AddRangeAsync(IEnumerable<T> items)
        {
            await _dbSet.AddRangeAsync(items);
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }

        public void Delete(T item)
        {
            _dbSet.Remove(item);
        }

        public void DeleteRange(IEnumerable<T> items)
        {
            _dbSet.RemoveRange(items);
        }

        public async Task<T?> FindBy(Expression<Func<T, bool>> expression)
        {
           return await _dbSet.FirstOrDefaultAsync(expression);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
          return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsyncs(Expression<Func<T, bool>>? predicate = null, bool asNoTracking = false, int? PageNumber = null, int? PageSize = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (PageNumber.HasValue && PageSize.HasValue)
            {
                query = query.Skip((PageNumber.Value - 1) * PageSize.Value).Take(PageSize.Value);
            }
            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(string key)
        {
            return await _dbSet.FindAsync(key);
        }

        public async Task<T?> GetFirstOrDefault(Expression<Func<T, bool>>? predicate = null, bool asNoTracking = false, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return await query.FirstOrDefaultAsync()!;
        }

        public void Update(T item)
        {
            _dbSet.Update(item);
        }

        public void UpdateRange(IEnumerable<T> items)
        {
             _dbSet.UpdateRange(items);
        }
    }
}
