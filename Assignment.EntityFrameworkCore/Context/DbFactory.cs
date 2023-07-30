using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.EntityFrameworkCore.Context
{
    public interface IDbFactory : IDisposable
    {
        AssignmentDbContext Init();
    }

    public class DbFactory : Disposable, IDbFactory
    {
        readonly AssignmentDbContext assignmentDbContext;

        public DbFactory(AssignmentDbContext pgContext)
        {
            assignmentDbContext = pgContext;
        }

        public AssignmentDbContext Init()
        {
            return assignmentDbContext;
        }

        protected override void DisposeCore()
        {
            if (assignmentDbContext != null)
                assignmentDbContext.Dispose();
        }
    }
}
