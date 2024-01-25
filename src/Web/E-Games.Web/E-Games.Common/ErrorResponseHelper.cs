using E_Games.Common;

namespace E_Games.Web.Exceptions
{
    public static class ErrorResponseHelper
    {
        public static void RaiseError(ErrorMessage errorMessage)
            => throw new ApiExceptionBase(errorMessage.Code, errorMessage.Message!);

        public static void RaiseError(ErrorMessage errorMessage, string error)
        {
            var errors = new List<string> { error };
            throw new ApiExceptionBase(errorMessage.Code, errorMessage.Message!, errors);
        }

        public static void RaiseError(ErrorMessage errorMessage, IEnumerable<string> errors)
            => throw new ApiExceptionBase(errorMessage.Code, errorMessage.Message!, errors);
    }
}