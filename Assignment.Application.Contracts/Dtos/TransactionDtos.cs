using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Application.Contracts.Dtos
{
    public class GetTransactionByDateRange
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class TransactionResponseDto
    {
        public string Id { get; set; }
        public string Payment { get; set; }
        public string Status { get; set; }
    }
}
