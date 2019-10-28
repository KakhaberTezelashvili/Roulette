using System.Net;

namespace Roulette.Domain.Exceptions
{
    public class BetNotCorrectException : BaseApiException
    {
        public override string Message { get; }

        public BetNotCorrectException() : base(HttpStatusCode.BadRequest)
        {
            Message = "Bet Not Correct";
        }

        public BetNotCorrectException(string message) : base(HttpStatusCode.BadRequest)
        {
            Message = message;
        }
    }
}
