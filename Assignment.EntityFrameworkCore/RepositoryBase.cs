using Assignment.EntityFrameworkCore.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Assignment.EntityFrameworkCore
{
    public abstract class RepositoryBase<T> where T : class, new()
    {
        #region Properties
        private AssignmentDbContext? dataContext;
        private readonly DbSet<T> dbSet;

        protected IDbFactory DbFactory
        {
            get;
            private set;
        }

        protected AssignmentDbContext AssignmentDbContext
        {
            get { return dataContext ??= DbFactory.Init(); }
        }
        #endregion

        protected RepositoryBase(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            dbSet = AssignmentDbContext.Set<T>();
        }

        #region Implementation
        public virtual async Task AddRangeAsync(IEnumerable<T> entityList)
        {
            await dbSet.AddRangeAsync(entityList);
        }

        public virtual async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where)
        {
            return await dbSet.Where(where).ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where, string include)
        {
            return await dbSet.Include(include).Where(where).ToListAsync();
        }
        #endregion
    }
}
