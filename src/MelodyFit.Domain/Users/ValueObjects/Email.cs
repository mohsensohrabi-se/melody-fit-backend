using MelodyFit.Domain.Common;


namespace MelodyFit.Domain.Users.ValueObjects;

public sealed class Email : ValueObject
{
    public string Value { get; }
    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Email>("Email is required");

        value = value.Trim().ToLower();
        try
        {
            var _ = new System.Net.Mail.MailAddress(value);
        }
        catch
        {
            return Result.Failure<Email>("Invalid email format");
        }
        return Result.Success<Email>(new Email(value));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
