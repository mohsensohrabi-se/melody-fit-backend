using MelodyFit.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;


namespace MelodyFit.Infrastructure.Services.Email
{
    public sealed class SmtpEmailService : IEmailService
    {
        private readonly SmtpOptions _options;

        public SmtpEmailService(IOptions<SmtpOptions> options)
        {
            _options = options.Value;
        }
        public async Task SendWelcomEmailAsync(string email)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_options.FromEmail, _options.FromName),
                Subject = "Welcome to MelodyFit",
                Body = BuildWelcomeEmailBody(),
                IsBodyHtml = true
            };
            message.To.Add(email);

            using var smtpClient = new SmtpClient(
                _options.Host,
                _options.Port
                )
            {
                Credentials = new NetworkCredential(_options.UserName,_options.Password),
                EnableSsl = true
            };
            await smtpClient.SendMailAsync(message);

        }

        private static string BuildWelcomeEmailBody()
        {
            return """
            <h2>Welcome to MelodyFit!</h2>
            <p>We're excited to have you onboard.</p>
            <p>Start your first workout and enjoy music that matches your tempo 🎵</p>
            <br/>
            <strong>— MelodyFit Team</strong>
            """;
        }
    }
}
