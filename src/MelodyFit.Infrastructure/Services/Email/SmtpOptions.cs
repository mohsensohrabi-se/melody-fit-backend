using System;
using System.Collections.Generic;
using System.Text;

namespace MelodyFit.Infrastructure.Services.Email
{
    public sealed class SmtpOptions
    {
        public string Host { get; init; } = null!;
        public int Port { get; init; }
        public string UserName { get; init; } = null!;
        public string Password { get; init; } = null!;
        public string FromEmail { get; init; } = null!;
        public string FromName { get; init; } = null!;
    }
}
