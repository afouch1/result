using System;
using System.Dynamic;
using System.Xml.Serialization;

namespace ResultType
{
    public struct Result<T>
    {
        #region Constructors

        /// <summary>
        /// Constructs a positive return value
        /// </summary>
        /// <param name="value"></param>
        internal Result(T value)
        {
            _value = value;
            IsOk = true;
            ErrorMessage = $"{value} of type {value.GetType()} has no Error";
        }

        /// <summary>
        /// Constructs a negative return value
        /// </summary>
        /// <param name="error"></param>
        internal Result(string error)
        {
            _value = default;
            IsOk = false;
            ErrorMessage = error;
        }
        
        #endregion


        #region Methods

        /// <summary>
        /// Throw an InvalidOperationException if result is an error
        /// </summary>
        /// <param name="errorMessage">Error message for the InvalidOperationException</param>
        /// <returns></returns>
        public T Expect(string errorMessage) => 
            IsOk ? _value : throw new InvalidOperationException(errorMessage);

        /// <summary>
        /// Returns the specified value if the result is an error
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Result<T> ValueOr(T value) => IsOk ? _value : value;

        public static implicit operator Result<T>(T value) => Result.Ok(value);
        
        #endregion
        
        #region Members
        
        private T _value;

        public T Value => 
            IsOk ? _value : throw new InvalidOperationException("Attempted to get value unsuccessfully");

        /// <summary>
        /// Returns the default value if hte result is an error
        /// </summary>
        public T ValueOrDefault => IsOk ? _value : default;

        public readonly string ErrorMessage;

        public readonly bool IsOk;
        
        #endregion
    }

    public static class Result
    {
        /// <summary>
        /// A positive result
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Result<T> Ok<T>(T value) => new Result<T>(value);
        
        /// <summary>
        /// A negative result
        /// </summary>
        /// <param name="error"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Result<T> Error<T>(string error) => new Result<T>(error);

        /// <summary>
        /// Performs a function if the result is not an error
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static Result<T> Then<T, A>(this Result<A> res, Func<A, Result<T>> predicate)
        {
            if (res.IsOk) return predicate(res.Value);
            else return Error<T>(res.ErrorMessage);
        }
    }
}