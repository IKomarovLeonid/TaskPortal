using System;
using Objects.Common;

namespace Core.API.View
{
    public class ErrorViewResponse
    {
        public string Code { get; }

        public string Message { get; }

        public DateTime ErrorTime { get; }

        public ErrorViewResponse(ErrorCode code, string message)
        {
            Code = code.ToString();
            Message = message;
            ErrorTime = DateTime.UtcNow;
        }
    }
}
