using Assignment.Application.Contracts.Dtos;
using Assignment.Application.Contracts.ServiceInterfaces;
using Assignment.Framework.ApiDtos;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Assignment.Framework;
using Assignment.Framework.Helper;

namespace Assignment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("get-by-currency/{currencyCode}")]
        public async Task<IActionResult> GetByCurrency(string currencyCode)
        {
            try
            {
                var result = await _transactionService.GetByCurrencyCode(currencyCode);

                return Ok(result);
            }
            catch (AppException e)
            {
                return BadRequest(new ErrorViewModel
                {
                    Code = ErrorCodes.ErrorCodeInternalError,
                    Message = ErrorMessages.ErrorMessageInternalError,
                    Details = e.Message
                });
            }
        }

        [HttpPost("get-by-date-range")]
        public async Task<IActionResult> GetByDateRange(GetTransactionByDateRange request)
        {
            try
            {
                var result = await _transactionService.GetTransactionByDateRange(request);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet("get-by-status/{status}")]
        public async Task<IActionResult> GetByStatus(string status)
        {

            try
            {
                var result = await _transactionService.GetByStatus(status);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
