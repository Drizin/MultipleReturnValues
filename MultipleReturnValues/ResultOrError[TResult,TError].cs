using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MultipleReturnValues
{
    /// <summary>
    /// May either hold an ErrorResult{TError} OR ELSE (if success) hold some other type (with the success result).
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{ToString()}")]
    public class ResultOrError<TResult, TError> : ResultOrErrorBase<TError>
        where TResult : class
        where TError : struct, Enum
    {
        #region Members
        public TResult SuccessResult { get; set; }
        #endregion

        #region ctor
        public static ResultOrError<TResult, TError> ValidationError(IList<ValidationError> validationErrors, string errorMessage = "Validation Error")
        {
            return new ResultOrError<TResult, TError>() { ErrorResult = new ErrorResult<TError>(validationErrors, errorMessage) };
        }
        public static ResultOrError<TResult, TError> Fail(TError errorCode, string errorMessage = null)
        {
            return new ResultOrError<TResult, TError>() { ErrorResult = new ErrorResult<TError>(errorCode, errorMessage) };
        }
        public static ResultOrError<TResult, TError> Success(TResult successResult, string successMessage = "Success")
        {
            return new ResultOrError<TResult, TError>() { SuccessResult = successResult, SuccessMessage = successMessage };
        }
        #endregion

        #region IsSuccess
        public override bool IsSuccess
        {
            get
            {
                if (ErrorResult == null && SuccessResult != null)
                    return true;
                if (ErrorResult != null && SuccessResult == null)
                    return false;
                throw new NotImplementedException("When error is returned you cannot return any other value together");
            }
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            if (this.IsSuccess)
                return $"{base.ToString()}: [{this.SuccessResult.GetType().Name}] {{ {this.SuccessResult.ToString()} }}";
            else
                return base.ToString();
        }
        #endregion

        #region Implicit conversions from ValueTuple syntax
        public static implicit operator ResultOrError<TResult, TError>(ValueTuple<TResult, TError?> tuple)
        {
            if (tuple.Item1 != null && tuple.Item2 == null)
                return Success(tuple.Item1);
            if (tuple.Item1 == null && tuple.Item2 != null)
                return Fail(tuple.Item2.Value);
            throw new NotImplementedException("When error is returned you cannot return any other value together");
        }
        #endregion

        #region Descontruct to ValueTuple
        public void Deconstruct(out TResult result, out ErrorResult<TError> error) => (result, error) = (this.SuccessResult, this.ErrorResult);
        #endregion


    }
}
