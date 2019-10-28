using System;
using System.Net;

namespace Roulette.Domain.Exceptions
{
    public class BaseApiException : Exception
    {
        public int ResponseHttpStatusCode { get; }

        public string BackEndMessage { get; protected set; }
        public bool ExceptionIsLogged { get; set; }

        public BaseApiException(HttpStatusCode responseHttpStatusCode)
        {
            ResponseHttpStatusCode = (int)responseHttpStatusCode;
        }

        public BaseApiException(HttpStatusCode responseHttpStatusCode, string message) : base(message)
        {
            ResponseHttpStatusCode = (int)responseHttpStatusCode;
            BackEndMessage = message;
        }
    }
}