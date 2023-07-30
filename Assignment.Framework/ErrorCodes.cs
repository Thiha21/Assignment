using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Framework
{
    public static class ErrorCodes
    {
        public const string ErrorCodeNotFound = "404";
        public const string ErrorCodeInternalError = "500";
    }

    public static class ErrorMessages
    {
        public const string ErrorMessageNotFound = "Object not found";
        public const string ErrorMessageInternalError = "Internal error occurred";
    }
}
