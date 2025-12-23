using MelodyFit.Domain.Common;
using MelodyFit.Domain.Users.Aggregates;


namespace MelodyFit.Application.Common.Interfaces.Persistence
{
    public interface IUserRepository
    {
        Task<Result> AddAsync(User user);
        Task<Result<bool>> ExistByEmailAsync(string email);
        Task<Result<User>> GetByIdAsync(Guid id);
    }
}
