using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PBG.Micro.CQRS.Commands;
using PBG.Micro.CQRS.Queries;
using Roulette.Application.Commands;
using Roulette.Application.Queries;
using Roulette.Shared.Rows;

namespace Roulette.API.Controllers
{
    [Route("api/" + nameof(Startup.v1) + "/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ICommandBusAsync _commandBus;
        private readonly IQueryBusAsync _queryBus;
        private readonly IHttpContextAccessor _accessor;

        public UserController(ICommandBusAsync commandBus,
           IQueryBusAsync queryBus, IHttpContextAccessor accessor)
        {
            _commandBus = commandBus;
            _queryBus = queryBus;
            _accessor = accessor;
        }

        #region Commands

        /// <summary>
        /// Method to make a bet
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status, SpinID, Winning Number, WonAmount in cents.</returns>
        [HttpPost("MakeBet")]
        public async Task<BetRow> MakeBet(MakeBetCommand command)
        {
            command.IpAddress = _accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            return await _commandBus.ExecuteAsync(command);
        }

        #endregion

        #region Queries

        /// <summary>
        /// This method is used to sign into the app, 
        /// system will automatically sign user out in 5 minutes 
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Generated JWT token</returns>
        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<string> SignIn(SignInQuery query)
            => await _queryBus.ExecuteAsync(query);

        /// <summary>
        /// Method to retrieve current jackpot
        /// </summary>
        /// <returns>Current JackPot</returns>
        [HttpPost("CurrentJackPot")]
        public async Task<decimal> GetCurrentJackpot(JackPotQuery query)
            => await _queryBus.ExecuteAsync(query);

        /// <summary>
        /// Method for requesting user's game history
        /// </summary>
        /// <param name="query"></param>
        /// <returns>List of user bets</returns>
        [HttpPost("GameHistory")]
        public async Task<PagedData<BetRow>> GetUserGameHistory(GameHistoryQuery query)
            => await _queryBus.ExecuteAsync(query);

        /// <summary>
        /// Method retrieves user's current balance
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Users current balance</returns>
        [HttpPost("UserBalance")]
        public async Task<decimal> GetUserBalance(UserBalanceQuery query)
            => await _queryBus.ExecuteAsync(query);

        #endregion
    }
}