using System;

namespace User.API.Filters
{
    public class UserOperationException : Exception
    {
        public UserOperationException() { }

        public UserOperationException(string message) : base(message) { }

        public UserOperationException(string message, Exception innerExpection) : base(message, innerExpection) { }
    }
}
