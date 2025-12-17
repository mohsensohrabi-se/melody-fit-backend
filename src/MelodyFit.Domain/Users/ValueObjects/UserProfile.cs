using MelodyFit.Domain.Common;

namespace MelodyFit.Domain.Users.ValueObjects;

public sealed class UserProfile : ValueObject
{
    public string FirstName { get; }
    public string LastName { get; }
    public DateTime? BirthDate { get; }
    public string? Gender { get; }
    public double? WeightKg { get; }
    public double? HeightCm { get; }

    private UserProfile(
        string firstName,
        string lastName,
        DateTime? birthDate,
        string? gender,
        double? weightKg,
        double? heightCm)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
        Gender = gender;
        WeightKg = weightKg;
        HeightCm = heightCm;
    }

    public static Result<UserProfile> Create(
        string firstName,
        string lastName,
        DateTime? birthDate = null,
        string? gender = null,
        double? weightKg = null,
        double? heightCm = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Result.Failure<UserProfile>("First name is required");

        if (string.IsNullOrWhiteSpace(lastName))
            return Result.Failure<UserProfile>("Last name is required");

        if (birthDate != null && birthDate > DateTime.UtcNow)
            return Result.Failure<UserProfile>("Birth date cannot be in the future");

        if (weightKg is < 20 or > 500)
            return Result.Failure<UserProfile>("Weight is out of realistic range");

        if (heightCm is < 50 or > 300)
            return Result.Failure<UserProfile>("Height is out of realistic range");

        return Result.Success(
            new UserProfile(
                firstName.Trim(),
                lastName.Trim(),
                birthDate,
                gender.Trim(),
                weightKg,
                heightCm));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
        yield return BirthDate;
        yield return Gender;
        yield return WeightKg;
        yield return HeightCm;
    }
}
