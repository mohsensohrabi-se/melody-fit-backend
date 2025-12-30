using MelodyFit.Application.Common.Interfaces.Persistence;
using MelodyFit.Domain.Common;
using MelodyFit.Domain.Users.Aggregates;
using Microsoft.EntityFrameworkCore;


namespace MelodyFit.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MelodyFitDbContext _context;

        public UserRepository(MelodyFitDbContext context)
        {
            _context = context;
        }
        public async Task<Result> AddAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (DbUpdateException ex) 
            {
                return Result.Failure($"Database error while creating user{ex.Message}");
            }

        }

        public async Task<Result<bool>> ExistsByEmailAsync(string email)
        {
            try
            {
                var exists =await  _context.Users.AnyAsync(u=> u.Email.Value == email.Trim().ToLower());
                return Result.Success(exists);
            }
            catch (Exception ex) 
            {
                return Result.Failure<bool>($"Database error {ex.Message}");
            }
        }

        public async Task<Result<User?>> GetByEmailAsync(string email)
        {
            try
            {
                var normalizedEmail = email.Trim().ToLower();

                var user = await _context.Users
                    .Include(u => u.PersonalRecords)
                    .FirstOrDefaultAsync(u => u.Email.Value == normalizedEmail);

                if (user is null)
                    return Result.Success<User?>(null);
                return Result.Success<User?>(user);
            }
            catch(Exception ex)
            {
                return Result.Failure<User?>($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<User?>> GetByIdAsync(Guid id)
        {
            try
            {
                var user = await _context.Users
                    .Include(u=>u.PersonalRecords)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user is null)
                    return Result.Failure<User?>(null);

                return Result.Success<User?>(user);
            }
            catch (Exception ex) 
            {
                return Result.Failure<User?>($"Database error {ex.Message}");
            }
        }
    }
}
