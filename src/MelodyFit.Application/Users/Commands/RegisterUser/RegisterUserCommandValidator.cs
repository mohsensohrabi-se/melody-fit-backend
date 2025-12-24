using FluentValidation;


namespace MelodyFit.Application.Users.Commands.RegisterUser
{
    public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator() 
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("A valid email is required");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long");

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.BirthDate)
                .LessThan(DateTime.UtcNow)
                .When(x => x.BirthDate.HasValue)
                .WithMessage("Birth date can not be in future");

            RuleFor(x => x.WeightKg)
                .InclusiveBetween(20, 400)
                .When(x => x.WeightKg.HasValue);

            RuleFor(x => x.HeightCm)
                .InclusiveBetween(50, 300)
                .When(x => x.HeightCm.HasValue);

        }
    }
}
