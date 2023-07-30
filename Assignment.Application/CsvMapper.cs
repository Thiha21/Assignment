using Assignment.Domain.Entities;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Application
{
    public class CsvMapper : ClassMap<Transaction>
    {
        public CsvMapper()
        {
            Map(m => m.TransactionId).Index(0);
            Map(m => m.Amount).Index(1).TypeConverter<DecimalConverter>();
            Map(m => m.CurrencyCode).Index(2);
            Map(m => m.TransactionDate).Index(3).TypeConverter<DateTimeConverter>();
            Map(m => m.Status).Index(4);
        }
    }
}
