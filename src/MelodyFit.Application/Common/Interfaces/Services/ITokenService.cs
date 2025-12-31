

namespace MelodyFit.Application.Common.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(
            Guid userId,
            string email,
            string role,
            DateTime expiresAt
            );

        string GenerateRefreshToken();
    }
}
