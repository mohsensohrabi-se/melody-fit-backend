using MediatR;
using MelodyFit.Application.Common.Exceptions;
using MelodyFit.Application.Common.Interfaces.Persistence;
using MelodyFit.Application.Common.Interfaces.Services;
using MelodyFit.Domain.Users.Aggregates;
using MelodyFit.Domain.Users.ValueObjects;


namespace MelodyFit.Application.Users.Commands.RegisterUser
{
    public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // check to see if the email already registered or not
            var existResult = await _userRepository.ExistsByEmailAsync(request.Email);
            if (!existResult.IsSuccess)
                throw new Exception(existResult.Error);

            if(existResult.Value)
                throw new ConflictException("Email already registered");

            //Hash password
            var passwordHash = await _passwordService.HashPassword(request.Password);

            // Create UserProfile
            var profileResult = UserProfile.Create(
                request.FirstName,
                request.LastName,
                request.BirthDate,
                request.Gender,
                request.WeightKg,
                request.HeightCm
                );

            if(!profileResult.IsSuccess)
                throw new ApplicationException(profileResult.Error);
            // Register User
            var userResult = User.Register(
                request.Email,
                passwordHash,
                profileResult.Value
                );
            if(!userResult.IsSuccess)
                throw new ApplicationException(userResult.Error);
            //Persist the User
            var addResult = await _userRepository.AddAsync(userResult.Value);
            if(!addResult.IsSuccess)
                throw new ApplicationException(addResult.Error);

            return userResult.Value.Id;
        }
    }
}
