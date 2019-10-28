using Newtonsoft.Json;
using PBG.Micro.CQRS.Commands;
using Roulette.Shared.Rows;

namespace Roulette.Application.Commands
{
    /// <summary>
    /// Command to make a bet
    /// </summary>
    public class MakeBetCommand : ICommandWithResult<BetRow>
    {
        /// <summary>
        /// Online User's Id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Json Bet String. for example: [{"T": "v", "I": 20, "C": 1, "K": 1}]
        /// </summary>
        public string BetString { get; set; }
        /// <summary>
        /// User's bet amount
        /// </summary>
        public decimal BetAmount { get; set; }

        [JsonIgnore]
        public string IpAddress { get; set; }
    }
}