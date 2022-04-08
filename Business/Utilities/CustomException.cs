using System.Net;

namespace Business.Utilities
{
    public class CustomException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public CustomException()
        {
        }

        public CustomException(string message, HttpStatusCode code)
            : base(message)
        {
            StatusCode = code;
        }
    }
}
