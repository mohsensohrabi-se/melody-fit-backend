using System;
using System.Collections.Generic;
using System.Text;

namespace MelodyFit.Application.Common.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendWelcomEmailAsync(string email);
    }
}
