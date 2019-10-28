using Roulette.Domain.Aggregate;
using Roulette.Domain.Repositories;
using Roulette.Infrastructure.DBProvider;
using Roulette.Shared.Enums;
using System;
using System.Threading.Tasks;

namespace Roulette.Infrastructure.Repositories
{
    public class CommandRepository : ICommandRepository
    {
        private readonly IDBProvider _dbProvider;

        public CommandRepository(IDBProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        public async Task<User> GetAggregate(int Id, UserContentFlag content = UserContentFlag.Basic)
        {
            string userQuery = @"SELECT * FROM Users WHERE Id = @Id AND IsActive = 1";

            User user = null;

            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();

                user = await db.QueryFirstOrDefaultAsync<User>(userQuery, new { Id });

                if (user != null && content.HasFlag(UserContentFlag.WithJackpot))
                {
                    var jackPotQuery = @"SELECT JackPotAmount FROM JackPot WHERE IsActive = 1";

                    var jackpotAmount = await db.QueryFirstOrDefaultAsync<decimal>(jackPotQuery);

                    user.WithJackpot(jackpotAmount);
                }

                db.Close();
            }

            return user;
        }

        public async Task SaveAggregate(User user)
        {
            if (user.Id == default(int))
            {
                await InsertUserAsync(user);
                return;
            }

            await UpdateUserAsync(user);
        }

        #region helpers

        private async Task UpdateUserAsync(User user)
        {
            var db = _dbProvider.GetDBInstance();

            try
            {
                db.Open();
                db.BeginTransaction();

                var updateUser = @"UPDATE Users SET FirstName = @FirstName, 
                            LastName = @LastName, 
                            UserName = @UserName,
                            Balance = @Balance,
                            UpdateDate = @UpdateDate WHERE Id = @Id";

                await db.ExecuteAsync(updateUser, new
                {
                    user.FirstName,
                    user.LastName,
                    user.UserName,
                    user.Balance,
                    UpdateDate = DateTime.Now,
                    user.Id
                });

                if (user.Bet != null)
                {
                    var addBet = @"INSERT INTO bets(SpinId,
                                   UserId, BetString, BetAmount, WinningNumber, JackPotId, 
                                   WonAmount, Status, IpAddress, CreateDate, IsActive)
                                   VALUES(@SpinId, @UserId, @BetString, @BetAmount, 
                                   @WinningNumber, @JackPotId, @WonAmount, @Status, 
                                   @IpAddress, @CreateDate, @IsActive)";

                    await db.ExecuteAsync(addBet, new
                    {
                        SpinId = user.Bet.Id,
                        UserId = user.Id,
                        user.Bet.BetString,
                        user.Bet.BetAmount,
                        user.Bet.WinningNumber,
                        JackPotId = 1, // harcoded for demo
                        user.Bet.WonAmount,
                        Status = (int)user.Bet.Status,
                        user.Bet.IpAddress,
                        CreateDate = DateTime.Now,
                        IsActive = 1
                    });

                    var updateJackPot = @"UPDATE JackPot SET
                        JackPotAmount = @JackPotAmount,
                        UpdateDate = @UpdateDate";

                    await db.ExecuteAsync(updateJackPot, new
                    {
                        JackPotAmount = user.JackpotAmount,
                        UpdateDate = DateTime.Now
                    });
                }

                db.CommitTransaction();
            }
            catch
            {
                db.RollBackTransaction();
            }
            finally
            {
                db.Close();
                db.Dispose();
            }
        }

        private Task InsertUserAsync(User user)
        {
            // TODO Some Insertion Logic

            return Task.CompletedTask;
        }

        #endregion
    }
}