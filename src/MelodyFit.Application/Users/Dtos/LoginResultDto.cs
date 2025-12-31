

namespace MelodyFit.Application.Users.Dtos
{
    public sealed record LoginResultDto(
        Guid UserId,
        string Email,
        string Role,
        string AccessToken,
        string RefreshToken,
        DateTime AccessTokenExpiresAt
        );
    
}
