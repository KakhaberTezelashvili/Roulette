using Roulette.Domain.Exceptions;
using Roulette.Domain.Repositories;
using Roulette.Infrastructure.DBProvider;
using Roulette.Shared.Rows;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Roulette.Infrastructure.Repositories
{
    public class QueryRepository : IQueryRepository
    {
        private readonly IDBProvider _dbProvider;

        public QueryRepository(IDBProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        public async Task<decimal> GetCurrentJackpot()
        {
            string sql = "SELECT JackPotAmount FROM Jackpot WHERE IsActive = 1";

            decimal jackPot = default(decimal);

            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();

                jackPot = await db.QueryFirstOrDefaultAsync<decimal>(sql);

                db.Close();
            }

            return jackPot;
        }

        public async Task<decimal?> GetUserBalance(int userId)
        {
            string sql = "SELECT Id, Balance FROM Users WHERE Id = @userId";

            UserRow user = null;

            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();

                user = await db.QueryFirstOrDefaultAsync<UserRow>(sql, new { userId });

                db.Close();
            }

            return user?.Balance;
        }

        public async Task<UserRow> FindUserByUserName(string userName)
        {
            string sql = "SELECT * FROM Users WHERE UserName = @userName";

            UserRow user = null;

            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();

                user = await db.QueryFirstOrDefaultAsync<UserRow>(sql, new { userName });

                db.Close();
            }

            return user;
        }

        public async Task<int> GetNumberOfBets(int userId)
        {
            string sql = "SELECT COUNT(1) FROM Bets WHERE UserId = @userId";

            int numberOfBets = default(int);

            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();

                numberOfBets = await db.ExecuteScalarAsync(sql, new { userId });

                db.Close();
            }
            return numberOfBets;
        }

        public async Task<IEnumerable<BetRow>> GetUserGameHistory(int userId, int? pageNumber, int? itemsPerPage)
        {
            var skip = pageNumber == null ? 0 : (pageNumber - 1) * itemsPerPage;
            var take = itemsPerPage == null ? 15 : itemsPerPage;

            string userQuery = "SELECT Id FROM Users WHERE Id = @userId";

            string betQuery = "SELECT SpinID, BetAmount, CreateDate, WonAmount FROM Bets WHERE UserId = @userId ORDER BY CreateDate DESC LIMIT @skip, @take";

            UserRow user = null;
            IEnumerable<BetRow> bets = null;

            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();

                user = await db.QueryFirstOrDefaultAsync<UserRow>(userQuery, new { userId });

                if (user != null)
                    bets = await db.QueryAsync<BetRow>(betQuery, new { userId, skip, take });

                db.Close();
            }
            
            return bets;
        }
    }
}