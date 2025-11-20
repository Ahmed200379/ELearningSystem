using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
   public interface IUnitOfWork
    {
       Task<int> SaveChanges();
       IGeneticRepo<T> GetRepository<T>() where T : Base;
    }
}
