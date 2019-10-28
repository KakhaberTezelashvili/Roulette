using PBG.Micro.DDD;
using Roulette.Shared.Enums;
using System;

namespace Roulette.Domain.Entities
{
    public class Bet : Entity<Guid>
    {
        public Bet(Guid spinId, string betString, decimal betAmount, int winningNumber, 
            decimal wonAmount, BetStatus status, string ipAddress, DateTime createDate)
        {
            Id = spinId;
            BetString = betString;
            BetAmount = betAmount;
            WinningNumber = winningNumber;
            WonAmount = wonAmount;
            Status = status;
            IpAddress = ipAddress;
            CreateDate = createDate;
        }

        public override Guid Id { get; protected set; }
        public string BetString { get; }
        public decimal BetAmount { get; }
        public int WinningNumber { get; }
        public decimal WonAmount { get; }
        public BetStatus Status { get; }
        public string IpAddress { get; }
        public DateTime CreateDate { get; }
    }
}