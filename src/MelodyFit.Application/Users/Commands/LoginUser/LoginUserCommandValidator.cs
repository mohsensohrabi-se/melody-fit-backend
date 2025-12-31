using FluentValidation;


namespace MelodyFit.Application.Users.Commands.LoginUser
{
    public sealed class LoginUserCommandValidator:AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator() 
        {
            RuleFor(ul => ul.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(256);
                
            
            RuleFor(ul => ul.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
