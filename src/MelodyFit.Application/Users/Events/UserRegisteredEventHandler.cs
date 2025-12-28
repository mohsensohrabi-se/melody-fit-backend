using MediatR;
using MelodyFit.Application.Common.Interfaces.Persistence;
using MelodyFit.Application.Common.Interfaces.Services;
using MelodyFit.Domain.Users.Events;


namespace MelodyFit.Application.Users.Events
{
    public class UserRegisteredEventHandler : INotificationHandler<UserRegisteredEvent>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public UserRegisteredEventHandler(
            IUserRepository userRepository,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
        {
            var userResult = await _userRepository.GetByIdAsync(notification.UserId);

            if (!userResult.IsSuccess || userResult.Value is null)
                return; // fail silently

            await _emailService.SendWelcomEmailAsync(userResult.Value.Email.Value);

        }
    }
}
