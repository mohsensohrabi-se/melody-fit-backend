namespace MelodyFit.API.Contracts.Auth
{
    public sealed class RegisterUserRequest
    {
        public string Email { get; init; } = null!;
        public string Password { get; init; } = null!;
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public DateTime? BirthDate { get; init; }
        public string? Gender { get; init; } 
        public double? WeightKg { get; init; }
        public double? HeightCm { get; init; }


    }
}
