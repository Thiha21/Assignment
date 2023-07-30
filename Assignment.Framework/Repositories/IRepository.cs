using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Framework.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where);
        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where, string include);
    }

}
