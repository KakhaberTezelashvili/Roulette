using System.Net;

namespace Roulette.Domain.Exceptions
{
    public class UserNotFoundException : BaseApiException
    {
        public override string Message { get; }

        public UserNotFoundException() : base(HttpStatusCode.NotFound)
        {
            Message = "User Not Found";
        }
    }
}