using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Framework
{

    public interface IUnitOfWork
    {
        void Commit();
        Task CommitAsync();
    }
}
