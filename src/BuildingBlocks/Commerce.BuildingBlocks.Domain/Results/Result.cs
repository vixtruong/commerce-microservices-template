namespace Commerce.BuildingBlocks.Domain.Results
{
    /// <summary>
    /// Kết quả của một thao tác, có thể thành công hoặc thất bại.
    /// </summary>
    public class Result
    {
        protected Result(bool isSuccess, Error? error)
        {
            ArgumentNullException.ThrowIfNull(error);

            if (isSuccess && error != Error.None)
            {
                throw new ArgumentException("A successful result cannot contain an error.", nameof(error));
            }

            if (!isSuccess && error == Error.None)
            {
                throw new ArgumentException("A failed result must contain an error.", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public Error Error { get; }

        public static Result Success() => new Result(true, Error.None);

        public static Result Failure(Error error) => new Result(false, error);

        public TResult Match<TResult>(Func<TResult> onSuccess, Func<Error, TResult> onFailure)
        {
            ArgumentNullException.ThrowIfNull(onSuccess);
            ArgumentNullException.ThrowIfNull(onFailure);

            return IsSuccess ? onSuccess() : onFailure(Error);
        }

        public static implicit operator Result(Error error)
        {
            return Failure(error);
        }
    }

    public sealed class Result<TValue> : Result
    {
        private readonly TValue? _value;

        private Result(
            TValue? value,
            bool isSuccess,
            Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        public TValue Value =>
            IsSuccess
                ? _value!
                : throw new InvalidOperationException("The value of a failed result cannot be accessed.");

        public static Result<TValue> Success(TValue value)
        {
            return value is null
                ? Failure(Error.NullValue)
                : new Result<TValue>(
                    value,
                    true,
                    Error.None);
        }

        public new static Result<TValue> Failure(Error error)
        {
            return new Result<TValue>(
                default,
                false,
                error);
        }

        public TResult Match<TResult>(
            Func<TValue, TResult> onSuccess,
            Func<Error, TResult> onFailure)
        {
            ArgumentNullException.ThrowIfNull(onSuccess);
            ArgumentNullException.ThrowIfNull(onFailure);

            return IsSuccess
                ? onSuccess(Value)
                : onFailure(Error);
        }

        public static implicit operator Result<TValue>(
            TValue value)
        {
            return Success(value);
        }

        public static implicit operator Result<TValue>(
            Error error)
        {
            return Failure(error);
        }
    }
}
