using Morgan.SalesforceSyncPOC.Core.Contracts;
using Morgan.SalesforceSyncPOC.Infrastrcture.Data;
using System.Linq.Expressions;

namespace Morgan.SalesforceSyncPOC.Infrastrcture.Repositories
{
    /// <summary>
    /// Base repository implementation for common CRUD operations.
    /// </summary>
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _dbContext;
        protected BaseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual T Add(T entity)
        {
            return _dbContext.Add(entity).Entity;
        }

        public virtual T Get(int id)
        {
            return _dbContext.Find<T>(id);
        }

        public virtual T Get(string id)
        {
            return _dbContext.Find<T>(id);
        }

        public virtual T Get(long id)
        {
            return _dbContext.Find<T>(id);

        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbContext.Set<T>().ToList();
        }
        public virtual T Delete(int id)
        {
            return _dbContext.Remove(Get(id))?.Entity;
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().AsQueryable().Where(predicate).ToList();
        }

        public virtual void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public virtual T Update(T entity)
        {
            return _dbContext.Update(entity).Entity;
        }
    }
}
