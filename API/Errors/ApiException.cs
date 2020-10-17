using System.Diagnostics;
namespace API.Errors                                                                    // will return any errors we get from our application
{
    public class ApiException
    {
        public ApiException(int statusCode, string message=null, string details=null)                     // properties populated inside constructor from create ctor
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }

        public int StatusCode { get; set; }

        public string Message { get; set; }                                          

        public string Details { get; set; }                                             // details of stack trace

    }
}