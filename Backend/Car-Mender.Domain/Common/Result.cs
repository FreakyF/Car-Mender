namespace Car_Mender.Domain.Common;

public sealed class Result<T>
{
    public Error Error { get; }
    public T? Value { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    private Result(bool success, T? value, Error error)
    {
        IsSuccess = success;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new(true, value, Error.None);
    public static Result<T> Failure(Error error) => new(false, default, error);
    
    public static implicit operator Result<T>(Error error) => Failure(error);
}

public sealed record Error(string code, string? description = null)
{
    public static readonly Error None = new(string.Empty);
}