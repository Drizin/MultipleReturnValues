using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MultipleReturnValues
{

    /// <summary>
    /// May hold an ErrorResult{TError} if an error occurs
    /// </summary>
    [DebuggerDisplay("{Error == null ? \"Success\" : Error,nq}")]
    public class SuccessOrError<TError> : SuccessOrErrorBase<TError>
        where TError : struct, Enum
    {
        #region Members
        #endregion

        #region ctor
        public static SuccessOrError<TError> ValidationError(IList<ValidationError> validationErrors, string errorMessage = "Validation Error")
        {
            return new SuccessOrError<TError>() { ErrorResult = new ErrorResult<TError>(validationErrors, errorMessage) };
        }
        public static SuccessOrError<TError> Fail(TError errorCode, string errorMessage = null)
        {
            return new SuccessOrError<TError>() { ErrorResult = new ErrorResult<TError>(errorCode, errorMessage) };
        }
        public static SuccessOrError<TError> Success(string successMessage = "Success")
        {
            return new SuccessOrError<TError>() { SuccessMessage = successMessage };
        }
        #endregion

        #region Implicit conversions from/to TError
        public static implicit operator SuccessOrError<TError>(TError? error)
        {
            //if you do "return (null)", it will be converted to a Success(). "return TError.SomeValue" will be converted to a Fail()
            if (error == null)
                return Success();
            else
                return Fail(error.Value);
        }
        public static implicit operator TError?(SuccessOrError<TError> result)
        {
            return result?.ErrorResult.ErrorCode;
        }
        #endregion

    }
}
