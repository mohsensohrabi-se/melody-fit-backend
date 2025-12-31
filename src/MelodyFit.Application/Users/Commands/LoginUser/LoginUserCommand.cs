using MediatR;
using MelodyFit.Application.Users.Dtos;


namespace MelodyFit.Application.Users.Commands.LoginUser
{
    public sealed record LoginUserCommand(string Email, string Password):IRequest<LoginResultDto>;
    
    
}
