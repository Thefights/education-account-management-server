namespace DTOs.Base
{
    public class AiServiceResult
    {
        public bool IsSuccess { get; set; }
        public string? Content { get; set; }
        public int StatusCode { get; set; }
        public string? ErrorMessage { get; set; }

        public static AiServiceResult Success(string content, int statusCode)
        {
            return new AiServiceResult
            {
                IsSuccess = true,
                Content = content,
                StatusCode = statusCode
            };
        }

        public static AiServiceResult Failure(string errorMessage, int statusCode)
        {
            return new AiServiceResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                StatusCode = statusCode
            };
        }
    }
}
