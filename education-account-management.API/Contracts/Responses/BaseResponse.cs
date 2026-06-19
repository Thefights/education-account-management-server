namespace Contracts.Responses
{
    public class BaseResponse<TData, TError>
    {
        public TData? Data { get; set; }
        public TError? Error { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}