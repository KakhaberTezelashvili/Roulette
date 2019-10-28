using PBG.Micro.CQRS.Queries;

namespace Roulette.Application.Queries
{
    /// <summary>
    /// Query to login user. User will automatically be signed out after 5 minutes
    /// </summary>
    public class SignInQuery : IQuery<string>
    {
        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// User's flat password
        /// </summary>
        public string Password { get; set; }
    }
}