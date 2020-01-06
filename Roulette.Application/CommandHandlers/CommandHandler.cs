using System.Threading.Tasks;
using PBG.Micro.CQRS.Commands;
using Roulette.Application.Commands;
using Roulette.Domain.Exceptions;
using Roulette.Domain.Repositories;
using Roulette.Shared.Enums;
using Roulette.Shared.Rows;

namespace Roulette.Application.CommandHandlers
{
    public class CommandHandler : ICommandHandlerAsync<MakeBetCommand, BetRow>
    {
        private readonly ICommandRepository _repository;

        public CommandHandler(ICommandRepository repository) 
            => _repository = repository;

        public async Task<BetRow> HandleAsync(MakeBetCommand command)
        {
            var user = await _repository.GetAggregate(command.UserId, UserContentFlag.WithJackpot)
                ?? throw new UserNotFoundException();

            var bet = user.MakeBet(command.BetString, command.BetAmount, command.IpAddress);

            await _repository.SaveAggregate(user);

            return new BetRow(bet.Id, bet.BetAmount, bet.WonAmount, bet.CreateDate);
        }
    }
}