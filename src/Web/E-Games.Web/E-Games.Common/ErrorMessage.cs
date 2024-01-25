using Microsoft.AspNetCore.Http;

namespace E_Games.Web.Exceptions
{
    public class ErrorMessage
    {
        public int Code { get; set; }
        public string? Message { get; set; }

        public ErrorMessage(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public static ErrorMessage BadRequest => new ErrorMessage(StatusCodes.Status400BadRequest, "Bad Request");

        public static ErrorMessage NotFound => new ErrorMessage(StatusCodes.Status404NotFound, "Not Found");
    }
}
