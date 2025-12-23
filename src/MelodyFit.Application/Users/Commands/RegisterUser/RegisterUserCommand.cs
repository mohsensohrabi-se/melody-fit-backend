using MediatR;


namespace MelodyFit.Application.Users.Commands.RegisterUser
{
    public sealed record RegisterUserCommand(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        DateTime? BirthDate,
        string? Gender,
        double? WeightKg,
        double? HeightCm
        ):IRequest<Guid>;
}
