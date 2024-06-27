namespace Car_Mender.Domain.Common;

public sealed class Result<T>
{
	private Result(bool success, T? value, Error error)
	{
		IsSuccess = success;
		Value = value;
		Error = error;
	}

	public Error Error { get; }
	public T? Value { get; }
	public bool IsSuccess { get; }
	public bool IsFailure => !IsSuccess;

	public static Result<T> Success(T value)
	{
		return new Result<T>(true, value, Error.None);
	}

	public static Result<T> Failure(Error error)
	{
		return new Result<T>(false, default, error);
	}

	public static implicit operator Result<T>(Error error)
	{
		return Failure(error);
	}
}

public class Result
{
	private Result(bool success, Error error)
	{
		IsSuccess = success;
		Error = error;
	}

	public Error Error { get; }
	public bool IsSuccess { get; }
	public bool IsFailure => !IsSuccess;

	public static Result Success()
	{
		return new Result(true, Error.None);
	}

	public static Result Failure(Error error)
	{
		return new Result(false, error);
	}

	public static implicit operator Result(Error error)
	{
		return Failure(error);
	}
}

public sealed record Error(string Code, string? Description = null)
{
	public static readonly Error None = new(string.Empty);
	public static readonly Error Unexpected = new(ErrorCodes.Unexpected, "An unexpected error occured");
	public static readonly Error InvalidId = new(ErrorCodes.InvalidId, "Given id is invalid");

	public static Error ValidationError(IEnumerable<string> errorMessages)
	{
		return new Error(ErrorCodes.ValidationError, string.Join("; ", errorMessages));
	}
}