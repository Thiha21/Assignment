using Assignment.Application.Contracts.Dtos;
using Assignment.Application.Contracts.ServiceInterfaces;
using Assignment.Domain.Repositories;
using Assignment.Framework;
using Assignment.Framework.Helper;
using AutoMapper;
using Serilog;

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
    }
}
