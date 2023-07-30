using Assignment.Application.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Application.Contracts.ServiceInterfaces
{
    public interface ITransactionService
    {
        Task<List<TransactionResponseDto>> GetByCurrencyCode(string currencyCode);
        Task<List<TransactionResponseDto>> GetTransactionByDateRange(GetTransactionByDateRange getTransactionByDateRange);
        Task<List<TransactionResponseDto>> GetByStatus(string status);
    }
}
