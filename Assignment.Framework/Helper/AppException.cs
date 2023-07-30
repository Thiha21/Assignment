using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Framework.Helper
{
    /// <summary>
    /// Only the first constructor is used to make excpetion, not needed in coverage at this time
    /// Can consider removing this in future
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class AppException : Exception
    {
        public AppException(string message) : base(message)
        {
        }

        public AppException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AppException()
        {
        }

        protected AppException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
