using System;
using System.Runtime.Serialization;

namespace Core.Common.Exceptions
{
    public class APIException : Exception
    {
        public APIException()
        {
            ErrorCode = "0001";
        }
        public String ErrorCode { get; set; }

        public APIException(string errorCode) : base() { ErrorCode = errorCode; }

        public APIException(string errorCode, string message) : base(message) { ErrorCode = errorCode; }

        public APIException(string errorCode, SerializationInfo info, StreamingContext context) : base(info, context) { ErrorCode = errorCode; }

        public APIException(string errorCode, string message, System.Exception innerException) : base(message, innerException) { ErrorCode = errorCode; }
    }
}
