using Assignment.Application.Contracts.Dtos;
using Assignment.Application.Contracts.ServiceInterfaces;
using Assignment.Framework.ApiDtos;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Assignment.Framework;
using Assignment.Framework.Helper;
using System.Net;

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
                return BadRequest(new ResponseDto
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
            catch (AppException e)
            {
                return BadRequest(new ResponseDto
                {
                    Code = ErrorCodes.ErrorCodeInternalError,
                    Message = ErrorMessages.ErrorMessageInternalError,
                    Details = e.Message
                });
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
            catch (AppException e)
            {
                return BadRequest(new ResponseDto
                {
                    Code = ErrorCodes.ErrorCodeInternalError,
                    Message = ErrorMessages.ErrorMessageInternalError,
                    Details = e.Message
                });
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new ResponseDto
                {
                    Code = ErrorCodes.ErrorCodeBadRequest,
                    Message = ErrorMessages.ErrorMessageUnknownFormat,
                    Details = "File not provided or Empty"
                });
            }

            if (file.Length > 1048576)
            {
                return BadRequest(new ResponseDto
                {
                    Code = ErrorCodes.ErrorCodeBadRequest,
                    Message = ErrorMessages.ErrorMessageUnknownFormat,
                    Details = "File size exceeds the allowed limit of 1MB."
                });
            }

            
            if (file.FileName.EndsWith(".csv") || file.FileName.EndsWith(".xml"))
            {
                try
                {
                    await _transactionService.UploadFile(file);
                }
                catch (AppException e)
                {
                    return BadRequest(new ResponseDto
                    {
                        Code = ErrorCodes.ErrorCodeBadRequest,
                        Message = ErrorMessages.ErrorMessageValidationError,
                        Details = e.Message
                    });
                }
                catch(Exception e)
                {
                    return BadRequest(new ResponseDto
                    {
                        Code = ErrorCodes.ErrorCodeInternalError,
                        Message = ErrorMessages.ErrorMessageInternalError,
                        Details = e.Message
                    });
                }
            }
            else
            {
                return BadRequest(new ResponseDto
                {
                    Code = ErrorCodes.ErrorCodeBadRequest,
                    Message = ErrorMessages.ErrorMessageUnknownFormat,
                    Details = ""
                });
            }

            return Ok(new ResponseDto
            {
                Code = "200",
                Message = "Success",
                Details = "File uploaded successfully"
            });
        }

    }
}
