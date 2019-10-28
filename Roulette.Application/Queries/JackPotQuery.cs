using PBG.Micro.CQRS.Queries;

namespace Roulette.Application.Queries
{
    /// <summary>
    /// Retrieves current jackpot amount
    /// </summary>
    public class JackPotQuery : IQuery<decimal>
    {
    }
}
