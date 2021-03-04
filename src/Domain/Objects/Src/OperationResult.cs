using Objects.Common;

namespace State.Src
{
    public class OperationResult
    {
        public ulong Id { get; set; }

        public ErrorCode ErrorCode { get; set; } 

        public string Message { get; set; }

        public static OperationResult Created(ulong id) => new OperationResult
        {
            Id = id
        };

        public static OperationResult Error(ErrorCode code, string message = "") => new OperationResult
        {
            ErrorCode = code,
            Message = message
        };

        public static OperationResult Applied() => new OperationResult
        {
            ErrorCode = ErrorCode.None,
            Message = "Done"
        };
    }
}
