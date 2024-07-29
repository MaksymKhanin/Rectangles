using Microsoft.Extensions.Logging;
using System;

#nullable enable
namespace SegmentRectangleIntersection.Models
{
    public class Result
    {
        public bool IsSuccess { get; }
        public Error? Error { get; }
        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, Error? error)
        {
            if (isSuccess && error is not null)
            {
                throw new InvalidOperationException();
            }

            if (!isSuccess && error is null)
            {
                throw new InvalidOperationException();
            }

            IsSuccess = isSuccess;
            Error = error;
        }
        public static Result Success() => new Result(true, null);
        public static Result<T> Success<T>(T value) => new Result<T>(value, true, null);
        public static Result Fail(Error error) => new Result(false, error);
        public static Result<T> Fail<T>(Error error) => new Result<T>(default, false, error);
        public TResult Match<TResult>(Func<TResult> success, Func<Error, TResult> error) => IsSuccess ? success() : error(Error!);

        public static implicit operator Result(Error error) => new Result(false, error);

        public static Error FailAndLog<T>(Error error, ILogger<T> logger)
        {
            logger.LogError(error.Message);
            return error;
        }
    }

    public class Result<T> : Result
    {
        public Result(T? value, bool isSuccess, Error? error) : base(isSuccess, error) => Value = value;
        public T? Value { get; set; }
        public TResult Match<TResult>(Func<T, TResult> success, Func<Error, TResult> error, Func<NotFoundError, TResult> notFound) => IsSuccess ? success(Value!) : Error is NotFoundError ? notFound((NotFoundError)Error) : error(Error!);

        public static implicit operator Result<T>(Error? error) => new Result<T>(default, false, error);
    }
}
