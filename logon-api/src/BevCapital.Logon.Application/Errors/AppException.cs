using System;
using System.Net;

namespace BevCapital.Logon.Application.Errors
{
    public class AppException : Exception
    {
        public HttpStatusCode Code { get; }
        public object Errors { get; }

        public AppException(HttpStatusCode code, object errors = null) : base()
        {
            Code = code;
            Errors = errors;
        }

        public AppException(string message, HttpStatusCode code, object errors = null) : base(message)
        {
            Code = code;
            Errors = errors;
        }

        public AppException(string message, Exception innerException, HttpStatusCode code, object errors = null)
            : base(message, innerException)
        {
            Code = code;
            Errors = errors;
        }
    }
}
