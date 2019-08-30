using System;

namespace Core.Common
{
    public class ResponseMessage : ResponseMessageWrap<object>
    {
        public ResponseMessage()
        {

        }
        public ResponseMessage(bool isSuccess)
        {
            this.IsSuccess = isSuccess;
        }
    }
    public class ResponseMessageWrap<TBody>
    {
        public bool IsSuccess { get; set; } = true;
        public String ErrorCode { get; set; } = "0000";
        public String Message { get; set; }
        public TBody Body { get; set; }
    }
}



