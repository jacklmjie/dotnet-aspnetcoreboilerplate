using System;

namespace Core.Common.Messages
{
    public class ResponseMessage : ResponseMessageWrap<object>
    {
    }
    public class ResponseMessageWrap<TBody>
    {
        public bool IsSuccess { get; set; } = true;
        public String ErrorCode { get; set; } = "0000";
        public String Message { get; set; }
        public TBody Body { get; set; }
    }
}



