using PBG.Micro.CQRS.Queries;
using Roulette.Shared.Rows;

namespace Roulette.Application.Queries
{
    /// <summary>
    /// Retrieve's Users Game History
    /// </summary>
    public class GameHistoryQuery : IQuery<PagedData<BetRow>>
    {
        /// <summary>
        /// Online Client's Id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Optional parameter for paging
        /// </summary>
        public int? PageNumber { get; set; }
        /// <summary>
        /// Optional parameter for paging
        /// </summary>
        public int? Take { get; set; }
    }
}