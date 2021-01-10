using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MultipleReturnValues
{
    /// <summary>
    /// May either hold an ErrorResult{TError} OR ELSE (if success) hold some other type (with the success result).
    /// </summary>
    [DebuggerDisplay("{Error == null ? Entity : Error,nq}")]
    public class SuccessOrError<TSuccess, TError> : SuccessOrErrorBase<TError>
        where TSuccess : class
        where TError : struct, Enum
    {
        #region Members
        public TSuccess SuccessResult { get; set; }
        #endregion

        #region ctor
        public static SuccessOrError<TSuccess, TError> ValidationError(IList<ValidationError> validationErrors, string errorMessage = "Validation Error")
        {
            return new SuccessOrError<TSuccess, TError>() { ErrorResult = new ErrorResult<TError>(validationErrors, errorMessage) };
        }
        public static SuccessOrError<TSuccess, TError> Fail(TError errorCode, string errorMessage = null)
        {
            return new SuccessOrError<TSuccess, TError>() { ErrorResult = new ErrorResult<TError>(errorCode, errorMessage) };
        }
        public static SuccessOrError<TSuccess, TError> Success(TSuccess entity, string successMessage = "Success")
        {
            return new SuccessOrError<TSuccess, TError>() { SuccessResult = entity, SuccessMessage = successMessage };
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
                return $"Success: {this.SuccessMessage}: {this.SuccessResult.ToString()}";
            else
                return base.ToString();
        }
        #endregion

        #region Implicit conversions from ValueTuple syntax
        public static implicit operator SuccessOrError<TSuccess, TError>(ValueTuple<TSuccess, TError?> tuple)
        {
            if (tuple.Item1 != null && tuple.Item2 == null)
                return Success(tuple.Item1);
            if (tuple.Item1 == null && tuple.Item2 != null)
                return Fail(tuple.Item2.Value);
            throw new NotImplementedException("When error is returned you cannot return any other value together");
        }
        #endregion

        #region Descontruct to ValueTuple
        public void Deconstruct(out TSuccess entity, out ErrorResult<TError> error) => (entity, error) = (this.SuccessResult, this.ErrorResult);
        #endregion


    }
}
