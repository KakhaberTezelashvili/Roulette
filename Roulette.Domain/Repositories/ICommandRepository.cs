using Roulette.Domain.Aggregate;
using Roulette.Shared.Enums;
using System.Threading.Tasks;

namespace Roulette.Domain.Repositories
{
    public interface ICommandRepository
    {
        Task<User> GetAggregate(int Id, UserContentFlag flag = UserContentFlag.Basic);
        Task SaveAggregate(User user);
    }
}