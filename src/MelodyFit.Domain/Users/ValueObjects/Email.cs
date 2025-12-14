using MelodyFit.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

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
        if (!value.Contains("@"))
            return Result.Failure<Email>("Invalid Email Format");
        return Result.Success<Email>(new Email(value));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
