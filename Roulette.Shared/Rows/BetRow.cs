using System;

namespace Roulette.Shared.Rows
{
    public class BetRow
    {
        public BetRow() { }

        public BetRow(Guid spinID, decimal betAmount, decimal wonAmount, DateTime createDate)
        {
            SpinID = spinID;
            BetAmount = betAmount;
            WonAmount = wonAmount;
            CreateDate = createDate;
        }

        public Guid SpinID { get; set; }
        public decimal BetAmount { get; set; }
        public decimal WonAmount { get; set; }
        public DateTime CreateDate { get; set; }
    }
}