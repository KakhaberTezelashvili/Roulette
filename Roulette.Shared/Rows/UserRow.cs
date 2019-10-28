namespace Roulette.Shared.Rows
{
    public class UserRow
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}