using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
   public interface IGeneticRepo<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T item);
        Task AddRangeAsync(IEnumerable<T> items);
        void Update(T item);
        void UpdateRange(IEnumerable<T> items);
        void Delete(T item);
        void DeleteRange(IEnumerable<T> items);
        Task<T> GetByIdAsync(string key);
        Task<IEnumerable<T>> GetAllAsyncs(
            Expression<Func<T, bool>>? predicate=null,
            bool asNoTracking = false,
            int? PageNumber = null,
            int? PageSize = null,
            params Expression<Func<T, object>>[] includes
            );
        Task<T> GetFirstOrDefault(
            Expression<Func<T,bool>>? predicate = null,
            bool asNoTracking = false,
            params Expression<Func<T, object>>[] includes
            );
        bool Any(Expression<Func<T, bool>> predicate);
        Task<T> FindBy(Expression<Func<T, bool>> expression);

    }
}
