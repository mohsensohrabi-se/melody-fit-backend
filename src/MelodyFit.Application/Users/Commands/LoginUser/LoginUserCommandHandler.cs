using MediatR;
using MelodyFit.Application.Common.Exceptions;
using MelodyFit.Application.Common.Interfaces.Persistence;
using MelodyFit.Application.Common.Interfaces.Services;
using MelodyFit.Application.Users.Dtos;


namespace MelodyFit.Application.Users.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginResultDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;

        public LoginUserCommandHandler(
            IUserRepository userRepository,
            IPasswordService passwordService,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        public async Task<LoginResultDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var normalizedEmail = request.Email.Trim().ToLower();
            var userResult = await _userRepository.GetByEmailAsync(normalizedEmail);
            if (!userResult.IsSuccess)
                throw new ApplicationException(userResult.Error);

            var user = userResult.Value;

            if(user == null)
                throw new ApplicationException("User not found");

            var isPassworValid =  await _passwordService.Verify(request.Password, user.PasswordHash);
            if (isPassworValid == false)
                throw new ApplicationException("Invalid username or password");
            // Generate token
            var accessTokenExpiresAt = DateTime.UtcNow.AddMinutes(60);

            var accessToken = _tokenService.GenerateAccessToken(
                user.Id,
                user.Email.Value,
                role:"User",
                expiresAt:accessTokenExpiresAt
                );
            var refreshToken = _tokenService.GenerateRefreshToken();

            return new LoginResultDto(
                user.Id,
                user.Email.Value,
                "User",
                accessToken,
                refreshToken,
                accessTokenExpiresAt
                );
        }
    }
}
