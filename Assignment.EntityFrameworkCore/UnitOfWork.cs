using Assignment.EntityFrameworkCore.Context;
using Assignment.Framework;

namespace Assignment.EntityFrameworkCore
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory dbFactory;
        private AssignmentDbContext? dbContext;

        public UnitOfWork(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public AssignmentDbContext DbContext
        {
            get { return dbContext ??= dbFactory.Init(); }
        }

        public void Commit()
        {
            DbContext.Commit();
        }

        public async Task CommitAsync()
        {
            await DbContext.CommitAsync();
        }
    }
}
