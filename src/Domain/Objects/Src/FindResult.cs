using Objects.Common;

namespace Objects
{
    public class FindResult<TModel>
    {
        public TModel Data { get; set; }

        public ErrorCode ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public static FindResult<TModel> Error(ErrorCode code, string message = "") => new FindResult<TModel>
        {
            ErrorCode = code,
            ErrorMessage = message
        };

        public static FindResult<TModel> Applied(TModel model) => new FindResult<TModel> { Data = model };
    }
}
