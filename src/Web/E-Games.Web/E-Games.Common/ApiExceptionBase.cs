namespace E_Games.Common
{
    public class ApiExceptionBase : Exception
    {
        public int StatusCode { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();

        public ApiExceptionBase(int statusCode, string message) : base(message)
            => StatusCode = statusCode;

        public ApiExceptionBase(int statusCode, string message, IEnumerable<string> errors)
            : base(message)
        {
            StatusCode = statusCode;
            Errors = errors;
        }
    }
}
