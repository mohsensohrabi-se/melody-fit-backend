using MelodyFit.Application.Common.Interfaces.Services;


namespace MelodyFit.Infrastructure.Services.Security
{
    public sealed class PasswordService : IPasswordService
    {
        public Task<string> HashPassword(string password)
        {
            if(string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("Password is empty");
            var hash = BCrypt.Net.BCrypt.HashPassword(
                password,
                workFactor:12
                );   
            return Task.FromResult( hash );
        }

        public Task<bool> Verify(string password, string hash)
        {
            var result = BCrypt.Net.BCrypt.Verify(password, hash);
            return Task.FromResult(result);
        }
    }
}
