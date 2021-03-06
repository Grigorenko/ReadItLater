using System;
using System.Collections.Generic;
using System.Linq;

namespace ReadItLater.Core.Infrastructure
{
    public struct Result : IResult
    {
        public bool IsFailure { get; }
        public bool IsSuccess => !IsFailure;

        private readonly IEnumerable<IResultError>? _errors;
        public IEnumerable<IResultError>? Errors => IsFailure ? _errors : throw new ResultSuccessException();

        public Result(bool isFailure = false)
        {
            if (isFailure)
                throw new ArgumentNullException(nameof(Errors), Result.Messages.ErrorObjectIsNotProvidedForFailure);

            IsFailure = isFailure;
            _errors = null;
        }

        public Result(IEnumerable<IResultError> errors)
        {
            if (!errors.Any())
                throw new ArgumentNullException(nameof(errors), Result.Messages.ErrorObjectIsNotProvidedForFailure);

            IsFailure = true;
            _errors = errors;
        }

        // static methods

        public static Result Success()
        {
            return new Result();
        }

        public static Result Failure(string propertyName, string errorMessage)
        {
            return Result.Failure(new DefaultResultError(propertyName, errorMessage));
        }

        public static Result Failure(IResultError error)
        {
            return Result.Failure(new IResultError[] { error });
        }

        public static Result Failure(IEnumerable<IResultError> errors)
        {
            return new Result(errors);
        }

        public static Result<TValue> Success<TValue>(TValue value)
            where TValue : class
        {
            return new Result<TValue>(value);
        }

        public static Result Failure<TException>(TException exception)
            where TException : ICustomResultException
        {
            return Result.Failure(exception.Errors);
        }

        public static Result<TValue> Failure<TValue, TException>(TException error)
            where TValue : class
            where TException : ICustomResultException
        {
            return new Result<TValue>(error.Errors);
        }

        /// <summary>
        /// Return failure result with custom exception. <paramref name="error"/> must implement <see cref="ICustomResultException"/>. For example, <see cref="DataValidationException"/>, <see cref="NotFoundException"/>.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TException">Implement <see cref="ICustomResultException"/>. For example, <see cref="DataValidationException"/>, <see cref="NotFoundException"/>.</typeparam>
        /// <param name="value"></param>
        /// <param name="error">Implement <see cref="ICustomResultException"/>. For example, <see cref="DataValidationException"/>, <see cref="NotFoundException"/>.</param>
        public static Result<TValue> Failure<TValue, TException>(TValue value, TException error)
            where TValue : class
            where TException : ICustomResultException
        {
            return new Result<TValue>(error.Errors);
        }

        internal static class Messages
        {
            public static readonly string ErrorIsInaccessibleForSuccess =
                "You attempted to access the Error property for a successful result. A successful result has no Error.";

            public static readonly string ValueIsInaccessibleForFailure =
                "You attempted to access the Value property for a failed result. A failed result has no Value.";

            public static readonly string ErrorObjectIsNotProvidedForFailure =
                "You attempted to create a failure result, which must have an error, but a null error object (or empty string) was passed to the constructor.";

            public static readonly string ErrorObjectIsProvidedForSuccess =
                "You attempted to create a success result, which cannot have an error, but a non-null error object was passed to the constructor.";
        }
    }

    public partial struct Result<TValue> : IResult, IValue<TValue>
        where TValue : class
    {
        public bool IsFailure { get; }
        public bool IsSuccess => !IsFailure;

        private readonly IEnumerable<IResultError>? _errors;
        public IEnumerable<IResultError>? Errors => IsFailure ? _errors : throw new ResultSuccessException();

        private readonly TValue? _value;
        public TValue? Value => IsSuccess ? _value : throw new ResultFailureException(Errors.Format());

        public Result(TValue value)
        {
            IsFailure = false;
            _errors = null;
            _value = value;
        }

        public Result(IEnumerable<IResultError> errors)
        {
            if (!errors.Any())
                throw new ArgumentNullException(nameof(errors), Result.Messages.ErrorObjectIsNotProvidedForFailure);

            IsFailure = true;
            _errors = errors;
            _value = default;
        }
    }

    public struct Result<TValue, TException> : IResult, IValue<TValue>
        where TValue : class
        where TException : class, ICustomResultException
    {
        public bool IsFailure { get; }
        public bool IsSuccess => !IsFailure;

        private readonly TException? _error;
        public IEnumerable<IResultError>? Errors => IsFailure ? _error?.Errors : throw new ResultSuccessException();

        private readonly TValue? _value;
        public TValue? Value => IsSuccess ? _value : throw new ResultFailureException<TException?>(_error);

        public Result(TValue value)
        {
            IsFailure = false;
            _error = null;
            _value = value;
        }

        public Result(TException error)
        {
            if (error is null)
                throw new ArgumentNullException(nameof(error), Result.Messages.ErrorObjectIsNotProvidedForFailure);

            IsFailure = true;
            _error = error;
            _value = default;
        }
    }
}
