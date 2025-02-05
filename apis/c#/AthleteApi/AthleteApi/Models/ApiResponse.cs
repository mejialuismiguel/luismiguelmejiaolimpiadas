namespace AthleteApi.Models
{
    public class ApiResponse
    {
        public string Message { get; set; } = string.Empty;
        public int ErrorCode { get; set; }

        public ApiResponse(string message, int errorCode)
        {
            Message = message;
            ErrorCode = errorCode;
        }
    }
}