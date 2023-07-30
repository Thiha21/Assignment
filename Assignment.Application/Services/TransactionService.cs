using Assignment.Application.Contracts.Dtos;
using Assignment.Application.Contracts.ServiceInterfaces;
using Assignment.Domain.Entities;
using Assignment.Domain.Repositories;
using Assignment.Framework;
using Assignment.Framework.Helper;
using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Assignment.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionRepository _transactionRepository;
        public TransactionService(IMapper mapper, IUnitOfWork unitOfWork, ITransactionRepository transactionRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;   
            _transactionRepository = transactionRepository;
        }

        public async Task<List<TransactionResponseDto>> GetByCurrencyCode(string currencyCode)
        {
            try
            {
                var tranList = await _transactionRepository.GetManyAsync(x => x.CurrencyCode.ToLower() == currencyCode.Trim().ToLower()); 
                if (tranList == null)
                    throw new AppException("Transaction does not exist");
                var tranListViewModel = _mapper.Map<List<TransactionResponseDto>>(tranList);
                return tranListViewModel;
            }
            catch(Exception ex)
            {
                Log.Error($"{ex.Message}\r\n{ex.StackTrace}");
                throw new AppException(ex.Message);
            }
        }

        public async Task<List<TransactionResponseDto>> GetByStatus(string status)
        {
            try 
            {
                List<string> statusList = new();
                switch (status.ToLower())
                {
                    case "done": statusList.Add(status.ToLower());statusList.Add("finished");break;
                    case "rejected": statusList.Add(status.ToLower());statusList.Add("failed");break;
                    case "finished": statusList.Add(status.ToLower());statusList.Add("done");break;
                    case "failed": statusList.Add(status.ToLower());statusList.Add("rejected");break;
                    default: statusList.Add(status.ToLower());break;
                }
                var tranList = await _transactionRepository.GetManyAsync(x => statusList.Contains(x.Status.ToLower()));
                if (tranList == null)
                    throw new AppException("Transaction does not exist");
                var tranListViewModel = _mapper.Map<List<TransactionResponseDto>>(tranList);
                return tranListViewModel;
            }
            catch(Exception ex)
            {
                Log.Error($"{ex.Message}\r\n{ex.StackTrace}");
                throw new AppException(ex.Message);
            }
}

        public async Task<List<TransactionResponseDto>> GetTransactionByDateRange(GetTransactionByDateRange getTransactionByDateRange)
        {
            try 
            { 
                var tranList = await _transactionRepository.GetManyAsync(x => x.TransactionDate.Date >= getTransactionByDateRange.From.Date
                                                                            && x.TransactionDate.Date <= getTransactionByDateRange.To.Date);
                if (tranList == null)
                    throw new AppException("Transaction does not exist");
                var tranListViewModel = _mapper.Map<List<TransactionResponseDto>>(tranList);
                return tranListViewModel;
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}\r\n{ex.StackTrace}");
                throw new AppException(ex.Message);
            }
        }

        public async Task UploadFile(IFormFile file)
        {
            try
            {
                List<Transaction> transactions = new List<Transaction>();
                if (file.FileName.EndsWith(".csv"))
                {
                    transactions = ParseCsv(file);
                }
                else
                {
                    transactions = ParseXml(file);
                }
                CheckMandatoryFieldsInTransaction(transactions);
                await _transactionRepository.AddRangeAsync(transactions);
                _unitOfWork.Commit();
            }
            catch (AppException ex)
            {
                Log.Error($"{ex.Message}");
                throw new AppException(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}\r\n{ex.StackTrace}\r\n{ex.InnerException}");
                throw ex;
            }
        }

        private List<Transaction> CheckMandatoryFieldsInTransaction(List<Transaction> tranList)
        {
            string errorMessage = string.Empty;
            if (tranList.Any(x => string.IsNullOrWhiteSpace(x.TransactionId)))
            {
                errorMessage = ((errorMessage == string.Empty) ? "" : errorMessage + ", ") + "TransactionId Is Required";
            }
            if (tranList.Any(x => string.IsNullOrEmpty(x.Amount.ToString())))
            {
                errorMessage = ((errorMessage == string.Empty) ? "" : errorMessage + ", ") + "Amount Is Required";
            }
            if (tranList.Any(x => string.IsNullOrWhiteSpace(x.CurrencyCode)))
            {
                errorMessage = ((errorMessage == string.Empty) ? "" : errorMessage + ", ") + "CurrencyCode Is Required";
            }
            if (tranList.Any(x => string.IsNullOrWhiteSpace(x.TransactionDate.ToString())))
            {
                errorMessage = ((errorMessage == string.Empty) ? "" : errorMessage + ", ") + "TransactionDate Is Required";
            }
            if (tranList.Any(x => string.IsNullOrWhiteSpace(x.Status)))
            {
                errorMessage = ((errorMessage == string.Empty) ? "" : errorMessage + ", ") + "Status Is Required";
            }
            if(errorMessage != string.Empty)
            {
                throw new AppException(errorMessage);
            }
            return tranList;
        }

        private List<Transaction> ParseCsv(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    AllowComments = true,
                    Mode = CsvMode.RFC4180,
                    HasHeaderRecord = false,
                    BadDataFound = null,
                    TrimOptions = TrimOptions.Trim,
                    DetectColumnCountChanges = true,
                };
                using (var csvReader = new CsvReader(reader, config))
                {
                    csvReader.Context.RegisterClassMap<CsvMapper>();
                    csvReader.Context.TypeConverterOptionsCache.GetOptions<DateTime>().Formats = new[] { "dd/MM/yyyy hh:mm:ss" };
                    var records = csvReader.GetRecords<Transaction>().ToList();
                    return records;
                }
            }
        }

        private List<Transaction> ParseXml(IFormFile file)
        {
            List<Transaction> transactions = new List<Transaction>();

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                XDocument xmlDocument = XDocument.Parse(reader.ReadToEnd());
                var transactionElements = xmlDocument.Descendants("Transaction");

                foreach (var transactionElement in transactionElements)
                {
                    Transaction transaction = new Transaction
                    {
                        TransactionId = transactionElement.Attribute("id").Value,
                        Amount = decimal.Parse(transactionElement.Element("PaymentDetails")?.Element("Amount").Value),
                        CurrencyCode = transactionElement.Element("PaymentDetails")?.Element("CurrencyCode").Value,
                        TransactionDate = DateTime.ParseExact(transactionElement.Element("TransactionDate").Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                        Status = transactionElement.Element("Status").Value
                    };

                    transactions.Add(transaction);
                }
            }

            return transactions;
        }

    }
}
