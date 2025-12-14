using System;
using System.Collections.Generic;
using System.Text;

namespace MelodyFit.Domain.Common;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; }

    public Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, string.Empty);
    public static Result Failure(string error)
    {
        if(string.IsNullOrWhiteSpace(error))
            throw new ArgumentNullException("Error can not be empty");
        return new(false, error);
    }

    public static Result<T> Success<T>(T value) => new(value, true, string.Empty);
    public static Result<T> Failure<T>(string error) => new(default!, false, error);
}
public class Result<T> : Result
{
    public T Value { get; }
    protected internal Result(T value, bool isSuccess, string error):base(isSuccess, error)
    {
        Value = value;
    }
}
