using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MultipleReturnValues
{

    /// <summary>
    /// May hold an ErrorResult{TError} if an error occurs
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{ToString()}")]
    public class MaybeError<TError> : ResultOrErrorBase<TError>
        where TError : struct, Enum
    {
        #region Members
        #endregion

        #region ctor
        public static MaybeError<TError> ValidationError(IList<ValidationError> validationErrors, string errorMessage = "Validation Error")
        {
            return new MaybeError<TError>() { ErrorResult = new ErrorResult<TError>(validationErrors, errorMessage) };
        }
        public static MaybeError<TError> Fail(TError errorCode, string errorMessage = null)
        {
            return new MaybeError<TError>() { ErrorResult = new ErrorResult<TError>(errorCode, errorMessage) };
        }
        public static MaybeError<TError> Success(string successMessage = "Success")
        {
            return new MaybeError<TError>() { SuccessMessage = successMessage };
        }
        #endregion

        #region Implicit conversions from/to TError
        public static implicit operator MaybeError<TError>(TError? error)
        {
            //if you do "return (null)", it will be converted to a Success(). "return TError.SomeValue" will be converted to a Fail()
            if (error == null)
                return Success();
            else
                return Fail(error.Value);
        }
        public static implicit operator TError?(MaybeError<TError> result)
        {
            return result?.ErrorResult.ErrorCode;
        }
        #endregion

    }
}
