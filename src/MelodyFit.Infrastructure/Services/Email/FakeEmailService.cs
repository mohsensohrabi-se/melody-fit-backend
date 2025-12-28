using MelodyFit.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Logging;


namespace MelodyFit.Infrastructure.Services.Email
{
    public class FakeEmailService : IEmailService
    {
        private readonly ILogger<FakeEmailService> _logger;
        public FakeEmailService(ILogger<FakeEmailService> logger)
        {
            _logger = logger;
        }
        public Task SendWelcomEmailAsync(string email)  
        {
            _logger.LogInformation("[FAKE EMAIL] Welcom email is sent to {email}",email);
            return Task.CompletedTask;
        }
    }
}
