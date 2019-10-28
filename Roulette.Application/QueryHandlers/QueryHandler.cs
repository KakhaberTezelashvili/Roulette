using System.Threading.Tasks;
using PBG.Micro.CQRS.Queries;
using Roulette.Application.Queries;
using Roulette.Application.SignInManager;
using Roulette.Domain.Exceptions;
using Roulette.Domain.Repositories;
using Roulette.Shared.Rows;

namespace Roulette.Application.QueryHandlers
{
    public class QueryHandler :
        IQueryHandlerAsync<SignInQuery, string>,
        IQueryHandlerAsync<UserBalanceQuery, decimal>,
        IQueryHandlerAsync<GameHistoryQuery, PagedData<BetRow>>,
        IQueryHandlerAsync<JackPotQuery, decimal>
    {
        private readonly IQueryRepository _repository;
        private readonly ISignInManager _signInManager;

        public QueryHandler(IQueryRepository repository, ISignInManager signInManager)
        {
            _repository = repository;
            _signInManager = signInManager;
        }

        public async Task<decimal> HandleAsync(JackPotQuery query)
            => await _repository.GetCurrentJackpot();

        public async Task<PagedData<BetRow>> HandleAsync(GameHistoryQuery query)
        {
            var data = await _repository.GetUserGameHistory(query.UserId, query.PageNumber, query.Take)
                 ?? throw new UserNotFoundException();

            var numberOfRows = await _repository.GetNumberOfBets(query.UserId);

            return new PagedData<BetRow>(data, numberOfRows);
        }

        public async Task<string> HandleAsync(SignInQuery query)
        {
            var user = await _repository.FindUserByUserName(query.UserName)
                ?? throw new UserNotFoundException();

            var password = _signInManager.GetPassword(query.Password, user.Salt);

            if (user.Password != password)
                throw new UserNotFoundException();

            return _signInManager.GenerateToken();
        }

        public async Task<decimal> HandleAsync(UserBalanceQuery query)
        {
            var balance = await _repository.GetUserBalance(query.UserId);

            if (balance == null)
                throw new UserNotFoundException();

            return balance.Value;
        }
    }
}