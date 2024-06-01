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

public class Result
{
	public Error Error { get; }
	public bool IsSuccess { get; }
	public bool IsFailure => !IsSuccess;

	private Result(bool success, Error error)
	{
		IsSuccess = success;
		Error = error;
	}

	public static Result Success() => new(true, Error.None);
	public static Result Failure(Error error) => new(false, error);

	public static implicit operator Result(Error error) => Failure(error);
}

public sealed record Error(string Code, string? Description = null)
{
	public static readonly Error None = new(string.Empty);
	public static readonly Error Unexpected = new(ErrorCodes.Unexpected, "An unexpected error occured");
	public static readonly Error InvalidId = new(ErrorCodes.InvalidId, "Given id is invalid");

	public static Error ValidationError(IEnumerable<string> errorMessages) =>
		new(ErrorCodes.ValidationError, string.Join("; ", errorMessages));
}