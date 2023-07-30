using Assignment.Domain.Entities;
using Assignment.Framework.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Domain.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
    }
}
