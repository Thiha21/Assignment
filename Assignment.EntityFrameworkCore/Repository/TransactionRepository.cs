using Assignment.Domain.Entities;
using Assignment.Domain.Repositories;
using Assignment.EntityFrameworkCore.Context;

namespace Assignment.EntityFrameworkCore.Repository
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(IDbFactory dbFactory) : base(dbFactory) { }

    }
}
