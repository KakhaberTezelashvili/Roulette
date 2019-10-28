using PBG.Micro.DDD;
using Roulette.Domain.Entities;
using System;
using Roulette.Shared.Enums;
using Roulette.Domain.Exceptions;
using ge.singular.roulette;

namespace Roulette.Domain.Aggregate
{
    public class User : AggregateRoot<int>
    {
        public Bet Bet { get; private set; }
        public decimal JackpotAmount { get; private set; }

        #region Attributes

        public override int Id { get; protected set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }

        #endregion

        #region Behaviour

        public Bet MakeBet(string betString, decimal betAmount, string ipAddress)
        {
            CheckBet(betString, betAmount);

            var winningNumber = new Random().Next(0, 36);

            var wonAmount = GetUsersWonAmount(betString, winningNumber);

            Bet = new Bet(Guid.NewGuid(), betString, betAmount, winningNumber,
                wonAmount, BetStatus.Accepted, ipAddress, DateTime.Now);

            UpdateBalance(betAmount);
            UpdateJackpotAmount(betAmount);

            return Bet;
        }

        public User WithJackpot(decimal jackpotAmount)
        {
            JackpotAmount = jackpotAmount;
            return this;
        }

        #endregion

        #region Helpers

        private void UpdateJackpotAmount(decimal betAmount) => JackpotAmount += betAmount * 0.01m;

        private void UpdateBalance(decimal betAmount) => Balance -= betAmount;

        private decimal GetUsersWonAmount(string betString, int winningNumber)
            => CheckBets.EstimateWin(betString, winningNumber);

        private void CheckBet(string betString, decimal betAmount)
        {
            if (!CheckBets.IsValid(betString).getIsValid())
                throw new BetNotCorrectException("Bet String Not Correct");

            if (betAmount >= Balance)
                throw new BetNotCorrectException("Inefficient Balance");
        }

        #endregion
    }
}