using Roulette.Shared.Rows;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Roulette.Domain.Repositories
{
    public interface IQueryRepository
    {
        Task<decimal> GetCurrentJackpot();
        Task<UserRow> FindUserByUserName(string userName);
        Task<decimal?> GetUserBalance(int userId);
        Task<int> GetNumberOfBets(int userId);
        Task<IEnumerable<BetRow>> GetUserGameHistory(int userId, int? pageNumber, int? itemsPerPage);
    }
}
