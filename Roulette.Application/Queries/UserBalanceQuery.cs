using PBG.Micro.CQRS.Queries;

namespace Roulette.Application.Queries
{
    /// <summary>
    /// Retrieves users current balance
    /// </summary>
    public class UserBalanceQuery : IQuery<decimal>
    {
        /// <summary>
        /// User's Id
        /// </summary>
        public int UserId { get; set; }
    }
}