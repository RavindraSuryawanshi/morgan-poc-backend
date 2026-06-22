using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Morgan.SalesforceSyncPOC.Core.Contracts
{
    /// <summary>
    /// Defines common CRUD operations for a repository.
    /// </summary>
    public interface IRepository<T>
    {
        T Get(int id);
        T Get(string id);
        T Get(long id);
        T Add(T entity);
        T Update(T entity);
        T Delete(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        void SaveChanges();
    }
}
